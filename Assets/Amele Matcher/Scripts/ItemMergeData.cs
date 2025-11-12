using UnityEngine;
using System.Collections.Generic;

public struct ItemMergeData
{
    public string itemName;
    public List<Item> items;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.name;

        items = new List<Item>();
        items.Add(firstItem);
    }
}
