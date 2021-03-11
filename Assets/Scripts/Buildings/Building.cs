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
    protected Resource[] income, upkeep;


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

        income = projectInfo.income.Copy();
        upkeep = projectInfo.upkeep.Copy();

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
                    FinishConstruction();
                break;
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

        player.ChangeTurnIncomes(income, true);
        player.ChangeTurnExpenses(upkeep, true);
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


    protected abstract void Select();
    protected abstract void DeSelect();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (projectInfo)
        {
            Debug.Log("Building selected: " + projectInfo.name);
            SelectedBuilding = this;
        }
    }
}
