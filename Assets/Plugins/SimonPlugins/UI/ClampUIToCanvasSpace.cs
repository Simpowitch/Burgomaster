using UnityEngine;

public class ClampUIToCanvasSpace : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform canvasRect = null;
    [SerializeField] RectTransform updatePositionRect = null;
    [SerializeField] RectTransform panelSizeRect = null;

    private Transform followTransform;

    [Header("Settings")]
    [SerializeField] bool followMouse = false;
    [SerializeField] Vector3 offset = new Vector3(25, 25);

    [Header("Padding")]
    public float up, right, down, left; 

    public void SetupFollowTransform(Transform follow)
    {
        this.followTransform = follow;
    }

    // Update is called once per frame
    void Update()
    {
        if (followMouse)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition + offset, null, out Vector2 localPoint);
            transform.localPosition = localPoint;
        }
        else if (followTransform)
        {
            Vector2 inputPoint = Camera.main.WorldToScreenPoint(followTransform.position) + offset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), inputPoint, null, out Vector2 localPoint);

            transform.localPosition = localPoint;
        }

        Vector2 anchoredPosition = updatePositionRect.anchoredPosition;
        if (anchoredPosition.y + panelSizeRect.rect.height + up > canvasRect.rect.height) //Top edge
        {
            anchoredPosition.y = canvasRect.rect.height - panelSizeRect.rect.height - up;
        }
        if (anchoredPosition.x + panelSizeRect.rect.width + right > canvasRect.rect.width) //Right edge
        {
            anchoredPosition.x = canvasRect.rect.width - panelSizeRect.rect.width - right;
        }
        if (anchoredPosition.y - down < 0) //Bottom edge
        {
            anchoredPosition.y = 0 + down;
        }
        if (anchoredPosition.x -left < 0) //Left edge
        {
            anchoredPosition.x = 0 + left;
        }
        updatePositionRect.anchoredPosition = anchoredPosition;
    }
}
