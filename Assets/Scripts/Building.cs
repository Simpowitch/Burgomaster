using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] Material constructed = null;
    bool isConstructing = true;
    [SerializeField] int turnsToBuild = 2;
    int remainingTurnsToBuild;
    [SerializeField] Transform canvasTransform = null;
    [SerializeField] Bar progressBar = null;

    private void Start()
    {
        TurnManager.OnNewTurnBegun += NewTurn;
        remainingTurnsToBuild = turnsToBuild;
        canvasTransform.rotation = Quaternion.identity;
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

    private void FinishConstruction()
    {
        isConstructing = false;
        spriteRenderer.material = constructed;
    }
}
