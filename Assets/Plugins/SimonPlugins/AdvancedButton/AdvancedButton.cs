using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AdvancedButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Clicks")]
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    [Header("Hoovers")]
    public UnityEvent mouseEnter;
    public UnityEvent mouseExit;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick?.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Middle)
            middleClick?.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseExit?.Invoke();
    }
}
