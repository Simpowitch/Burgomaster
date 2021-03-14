using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Advisor
{
    public string nameOfCharacter;
    public Sprite portrait;
    public Sprite shield;
    //public int force, cunning, diplomacy, knowledge, survival, infrastructure;

    List<ProjectConstructionSlot> projectQueue = new List<ProjectConstructionSlot>();

    public void AddProject(ProjectConstructionSlot projectToAdd)
    {
        if (projectQueue.Contains(projectToAdd))
            return;

        projectQueue.Add(projectToAdd);
        UpdateQueue();
    }

    public void RemoveProject(ProjectConstructionSlot projectToRemove)
    {
        if (projectQueue.Contains(projectToRemove))
        {
            projectToRemove.AssignedAdvisor = null;
            projectToRemove.ActivelyWorkedOn = false;
            projectQueue.Remove(projectToRemove);
        }
        else
            Debug.LogError("Tried to remove a project not active in this leader");
        UpdateQueue();
    }

    public void TurnUpdate()
    {
        UpdateQueue();
    }


    private void UpdateQueue()
    {
        for (int i = 0; i < projectQueue.Count; i++)
        {
            if (projectQueue[i].ProjectCompleted)
            {
                projectQueue.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < projectQueue.Count; i++)
        {
            projectQueue[i].AssignedAdvisor = this;
            projectQueue[i].UpdateQueueText(i);
            projectQueue[i].ActivelyWorkedOn = i == 0;
        }
    }
}
