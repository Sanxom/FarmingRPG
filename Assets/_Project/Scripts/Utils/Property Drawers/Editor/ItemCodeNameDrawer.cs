using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(ItemCodeNameAttribute))]
public class ItemCodeNameDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginChangeCheck();

            int newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height * 0.5f), label, property.intValue);

            EditorGUI.LabelField(new Rect(position.x, position.y + (position.height * 0.5f), position.width, position.height * 0.5f), "Item Name", GetItemName(property.intValue));

            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }
        EditorGUI.EndProperty();
    }

    private string GetItemName(int itemCode)
    {
        ItemListSO itemListSO;

        itemListSO = AssetDatabase.LoadAssetAtPath("Assets/_Project/Scriptable Object Assets/Item/ItemListSO.asset", typeof(ItemListSO)) as ItemListSO;

        List<ItemDetails> itemDetailsList = itemListSO.ItemDetailsList;
        ItemDetails itemDetails = itemDetailsList.Find(x => x.itemCode == itemCode);

        return itemDetails != null ? itemDetails.itemName : "";
    }
}