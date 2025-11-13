using System;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpotParent;
    private ItemSpot[] spots;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    private bool isBusy;

    [Header("Data")]
    private Dictionary<EItemName, ItemMergeData> itemMergeDataDictionary = new Dictionary<EItemName, ItemMergeData>();

    private void Awake()
    {
        InputManager.itemClicked += OnItemClicked;

        StoreSpots();
    }
    private void OnDestroy()
    {
        InputManager.itemClicked -= OnItemClicked;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnItemClicked(Item item)
    {
        if(isBusy)
        {
            Debug.LogWarning("ItemSpotsManager is busy! Amele seni...");
            return;
        }
        if (!IsFreeSpotsAvailable())
        {
            Debug.LogWarning("No free item spots available! Game Over AMELEEE!");
            return;
        }

        // Simdi busy oldugumuz icin item ile senaryomuzu isleyebiliriz
        isBusy = true;


        HandleItemClicked(item);
        
    }

    private void HandleItemClicked(Item item)
    {
        if (itemMergeDataDictionary.ContainsKey(item.ItemName))
            HandleItemMergeDataFound(item);
        else
            MoveItemFirstFreeSpot(item);
    }

    private void HandleItemMergeDataFound(Item item)
    {
        ItemSpot idealSpot = GetIdealSpotFor(item);

        itemMergeDataDictionary[item.ItemName].Add(item);

        TryMoveItemToIdealSpot(item, idealSpot);
    }

    private ItemSpot GetIdealSpotFor(Item item)
    {
        List<Item> items = itemMergeDataDictionary[item.ItemName].items;
        List<ItemSpot> itemSpots = new List<ItemSpot>();

        for (int i = 0; i < items.Count; i++)
            itemSpots.Add(items[i].Spot);

        // itemler gibi kullanilan spotların da listesi var artık

        // eger bir spot varsa, basitce siradakini kullanmali.. abv!
        if (itemSpots.Count >= 2)
            itemSpots.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));

        int IdealSpotIndex = itemSpots[0].transform.GetSiblingIndex() + 1;

        return spots[IdealSpotIndex];
    }
    
    private void TryMoveItemToIdealSpot(Item item, ItemSpot idealSpot)
    {
        if (!idealSpot.isEmpty())
        {
            HandleIdealSpotFull(item, idealSpot);
            return;
        }
        
        MoveItemToSpot(item, idealSpot);
    }

    private void MoveItemToSpot(Item item, ItemSpot targetSpot, bool checkForMerge = true)
    {
        targetSpot.Populate(item);
        
        // sonra 2, scale the item down and set its local position to 0,0,0
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;

        // 3, disable the shadow of the item
        item.DisableShadow();

        // 4, disable the collider of the item to prevent further clicks + physics featues
        item.DisablePhysics();

        HandleItemReachedSpot(item, checkForMerge);
    }

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true)
    {
        if(!checkForMerge)
            return;
         
        if (itemMergeDataDictionary[item.ItemName].CanMergeItems())
            MergeItems(itemMergeDataDictionary[item.ItemName]);
        else
            CheckForGameOver();
    }

    private void MergeItems(ItemMergeData itemMergeData)
    {
        List<Item> items = itemMergeData.items;

        // removing items from dictionary ama spotlar da clearlenmeli
        itemMergeDataDictionary.Remove(itemMergeData.itemName);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Spot.Clear();
            Destroy(items[i].gameObject);
        }

        // itemleri sola dayama eklendikten sonra burayı silecegiz
        isBusy = false;
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot)
    {
        MoveAllItemsToTheRight(idealSpot, item);
    }

    private void MoveAllItemsToTheRight(ItemSpot idealSpot, Item itemToPlace)
    {
        int spotIndex = idealSpot.transform.GetSiblingIndex();

        for (int i = spots.Length - 2; i >= spotIndex; i--)
        {

            ItemSpot spot = spots[i];

            //double check burayi
            if (spot.isEmpty())
                continue;

            Item item = spot.Item;

            spot.Clear();

            ItemSpot targetSpot = spots[i + 1];

            if (!targetSpot.isEmpty())
            {
                Debug.LogError("Error, this should not happen! Amele!");
                isBusy = false;
                return;
            }

            MoveItemToSpot(item, targetSpot, false);
        }

        MoveItemToSpot(itemToPlace, idealSpot);
    }

    private void MoveItemFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null)
        {
            Debug.LogError("Target spot is null => Napiyon AMELE!");
            return;
        }

        CreateItemMergeData(item);
        
        targetSpot.Populate(item);
        

        // sonra 2, scale the item down and set its local position to 0,0,0
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;

        // 3, disable the shadow of the item
        item.DisableShadow();

        // 4, disable the collider of the item to prevent further clicks + physics featues
        item.DisablePhysics();

        HandleFirstItemReachedSpot(item);
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (GetFreeSpot() == null)
            Debug.LogWarning("Game Over AMELE!");
        else
            isBusy = false;
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.ItemName, new ItemMergeData(item));
    }

    private ItemSpot GetFreeSpot()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].isEmpty())
                return spots[i];
        }
        return null;
    }

    private void StoreSpots()
    {
        spots = new ItemSpot[itemSpotParent.childCount];

        for (int i = 0; i < itemSpotParent.childCount; i++)
            spots[i] = itemSpotParent.GetChild(i).GetComponent<ItemSpot>();
    }
    private bool IsFreeSpotsAvailable()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].isEmpty())
                return true;
        }
        return false;
    }
}
