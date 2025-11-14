using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Item : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EItemName itemName;
    public EItemName ItemName => itemName;

    private ItemSpot spot;
    public ItemSpot Spot => spot;
    
    [Header("Elements")]
    [SerializeField] private Renderer render;
    [SerializeField] private Collider collid;
    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = render.material;
    }

    public void AssignSpot(ItemSpot spot)
        => this.spot = spot;

    public void DisableShadow()
    {
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        collid.enabled = false;
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
