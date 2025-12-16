using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
 
public class ClearNavMesh
{
    [MenuItem("Tools/NavMesh/Clear All NavMeshes")]
    static void ClearAll()
    {
        NavMesh.RemoveAllNavMeshData();
        Debug.Log(" Tutti i NavMesh rimossi dalla scena attiva.");
    }
}
 