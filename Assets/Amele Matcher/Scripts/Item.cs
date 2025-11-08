using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]

public class Item : MonoBehaviour
{
    public void DisableShadow()
    {
        // implement shadow disabling logic here
    }
    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }   
}
