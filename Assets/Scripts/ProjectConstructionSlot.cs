using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ProjectConstructionSlot : MonoBehaviour, IPointerClickHandler
{
    public Image portrait;
    public TextMeshProUGUI queueText;

    public bool IsFilledAndActive => AssignedAdvisor != null && activelyWorkedOn;

    Advisor advisor;
    public Advisor AssignedAdvisor
    {
        get => advisor;
        set
        {
            advisor = value;
            if (value != null)
            {
                portrait.sprite = value.portrait;
                portrait.enabled = true;
            }
            else
            {
                portrait.enabled = false;
                queueText.text = "";
            }
        }
    }
    bool activelyWorkedOn;
    public bool ActivelyWorkedOn
    {
        private get => activelyWorkedOn;
        set
        {
            activelyWorkedOn = value;
            float alphaTranspherency = value ? 1 : 0.25f;
            portrait.color = new Color(1f, 1f, 1f, alphaTranspherency);
        }
    }

    public bool ProjectCompleted { get; set; } = false;

    public void UpdateQueueText(int queueNumber)
    {
        queueText.enabled = queueNumber > 0;
        queueText.text = queueNumber.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (AdvisorManager.SelectedAdvisor == null)
                return;

            if (AssignedAdvisor != null)
                AssignedAdvisor.RemoveProject(this);
            AssignedAdvisor = AdvisorManager.SelectedAdvisor;
            AssignedAdvisor.AddProject(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Remove this
            if (AssignedAdvisor != null)
            {
                AssignedAdvisor.RemoveProject(this);
                AssignedAdvisor = null;
            }
        }
    }
}
