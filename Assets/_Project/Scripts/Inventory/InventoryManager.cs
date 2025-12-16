using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    [SerializeField] private ItemListSO _itemList;

    private Dictionary<int, ItemDetails> _itemDetailsDictionary;

    private void Start()
    {
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
}