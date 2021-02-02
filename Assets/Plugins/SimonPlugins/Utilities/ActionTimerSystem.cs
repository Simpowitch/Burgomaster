using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionTimerSystem : MonoBehaviour
{
    static List<ActionTimer> actionTimers = new List<ActionTimer>();
    public static void AddActionTimer(ActionTimer newTimer) => actionTimers.Add(newTimer);
    public static void RemoveActionTimer(ActionTimer finishedTimer)
    {
        if (actionTimers.Contains(finishedTimer))
            actionTimers.Remove(finishedTimer);
    }

    private void Update()
    {
        if (actionTimers.Count <= 0)
        {
            return;
        }
        float time = Time.deltaTime;
        for (int i = 0; i < actionTimers.Count; i++)
        {
            if (actionTimers[i] != null)
                actionTimers[i].Update(time);
        }
    }
}

public class ActionTimer
{
    Action actionOnTimerEnd;
    float startTime;
    float remainingTime;
    bool running;
    bool invokeRepeatingly;
    public bool IsFinished { get => remainingTime <= 0; }
    public float Progress { get => 1 - (remainingTime / startTime); }

    public ActionTimer(float time, Action actionOnTimerEnd, bool startRunning, bool invokeRepeatingly)
    {
        startTime = time;
        remainingTime = time;
        this.actionOnTimerEnd = actionOnTimerEnd;
        this.running = startRunning;
        this.invokeRepeatingly = invokeRepeatingly;
        ActionTimerSystem.AddActionTimer(this);
    }

    public void DestroyMe()
    {
        actionOnTimerEnd = null;
        ActionTimerSystem.RemoveActionTimer(this);
    }

    public void PlayPause(bool play) => running = play;

    public void Update(float timeChange)
    {
        if (!running)
            return;
        remainingTime -= timeChange;
        if (IsFinished)
        {
            actionOnTimerEnd?.Invoke();
            if (invokeRepeatingly)
            {
                remainingTime += startTime;
            }
            else
            {
                actionOnTimerEnd = null;
                ActionTimerSystem.RemoveActionTimer(this);
            }
        }
    }
}
