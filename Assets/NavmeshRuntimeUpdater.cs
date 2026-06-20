using NavMeshPlus.Components;
using UnityEngine;

public class NavmeshRuntimeUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _surface;

    [ContextMenu("Rebuild mesh")]
    public void Rebuild()
    {

        _surface.UpdateNavMesh(_surface.navMeshData);
    }
}



