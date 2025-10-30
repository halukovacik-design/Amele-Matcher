using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseDown();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            HandleMouseDown();
    }
    
    private void HandleMouseDown()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100);

        if (hit.collider == null)
            return;
        
            Debug.Log("We've clicked on : " + hit.collider.name);
        
    }
}
