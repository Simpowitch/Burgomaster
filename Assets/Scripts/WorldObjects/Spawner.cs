using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public bool CanSpawn { get; private set; } = false;
    [SerializeField] SpriteRenderer visuals = null;
    [SerializeField] Material allowed = null, forbidden = null;
    public string TooltipExplanation { get; private set; }

    List<Collider2D> collisions = new List<Collider2D>();

    protected virtual void Start()
    {
        //Visually display if can be spawned or not
        visuals.material = CanSpawn ? allowed : forbidden;
    }

    private void FixedUpdate()
    {
        CheckCanSpawn();
    }

    public abstract void Spawn(Blueprint blueprint, Transform spawnParent, Player player);

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
        visuals.material = CanSpawn ? allowed : forbidden;
    }

    public abstract void ChangeTheme(bool next);
    public abstract int CurrentTheme { get; }

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
