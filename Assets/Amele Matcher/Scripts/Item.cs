using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]

public class Item : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Renderer render;
    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = render.material;
    }
    public void DisableShadow()
    {
        // implement shadow disabling logic here
    }
    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    public void Select(Material outlineMaterial)
    {
        render.materials = new Material[] { baseMaterial, outlineMaterial };
    }
    public void Deselect()
    {
        render.materials = new Material[] { baseMaterial };
    }
}
