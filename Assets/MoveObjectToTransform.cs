using UnityEngine;

public class MoveObjectToTransform : MonoBehaviour
{
    public Transform targetTransform;

    public void MoveToTransform()
    {
        //Calculate the size of this game object
        Vector3 size = Vector3.zero;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            size += renderer.bounds.size;
        }
        // Adjust the position to center the object at the target transform
        Vector3 offset = new Vector3(size.x / 2, size.y / 2, size.z / 2);
        this.transform.position = targetTransform.position - offset;
        this.transform.rotation = targetTransform.rotation;
    }
}
