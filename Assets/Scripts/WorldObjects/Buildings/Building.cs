using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class Building : WorldObject
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
    private Action OnLevelUpCompleted;

    [Header("References")]
    [SerializeField] ProjectConstructionSlot[] constructionSlots = null;
    [SerializeField] GameObject[] levelObjects = null;

    Bar progressBar = null;
    Transform canvasTransform = null;

    protected bool isFinished = false;
    int turnsToBuild = 2;
    int remainingTurnsToBuild;

    protected Player player;
    protected BuildingBlueprint projectInfo;

    #region CurrentStats
    public string MyName { protected set; get; }
    public Sprite InspectorSprite { protected set; get; }
    public Resource[] Income { protected set; get; }
    public Resource[] Upkeep { protected set; get; }
    public Effect[] CurrentEffects { protected set; get; }

    protected float effectivity = 1f;

    #endregion
    #region Levels
    int level;
    protected bool CanLevelUp => isFinished && projectInfo.levelUpBlueprint && player.IsAffordable(projectInfo.levelUpBlueprint.cost);

    public string NextLevelName { private set; get; }
    public Sprite NextLevelInspectorSprite { private set; get; }
    public Resource[] NextLevelIncome { private set; get; }
    public Resource[] NextLevelUpkeep { private set; get; }
    public Effect[] NextLevelEffects { private set; get; }
    public Resource[] LevelUpCost { private set; get; }
    #endregion

    protected virtual void OnEnable()
    {
        TurnManager.OnUpdateConstruction += UpdateConstruction;
    }

    protected virtual void OnDisable()
    {
        TurnManager.OnUpdateConstruction -= UpdateConstruction;
    }

    protected override void Awake()
    {
        base.Awake();
        progressBar = GetComponentInChildren<Bar>();
        canvasTransform = GetComponentInChildren<Canvas>().transform;
    }

    public override void Setup(Player player, Blueprint blueprint, int themeIndex)
    {
        base.Setup(player, blueprint, themeIndex);

        this.player = player;
        projectInfo = blueprint as BuildingBlueprint;

        turnsToBuild = projectInfo.turnsToComplete;
        remainingTurnsToBuild = turnsToBuild;

        foreach (var slot in constructionSlots)
        {
            slot.gameObject.SetActive(true);
        }
        canvasTransform.rotation = Quaternion.identity;

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
        visuals.material = constructedMaterial;

        foreach (var slot in constructionSlots)
        {
            slot.ProjectCompleted = true;
            slot.gameObject.SetActive(false);
        }

        LevelUp();
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
            player.RemoveResources(LevelUpCost);

            projectInfo = projectInfo.levelUpBlueprint;
            UpdateStats();
        }
        else
            UpdateStats(false);

        if (levelObjects != null && level > 0)
        {
            levelObjects[level].SetActive(false);
            levelObjects[level + 1].SetActive(true);
        }

        level++;

        OnLevelUpCompleted?.Invoke();
    }

    private void UpdateStats(bool removeOld = true)
    {
        if (isFinished && removeOld)
        {
            player.ChangeTurnIncomes(Income, false);
            player.ChangeTurnExpenses(Upkeep, false);
            UseEffects(false);
        }

        MyName = projectInfo.buildingName;
        InspectorSprite = projectInfo.uiSprite;
        Income = projectInfo.income.Copy(effectivity);
        Upkeep = projectInfo.upkeep.Copy(1f / effectivity);
        CurrentEffects = projectInfo.completionEffects.Copy(effectivity);

        if (isFinished)
        {
            player.ChangeTurnIncomes(Income, true);
            player.ChangeTurnExpenses(Upkeep, true);
            UseEffects(true);
        }

        BuildingBlueprint nextLevelProject = projectInfo.levelUpBlueprint;
        if (nextLevelProject)
        {
            NextLevelName = nextLevelProject.buildingName;
            NextLevelInspectorSprite = nextLevelProject.uiSprite;
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
        OnLevelUpCompleted += SetupBuildingInspector;

        visuals.material.SetInt(Shader.PropertyToID("_ShowOutline"), 1);
        visuals.material.SetInt(Shader.PropertyToID("_IsPulsing"), 0);
    }
    protected virtual void DeSelect()
    {
        BuildingInspector.instance.Show(false);
        OnLevelUpCompleted -= SetupBuildingInspector;

        visuals.material.SetInt(Shader.PropertyToID("_ShowOutline"), 0);
    }

    private void SetupBuildingInspector()
    {
        if (projectInfo.HasLevelUpBlueprint)
            BuildingInspector.instance.SetupUpgradeable(this, LevelUp, CanLevelUp);
        else
            BuildingInspector.instance.SetupDefault(this);
    }

    private void OnMouseDown()
    {
        if (projectInfo)
        {
            Debug.Log("Building selected: " + projectInfo.name);
            SelectedBuilding = this;
        }
    }

    void OnMouseEnter()
    {
        if (this != SelectedBuilding)
        {
            visuals.material.SetInt(Shader.PropertyToID("_IsPulsing"), 1);
            visuals.material.SetInt(Shader.PropertyToID("_ShowOutline"), 1);
        }
    }

    void OnMouseExit()
    {
        if (this != SelectedBuilding)
        {
            visuals.material.SetInt(Shader.PropertyToID("_IsPulsing"), 0);
            visuals.material.SetInt(Shader.PropertyToID("_ShowOutline"), 0);
        }
    }
}