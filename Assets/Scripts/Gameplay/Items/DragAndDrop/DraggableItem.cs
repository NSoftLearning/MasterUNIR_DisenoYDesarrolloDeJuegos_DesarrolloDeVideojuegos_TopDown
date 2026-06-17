using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Transform _parentAfterDrag;
    private bool _wasDroppedSuccessfully;

    public ItemSO ItemData { get; private set; }
    public int InventoryIndex { get; private set; }

    public void Initialize(ItemSO itemData, int inventoryIndex)
    {
        ItemData = itemData;
        InventoryIndex = inventoryIndex;

        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (image != null && itemData != null)
        {
            image.sprite = itemData.ItemIcon;
        }
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _wasDroppedSuccessfully = false;
        _parentAfterDrag = transform.parent;

        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        if (_canvas != null)
        {
            transform.SetParent(_canvas.transform, true);
            transform.SetAsLastSibling();
        }

        if (image != null)
        {
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_rectTransform == null || _canvas == null)
            return;

        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_parentAfterDrag != null)
        {
            transform.SetParent(_parentAfterDrag, false);
            transform.localPosition = Vector3.zero;
        }

        if (image != null)
        {
            image.raycastTarget = true;
        }
    }

    public void MarkAsDroppedSuccessfully()
    {
        _wasDroppedSuccessfully = true;
    }
}