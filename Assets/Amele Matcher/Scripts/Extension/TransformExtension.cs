using UnityEngine;
public static class TransformExtension
{
   public static void Clear(this Transform transform)
   {
       while (transform.childCount > 0)
       {
           Transform t = transform.GetChild(0);
           t.SetParent(null);
           Object.Destroy(t.gameObject);
       }
   }
}
