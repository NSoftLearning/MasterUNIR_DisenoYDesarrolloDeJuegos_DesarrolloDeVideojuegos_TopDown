using TMPro;
using UnityEngine;

public class ItemTooltipUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform tooltipPanel;
    [SerializeField] private TMP_Text itemNameText;

    [Header("Settings")]
    [SerializeField] private Vector2 offset = new Vector2(15f, -15f);

    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private bool isVisible;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        Hide();
    }

    private void Update()
    {
        if (!isVisible)
            return;

        FollowMouse();
    }

    public void Show(string itemName)
    {
        if (tooltipPanel == null || itemNameText == null)
            return;

        itemNameText.text = itemName;
        tooltipPanel.gameObject.SetActive(true);

        isVisible = true;
        FollowMouse();
    }

    public void Hide()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.gameObject.SetActive(false);
        }

        isVisible = false;
    }

    private void FollowMouse()
    {
        if (canvas == null || canvasRectTransform == null || tooltipPanel == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        tooltipPanel.anchoredPosition = localPoint + offset;
    }
}