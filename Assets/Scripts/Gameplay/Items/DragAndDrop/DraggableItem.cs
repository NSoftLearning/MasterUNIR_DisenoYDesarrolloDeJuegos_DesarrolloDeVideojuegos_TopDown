using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Events")]
    [SerializeField] private UnityEvent OnDropNullEvent;

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

        image.sprite = itemData.ItemIcon;
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

        transform.SetParent(_canvas.transform);
        transform.SetAsLastSibling();

        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);
        transform.localPosition = Vector3.zero;

        image.raycastTarget = true;

        if (!_wasDroppedSuccessfully)
        {
            OnDropNullEvent.Invoke();
        }
    }

    public void MarkAsDroppedSuccessfully()
    {
        _wasDroppedSuccessfully = true;
    }
}