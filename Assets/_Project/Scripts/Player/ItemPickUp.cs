using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Item item))
        {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            if (!itemDetails.canBePickedUp) return;

            InventoryManager.Instance.AddItem(InventoryLocation.Player, item, collision.gameObject);
        }
    }
}