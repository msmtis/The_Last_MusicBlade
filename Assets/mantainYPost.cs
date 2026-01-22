using UnityEngine;

public class FollowXZOnly : MonoBehaviour
{
    public Transform target; // personaggio da seguire
    private float fixedY;    // altezza fissa del cubo

    void Start()
    {
        fixedY = transform.position.y; // salva Y iniziale
    }

    void Update()
    {
        // Posizione solo X/Z del personaggio, Y rimane fissa
        Vector3 newPos = new Vector3(target.position.x, fixedY, target.position.z);
        transform.position = newPos;
    }
}