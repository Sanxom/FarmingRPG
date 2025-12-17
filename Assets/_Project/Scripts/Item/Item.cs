using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer _sr;
    [field: ItemCodeName][field: SerializeField] public int ItemCode { get; set; }

    // Mason's Additional Code
    [field: SerializeField] public bool IsStackable { get; private set; }
    // Mason's Additional Code

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (ItemCode == 0) return;

        Init(ItemCode);
    }

    public void Init(int itemCode)
    {
        if (itemCode == 0) return;

        ItemCode = itemCode;

        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);
        _sr.sprite = itemDetails.itemSprite;

        if (itemDetails.itemType == ItemType.ReapableScenery)
        {
            ItemNudge temp = gameObject.AddComponent<ItemNudge>();
            temp.ObjectToRotate = temp.transform.GetChild(0);
        }
    }
}