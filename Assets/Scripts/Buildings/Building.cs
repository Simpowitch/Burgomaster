using UnityEngine;
using System;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    protected Action OnCompletion;
    public UnityEvent OnCompletionInspector;

    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Material constructed = null;
    protected bool isFinished = false;
    [SerializeField] ProjectConstructionSlot[] constructionSlots = null;
    int turnsToBuild = 2;
    int remainingTurnsToBuild;
    [SerializeField] Transform canvasTransform = null;
    [SerializeField] Bar progressBar = null;
    [SerializeField] ConstructionPlacer constructionPlacer = null;
    [SerializeField] Rigidbody2D rb = null;

    protected Resource[] income = null, upkeep = null;

    protected Player player;

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
        income = new Resource[project.income.Length];
        for (int i = 0; i < project.income.Length; i++)
        {
            income[i] = project.income[i].Copy();
        }
        upkeep = new Resource[project.upkeep.Length];
        for (int i = 0; i < project.upkeep.Length; i++)
        {
            upkeep[i] = project.upkeep[i].Copy();
        }
        turnsToBuild = project.turnsToComplete;
        remainingTurnsToBuild = turnsToBuild;

        SetThemeIndex(themeIndex);

        foreach (var slot in constructionSlots)
        {
            slot.gameObject.SetActive(true);
        }

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

        player.IncreaseTurnIncome(income);
        player.IncreaseTurnExpenses(upkeep);
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

    
}
