using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    public List<InventoryItem>[] inventoryListsArray;

    // The index of this array is the inventory list (from InventoryLocation); The value of that index is the capacity of that List
    [HideInInspector] public int[] inventoryListCapacityIntArray;

    [SerializeField] private ItemListSO _itemList;

    private Dictionary<int, ItemDetails> _itemDetailsDictionary;

    protected override void Awake()
    {
        base.Awake();

        CreateInventoryLists();

        CreateItemDetailsDictionary();
    }

    /// <summary>
    /// Returns the ItemDetails (from the ItemListSO) for the itemCode or null if the item code doesn't exist
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemDetails GetItemDetails(int itemCode)
    {
        if (_itemDetailsDictionary.TryGetValue(itemCode, out ItemDetails itemDetails))
            return itemDetails;
        else
            return null;
    }

    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryListsArray[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
                return i;
        }

        return -1;
    }

    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject objectToDestroy)
    {
        AddItem(inventoryLocation, item);
        Destroy(objectToDestroy);
        // TODO: Return the object to its respective Pool later here instead of destroying
    }

    /// <summary>
    /// Add an Item to the inventory list for the inventory location
    /// </summary>
    /// <param name="inventoryLocation"></param>
    /// <param name="item"></param>
    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryListsArray[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1 && item.IsStackable)
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        else
            AddItemAtPosition(inventoryList, itemCode);

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryListsArray[(int)inventoryLocation]);
    }

    /// <summary>
    /// Adds an item to the specified position of the specified inventory list (for stackable items)
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    /// <param name="itemPosition"></param>
    public void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode,  int itemPosition)
    {
        int quantity = inventoryList[itemPosition].itemQuantity + 1;

        InventoryItem inventoryItem = new()
        {
            itemQuantity = quantity,
            itemCode = itemCode,
        };

        inventoryList[itemPosition] = inventoryItem;
        //DebugPrintInventoryList(inventoryList);
    }

    /// <summary>
    /// Adds a new item to the end of the specified inventory list
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemCode"></param>
    public void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new()
        {
            itemCode = itemCode,
            itemQuantity = 1
        };
        inventoryList.Add(inventoryItem);

        //DebugPrintInventoryList(inventoryList);
    }

    private void CreateInventoryLists()
    {
        inventoryListsArray = new List<InventoryItem>[(int)InventoryLocation.Count];

        for (int i = 0; i < (int)InventoryLocation.Count; i++)
        {
            inventoryListsArray[i] = new();
        }

        // Initialize capacity array
        inventoryListCapacityIntArray = new int[(int)InventoryLocation.Count];

        // initialize Player inventory capacity
        inventoryListCapacityIntArray[(int)InventoryLocation.Player] = Settings.playerInitialInventoryCapacity;
    }

    /// <summary>
    /// Populates the _itemDetailsDictionary from the ScriptableObject items list
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        _itemDetailsDictionary = new();

        foreach (ItemDetails itemDetails in _itemList.ItemDetailsList)
        {
            _itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    {
        foreach (InventoryItem inventoryItem in inventoryList)
        {
            print($"Item Name: {GetItemDetails(inventoryItem.itemCode).itemName}\nItem Quantity: {inventoryItem.itemQuantity}");
        }
        print("*********************************************");
    }
}