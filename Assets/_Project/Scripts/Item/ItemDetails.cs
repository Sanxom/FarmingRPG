using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public Sprite itemSprite;
    public ItemType itemType;
    public string itemLongDescription;
    public string itemDescription;
    public float itemUseRadius;
    public int itemCode;
    public short itemUseGridRadius;
    public bool isStartingItem;
    public bool canBePickedUp;
    public bool canBeDropped;
    public bool canBeEaten;
    public bool canBeCarried;
}