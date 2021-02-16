﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class Bar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Linear Bar")]
    public static void AddLinearBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Linear Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
    [MenuItem("GameObject/UI/Radial Bar")]
    public static void AddRadialBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Radial Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif


    [SerializeField] int minimumValue;
    [SerializeField] int maximumValue;
    [SerializeField] int currentValue;

    [SerializeField] Image mask = null;
    [SerializeField] Image fill = null;
    [SerializeField] Text currentValueText = null;

    [SerializeField] bool hideWhenFull = false;
    [SerializeField] Image[] visuals = null;

    [SerializeField] float animationTime = 0.5f;

    public Color color;

    ProgressStatus currentStatus;

    private void Update()
    {
        SetCurrentFill();
        SetColor();
        ShowCurrentValue();

        if (hideWhenFull)
            Show(currentValue < maximumValue);
    }

    private void OnEnable()
    {
        SetNewValues(currentStatus);
    }

    void SetColor()
    {
        fill.color = color;
    }

    void SetCurrentFill()
    {
        float currentOffset = currentValue - minimumValue;
        float maximumOffset = maximumValue - minimumValue;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
    }

    void Show(bool show)
    {
        foreach (var item in visuals)
        {
            if (show != item.enabled)
                item.enabled = show;
        }
    }

    public void SetNewValues(float percentageFactor) => SetNewValues(new ProgressStatus(percentageFactor));

    public void SetNewValues(ProgressStatus newStatus)
    {
        Show(true);
        StopAllCoroutines();
        currentStatus = newStatus;
        maximumValue = newStatus.newMaximum;
        minimumValue = newStatus.newMinimum;
        if (this.gameObject.activeInHierarchy)
            StartCoroutine(ChangeCurrentOverTime(newStatus.newCurrent, animationTime));
    }

    IEnumerator ChangeCurrentOverTime(int targetValue, float animationTime)
    {
        //animatingChange = true;
        float timer = 0;
        float t = 0;
        int startCurrent = currentValue;

        while (timer < animationTime && currentValue < maximumValue)
        {
            timer += Time.deltaTime;
            t = timer / animationTime;
            currentValue = Mathf.RoundToInt(Mathf.Lerp(startCurrent, targetValue, t));
            yield return null;
        }
        currentValue = targetValue;
    }

    private void ShowCurrentValue()
    {
        if (currentValueText != null)
        {
            currentValueText.text = currentValue.ToString() + " / " + maximumValue.ToString();
        }
    }

    public struct ProgressStatus
    {
        public readonly int newCurrent;
        public readonly int newMaximum;
        public readonly int newMinimum;

        public ProgressStatus(float percentageFactor)
        {
            this.newCurrent = Mathf.RoundToInt(percentageFactor * 100);
            this.newMaximum = 100;
            this.newMinimum = 0;
        }

        public ProgressStatus(int newCurrent, int newMaximum, int newMinimum = 0)
        {
            this.newCurrent = newCurrent;
            this.newMaximum = newMaximum;
            this.newMinimum = newMinimum;
        }
    }
}
