using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionPlacer : MonoBehaviour
{
    bool canSpawn = false;
    [SerializeField] GameObject blueprint = null;
    MeshRenderer[] renderers;
    public Cost cost;
    [SerializeField] Material allowed = null, forbidden = null;

    public string TooltipExplanation { get; private set; }

    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        //Visually display if can be spawned or not
        foreach (var renderer in renderers)
        {
            renderer.material = canSpawn ? allowed : forbidden;
        }
    }

    private void FixedUpdate()
    {
        CheckCanSpawn();
    }

    public bool ConfirmSpawn()
    {
        CheckCanSpawn();
        if (canSpawn)
        {
            Instantiate(blueprint, this.transform.position, this.transform.rotation, this.transform.parent);
            return true;
        }
        else
        {
            Debug.Log("Can't spawn");
            return false;
        }
    }

    private void CheckCanSpawn()
    {
        bool freeOfCollisions = collisions.Count == 0;
        string explanation = "";

        if (!freeOfCollisions)
            explanation = Utility.AddTextOnNewLine(explanation, $"Colliding with: {collisions[0].gameObject.name}");

        ChangeCanSpawn(freeOfCollisions, explanation);
    }

    private void ChangeCanSpawn(bool newState, string explanation)
    {
        TooltipExplanation = explanation;

        //No change
        if (newState == canSpawn)
            return;

        canSpawn = newState;

        //Visually display if can be spawned or not
        foreach (var renderer in renderers)
        {
            renderer.material = canSpawn ? allowed : forbidden;
        }
    }

    List<Collider2D> collisions = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisions.Contains(collision))
            return;
        collisions.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisions.Remove(collision);
    }
}
