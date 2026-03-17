using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class RemoveAllScripts : MonoBehaviour
{
    public bool remove;

    private void OnValidate()
    {
        if (remove)
        {
            Remove();
            remove = false;
        }
    }

    public void Remove()
    {
        var components = GetComponents<BoxCollider>()
            .Where(c => c != transform) // keep Transform
            .ToList();

        foreach (var comp in components)
        {
            Destroy(comp);
        }
    }
}