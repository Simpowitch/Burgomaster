using System.Collections.Generic;
using UnityEngine;

public class ConstructionPlacer : MonoBehaviour
{
    public bool CanSpawn { get; private set; } = false;
    MeshRenderer[] renderers;
    [SerializeField] Material allowed = null, forbidden = null;
    public string TooltipExplanation { get; private set; }

    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        //Visually display if can be spawned or not
        foreach (var renderer in renderers)
        {
            renderer.material = CanSpawn ? allowed : forbidden;
        }
    }

    private void FixedUpdate()
    {
        CheckCanSpawn();
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
        if (newState == CanSpawn)
            return;

        CanSpawn = newState;

        //Visually display if can be spawned or not
        foreach (var renderer in renderers)
        {
            renderer.material = CanSpawn ? allowed : forbidden;
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
