using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<Item> itemClicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseDown();
    }
    
    private void HandleMouseDown()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100);

        if (hit.collider == null)
            return;

        if (!hit.collider.TryGetComponent(out Item item))
            return;

        Debug.Log("We've clicked on : " + hit.collider.name);
            
        itemClicked?.Invoke(item);
        
    }
}
