using UnityEngine;
using System;

public class Building : MonoBehaviour
{
    protected Action OnCompletion;

    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Material constructed = null;
    protected bool isFinished = false;
    bool isConstructing = true;
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
        TurnManager.OnTurnBegunLate += NewTurn;
    }

    protected virtual void OnDisable()
    {
        TurnManager.OnTurnBegunLate -= NewTurn;
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

        UpdateProgress();
    }

    private void NewTurn(object sender, TurnManager.OnTurnEventArgs e)
    {
        if (isConstructing)
        {
            remainingTurnsToBuild--;
            if (remainingTurnsToBuild == 0)
            {
                FinishConstruction();
            }
            UpdateProgress();
        }
    }

    private void UpdateProgress()
    {
        int turnsBuilt = turnsToBuild - remainingTurnsToBuild;
        float progressNormalized = turnsBuilt * 1f / turnsToBuild;
        progressBar.SetNewValues(progressNormalized);
    }

    protected virtual void FinishConstruction()
    {
        TurnManager.OnTurnBegunLate -= NewTurn;
        OnCompletion?.Invoke();

        isFinished = true;
        isConstructing = false;
        spriteRenderer.material = constructed;

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
        spriteRenderer.sprite = themes[index];
        ThemeIndex = index;
    }
}
