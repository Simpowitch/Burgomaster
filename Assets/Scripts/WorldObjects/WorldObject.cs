using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WorldObject : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer visuals = null;
    Spawner constructionPlacer = null;
    Rigidbody2D rb = null;

    [SerializeField] protected Material constructedMaterial = null;
    public Sprite[] themes = null;
    public int ThemeIndex { get; private set; } = 0;
    public bool HasThemes => themes != null && themes.Length > 0;
    public float despawnTime = 0.5f;

    public UnityEvent OnCompletionEvents, OnDespawnEvents;


    protected virtual void Awake()
    {
        constructionPlacer = GetComponent<Spawner>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Setup(Player player, Blueprint project, int themeIndex)
    {
        Destroy(constructionPlacer);
        Destroy(rb);
        this.gameObject.layer = 0;

        ChangeTheme(themeIndex);
    }

    public virtual void Despawn()
    {
        Destroy(this.gameObject, despawnTime);
        OnDespawnEvents?.Invoke();
    }

    public void ChangeTheme(bool next)
    {
        if (HasThemes)
        {
            int previousIndex = ThemeIndex;
            visuals.sprite = next ? themes.Next(ref previousIndex) : themes.Previous(ref previousIndex);
            ThemeIndex = previousIndex;
        }
    }

    public void ChangeTheme(int index)
    {
        if (themes == null || index >= themes.Length)
            return;
        visuals.sprite = themes[index];
        ThemeIndex = index;
    }
}
