using System;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpot;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

    private void Awake()
    {
        InputManager.itemClicked += OnItemClicked;
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
        // deneyecegim ilk sey 1, turn the item into a child of itemSpot
        item.transform.SetParent(itemSpot);

        // sonra 2, scale the item down and set its local position to 0,0,0
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;

        // 3, disable the shadow of the item
        item.DisableShadow();

       // 4, disable the collider of the item to prevent further clicks + physics featues
        item.DisablePhysics();
    }
}
