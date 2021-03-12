using System.Collections.Generic;
using UnityEngine;

public class ConstructionPlacer : MonoBehaviour
{
    public bool CanSpawn { get; private set; } = false;
    [SerializeField] SpriteRenderer[] renderers = null;
    [SerializeField] Material allowed = null, forbidden = null;
    public string TooltipExplanation { get; private set; }
    [SerializeField] Building building = null;

    public int CurrentTheme => building.ThemeIndex;

    private void Start()
    {
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

    public void ChangeTheme(bool next) => building.ChangeTheme(next);

    List<Collider2D> collisions = new List<Collider2D>();

    public void SetActive(bool value)
    {
        if (value != this.gameObject.activeSelf)
            this.gameObject.SetActive(value);
    }

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
