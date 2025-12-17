using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite blank16x16Sprite;
    [SerializeField] private UIInventorySlot[] inventorySlotArray;

    private RectTransform _rectTransform;
    private Image _image;
    private float _moveSpeed = 0.2f;
    
    public bool IsInventoryBarPositionBottom { get; private set; }
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void Update()
    {
        SwitchInventoryBarPosition();
    }

    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    private IEnumerator MoveCoroutine(Vector2 targetPivot, Vector2 targetAnchorMin, Vector2 targetAnchorMax, Vector2 targetPos, float time)
    {
        IsMoving = true;
        Vector2 currentPivot = _rectTransform.pivot;
        Vector2 currentAnchorMin = _rectTransform.anchorMin;
        Vector2 currentAnchorMax = _rectTransform.anchorMax;
        Vector2 currentPos = _rectTransform.anchoredPosition;
        _image.color = new(_image.color.r, _image.color.g, _image.color.b, 0.05f);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            _rectTransform.pivot = Vector2.Lerp(currentPivot, targetPivot, t);
            _rectTransform.anchorMin = Vector2.Lerp(currentAnchorMin, targetAnchorMin, t);
            _rectTransform.anchorMax = Vector2.Lerp(currentAnchorMax, targetAnchorMax, t);
            _rectTransform.anchoredPosition = Vector2.Lerp(currentPos, targetPos, t);
            yield return null;
        }

        _rectTransform.pivot = targetPivot;
        _rectTransform.anchorMin = targetAnchorMin;
        _rectTransform.anchorMax = targetAnchorMax;
        _rectTransform.anchoredPosition = targetPos;
        _image.color = new(_image.color.r, _image.color.g, _image.color.b, 1f);
        IsInventoryBarPositionBottom = !IsInventoryBarPositionBottom;
        IsMoving = false;
    }

    private void SwitchInventoryBarPosition()
    {
        if (IsMoving) return;

        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();

        Vector2 targetPivot;
        Vector2 targetAnchorMin;
        Vector2 targetAnchorMax;
        Vector2 targetPos;

        switch (playerViewportPosition.y)
        {
            case > 0.3f when !IsInventoryBarPositionBottom:
                targetPivot = new(0.5f, 0f);
                targetAnchorMin = new(0.5f, 0f);
                targetAnchorMax = new(0.5f, 0f);
                targetPos = new(0f, 2.5f);
                //_rectTransform.pivot = new(0.5f, 0f);
                //_rectTransform.anchorMin = new(0.5f, 0f);
                //_rectTransform.anchorMax = new(0.5f, 0f);
                //_rectTransform.anchoredPosition = new(0f, 2.5f);
                StartCoroutine(MoveCoroutine(targetPivot, targetAnchorMin, targetAnchorMax, targetPos, _moveSpeed));
                break;
            case <= 0.3f when IsInventoryBarPositionBottom:
                targetPivot = new(0.5f, 1f);
                targetAnchorMin = new(0.5f, 1f);
                targetAnchorMax = new(0.5f, 1f);
                targetPos = new(0f, -2.5f);
                //_rectTransform.pivot = new(0.5f, 1f);
                //_rectTransform.anchorMin = new(0.5f, 1f);
                //_rectTransform.anchorMax = new(0.5f, 1f);
                StartCoroutine(MoveCoroutine(targetPivot, targetAnchorMin, targetAnchorMax, targetPos, _moveSpeed));
                break;
            default:
                break;
        }
    }

    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if (inventoryLocation == InventoryLocation.Player)
        {
            ClearInventorySlots();

            if (inventorySlotArray.Length == 0 || inventoryList.Count == 0) return;

            for (int i = 0; i < inventorySlotArray.Length; i++)
            {
                if (i >= inventoryList.Count) break;

                int itemCode = inventoryList[i].itemCode;

                ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                if (itemDetails == null) continue;

                inventorySlotArray[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                inventorySlotArray[i].itemCountText.text = $"{inventoryList[i].itemQuantity}";
                inventorySlotArray[i].itemDetails = itemDetails;
                inventorySlotArray[i].itemQuantity = inventoryList[i].itemQuantity;
            }
        }
    }

    private void ClearInventorySlots()
    {
        if (inventorySlotArray.Length == 0) return;

        for(int i = 0; i < inventorySlotArray.Length; i++)
        {
            inventorySlotArray[i].inventorySlotImage.sprite = blank16x16Sprite;
            inventorySlotArray[i].itemCountText.text = "";
            inventorySlotArray[i].itemDetails = null;
            inventorySlotArray[i].itemQuantity = 0;
        }
    }
}