using System;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpotParent;
    private ItemSpot[] spots;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

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
        if (!IsFreeSpotsAvailable())
        {
            Debug.LogWarning("No free item spots available! Game Over AMELEEE!");
            return;
        }

        HandleItemClicked(item);
        
    }

    private void HandleItemClicked(Item item)
    {
        MoveItemFirstFreeSpot(item);
    }

    private void MoveItemFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null)
        {
            Debug.LogError("Target spot is null => Napiyon AMELE!");
            return;
        }

        targetSpot.Populate(item);
        

        // sonra 2, scale the item down and set its local position to 0,0,0
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;

        // 3, disable the shadow of the item
        item.DisableShadow();

        // 4, disable the collider of the item to prevent further clicks + physics featues
        item.DisablePhysics();
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
