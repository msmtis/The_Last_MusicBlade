using UnityEngine;

public class followSettings : MonoBehaviour
{
    public Transform character; // Il personaggio da seguire
    public float followSpeed = 10f; // Velocità di inseguimento

    void LateUpdate()
    {
        if (character != null)
        {
            // Segue solo la posizione del personaggio senza il movimento del roll
            Vector3 targetPosition = character.position;

            // Interpolazione per rendere il movimento più fluido
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}