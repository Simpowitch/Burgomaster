using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer visuals = null;
    Spawner constructionPlacer = null;
    Rigidbody2D rb = null;

    [SerializeField] protected Material constructedMaterial = null;
    public Sprite[] themes = null;
    public int ThemeIndex { get; private set; } = 0;
    public bool HasThemes => themes != null && themes.Length > 0;

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
