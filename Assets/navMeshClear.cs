using UnityEngine;
using UnityEngine.AI;

public class NavMeshClean : MonoBehaviour
{
    void Awake()
    {
        NavMesh.RemoveAllNavMeshData();
        Debug.Log(" NavMesh runtime rimosso all'avvio.");
    }
}