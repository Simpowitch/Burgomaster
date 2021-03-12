using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class Building : MonoBehaviour, IPointerClickHandler
{
    private static Building selectedBuilding;
    public static Building SelectedBuilding
    {
        get => selectedBuilding;
        set
        {
            if (selectedBuilding)
                selectedBuilding.DeSelect();
            selectedBuilding = value;
            if (selectedBuilding)
                selectedBuilding.Select();
        }
    }

    protected Action OnCompletion;
    public UnityEvent OnCompletionInspector;

    [Header("References")]
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Material constructed = null;
    [SerializeField] ProjectConstructionSlot[] constructionSlots = null;
    [SerializeField] Transform canvasTransform = null;
    [SerializeField] Bar progressBar = null;
    [SerializeField] ConstructionPlacer constructionPlacer = null;
    [SerializeField] Rigidbody2D rb = null;


    protected bool isFinished = false;
    int turnsToBuild = 2;
    int remainingTurnsToBuild;

    protected Player player;
    protected Project projectInfo;

    #region CurrentStats
    public string MyName { protected set; get; }
    public Sprite InspectorSprite { protected set; get; }
    public Resource[] Income { protected set; get; }
    public Resource[] Upkeep { protected set; get; }
    public Effect[] CurrentEffects { protected set; get; }

    protected float effectivity = 1f;

    #endregion
    #region Levels
    public int level;
    protected bool CanLevelUp => isFinished && projectInfo.levelUpProject && player.IsAffordable(projectInfo.levelUpProject.cost);

    public string NextLevelName { private set; get; }
    public Sprite NextLevelInspectorSprite { private set; get; }
    public Resource[] NextLevelIncome { private set; get; }
    public Resource[] NextLevelUpkeep { private set; get; }
    public Effect[] NextLevelEffects { private set; get; }
    public Resource[] LevelUpCost { private set; get; }
    #endregion

    [SerializeField] Sprite[] themes = null;
    public int ThemeIndex { get; private set; } = 0;
    public bool HasThemes => themes != null && themes.Length > 0;

    protected virtual void OnEnable()
    {
        TurnManager.OnUpdateConstruction += UpdateConstruction;
    }

    protected virtual void OnDisable()
    {
        TurnManager.OnUpdateConstruction -= UpdateConstruction;
    }

    public virtual void Setup(Player player, Project project, int themeIndex)
    {
        Destroy(constructionPlacer);
        Destroy(rb);
        this.gameObject.layer = 0;
        canvasTransform.rotation = Quaternion.identity;

        this.player = player;
        projectInfo = project;

        turnsToBuild = project.turnsToComplete;
        remainingTurnsToBuild = turnsToBuild;

        SetThemeIndex(themeIndex);

        foreach (var slot in constructionSlots)
        {
            slot.gameObject.SetActive(true);
        }

        UpdateStats();

        DisplayConstructionProgress();
    }

    private void UpdateConstruction(object sender, TurnManager.OnTurnEventArgs e)
    {
        foreach (var slot in constructionSlots)
        {
            if (slot.IsFilledAndActive)
            {
                remainingTurnsToBuild--;
                if (remainingTurnsToBuild == 0)
                {
                    FinishConstruction();
                    break;
                }
            }
        }
        DisplayConstructionProgress();
    }

    private void DisplayConstructionProgress()
    {
        int turnsBuilt = turnsToBuild - remainingTurnsToBuild;
        float progressNormalized = turnsBuilt * 1f / turnsToBuild;
        progressBar.SetNewValues(progressNormalized);
    }

    protected virtual void FinishConstruction()
    {
        TurnManager.OnUpdateConstruction -= UpdateConstruction;
        OnCompletion?.Invoke();
        OnCompletionInspector?.Invoke();

        isFinished = true;
        spriteRenderer.material = constructed;

        foreach (var slot in constructionSlots)
        {
            slot.ProjectCompleted = true;
            slot.gameObject.SetActive(false);
        }

        LevelUp();
    }

    public void ChangeTheme(bool next)
    {
        if (HasThemes)
        {
            int previousIndex = ThemeIndex;
            spriteRenderer.sprite = next ? themes.Next(ref previousIndex) : themes.Previous(ref previousIndex);
            ThemeIndex = previousIndex;
        }
    }

    private void SetThemeIndex(int index)
    {
        if (themes == null || index >= themes.Length)
            return;
        spriteRenderer.sprite = themes[index];
        ThemeIndex = index;
    }

    protected virtual void UseEffects(bool apply)
    {
        if (apply)
        {
            foreach (var effect in CurrentEffects)
            {
                switch (effect.type)
                {
                    case Effect.Type.Authority:
                        player.ChangeAbilityScore(AbilityScore.Authority, effect.effectValue);
                        break;
                    case Effect.Type.Cunning:
                        player.ChangeAbilityScore(AbilityScore.Cunning, effect.effectValue);
                        break;
                    case Effect.Type.Diplomacy:
                        player.ChangeAbilityScore(AbilityScore.Diplomacy, effect.effectValue);
                        break;
                    case Effect.Type.Knowledge:
                        player.ChangeAbilityScore(AbilityScore.Knowledge, effect.effectValue);
                        break;
                    case Effect.Type.Nature:
                        player.ChangeAbilityScore(AbilityScore.Nature, effect.effectValue);
                        break;
                    case Effect.Type.Piety:
                        player.ChangeAbilityScore(AbilityScore.Piety, effect.effectValue);
                        break;
                    case Effect.Type.Population:
                        player.IncreasePopulation(effect.effectValue);
                        break;
                    case Effect.Type.ProductivityPercentage:
                        Debug.LogWarning("Unhandled effecttype for Building" + gameObject.name);
                        break;
                }
            }
        }
        else
        {
            foreach (var effect in CurrentEffects)
            {
                switch (effect.type)
                {
                    case Effect.Type.Authority:
                        player.ChangeAbilityScore(AbilityScore.Authority, -effect.effectValue);
                        break;
                    case Effect.Type.Cunning:
                        player.ChangeAbilityScore(AbilityScore.Cunning, -effect.effectValue);
                        break;
                    case Effect.Type.Diplomacy:
                        player.ChangeAbilityScore(AbilityScore.Diplomacy, -effect.effectValue);
                        break;
                    case Effect.Type.Knowledge:
                        player.ChangeAbilityScore(AbilityScore.Knowledge, -effect.effectValue);
                        break;
                    case Effect.Type.Nature:
                        player.ChangeAbilityScore(AbilityScore.Nature, -effect.effectValue);
                        break;
                    case Effect.Type.Piety:
                        player.ChangeAbilityScore(AbilityScore.Piety, -effect.effectValue);
                        break;
                    case Effect.Type.Population:
                        player.DecreasePopulation(effect.effectValue);
                        break;
                    case Effect.Type.ProductivityPercentage:
                        Debug.LogWarning("Unhandled effecttype for Building" + gameObject.name);
                        break;
                }
            }
        }
    }

    protected void LevelUp()
    {
        if (level > 0)
        {
            projectInfo = projectInfo.levelUpProject;
            UpdateStats();
            player.RemoveResources(LevelUpCost);
        }
        else
            UpdateStats(false);

        level++;
    }

    private void UpdateStats(bool removeOld = true)
    {
        if (isFinished && removeOld)
        {
            player.ChangeTurnIncomes(Income, false);
            player.ChangeTurnExpenses(Upkeep, false);
            UseEffects(false);
        }

        MyName = projectInfo.projectName;
        InspectorSprite = projectInfo.sprite;
        Income = projectInfo.income.Copy(effectivity);
        Upkeep = projectInfo.upkeep.Copy(1f / effectivity);
        CurrentEffects = projectInfo.completionEffects.Copy(effectivity);

        if (isFinished)
        {
            player.ChangeTurnIncomes(Income, true);
            player.ChangeTurnExpenses(Upkeep, true);
            UseEffects(true);
        }

        Project nextLevelProject = projectInfo.levelUpProject;
        if (nextLevelProject)
        {
            NextLevelName = nextLevelProject.projectName;
            NextLevelInspectorSprite = nextLevelProject.sprite;
            NextLevelEffects = nextLevelProject.completionEffects.Copy(effectivity);
            NextLevelIncome = nextLevelProject.income.Copy(effectivity);
            NextLevelUpkeep = nextLevelProject.upkeep.Copy(1f / effectivity);
            LevelUpCost = nextLevelProject.cost;
        }
        else
        {
            NextLevelName = null;
            NextLevelInspectorSprite = null;
            NextLevelEffects = null;
            NextLevelIncome = null;
            NextLevelUpkeep = null;
            LevelUpCost = null;
        }
    }

    public void ChangeEffectivity(float change)
    {
        effectivity += change;
        UpdateStats();
    }

    protected virtual void Select()
    {
        SetupBuildingInspector();
        BuildingInspector.instance.Show(true);
        player.OnEconomyChanged += SetupBuildingInspector;
    }
    protected virtual void DeSelect()
    {
        BuildingInspector.instance.Show(false);
        player.OnEconomyChanged -= SetupBuildingInspector;
    }

    private void SetupBuildingInspector()
    {
        if (projectInfo.HasLevelUpProject)
            BuildingInspector.instance.SetupUpgradeable(this, LevelUp, CanLevelUp);
        else
            BuildingInspector.instance.SetupDefault(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (projectInfo)
        {
            Debug.Log("Building selected: " + projectInfo.name);
            SelectedBuilding = this;
        }
    }
}
