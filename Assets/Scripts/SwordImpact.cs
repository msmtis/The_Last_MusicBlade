using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class SwordImpact : MonoBehaviour
{
    /*
     * 
     * 
     * 
     */
    private int damage = 20;
    private Collider swordCollider;
    public string TAG_tocheck;

    void Start()
    {
        swordCollider = GetComponent<Collider>();
    }
        private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider" + other.gameObject.tag+"  "+other.gameObject.name);
        if (swordCollider.enabled && other.gameObject.CompareTag(TAG_tocheck))
        {
            other.GetComponent<HealthManager>().TakeDamage(damage);
            Debug.Log(gameObject.tag + " ha colpito il nemico!");
            Debug.Log(other.gameObject.tag);
           

            swordCollider.enabled = false;
        }
        
        
    }


    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public int getDamage()
    {
        return damage;
    }
}


