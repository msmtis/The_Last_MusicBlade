using UnityEngine;

public class HealthManager : MonoBehaviour
{

    private bool checkpoint1reached = false;
    private bool checkpoint2reached = false;
    private bool checkpoint3reached = false;
    public string charactername;
    public int maxHealth;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(UIManager.Instance.DEBUGETXT);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.tag + " ha subito " + damage + " danni. Vita rimanente: " + currentHealth);
        float percentage = ((float)currentHealth / maxHealth);
        UIManager.Instance.UpdateLifeBar(percentage, charactername);



        if (gameObject.CompareTag("Enemy"))
        {
            if (percentage < 0.75 & !checkpoint1reached)
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                checkpoint1reached = true;
                FightManager.Instance.cardChosing = true;
                FightManager.Instance.soundTrackManager.CheckPoint1();
                


            }
            if (percentage < 0.5 & !checkpoint2reached)
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                checkpoint2reached = true;
                FightManager.Instance.cardChosing = true;
                FightManager.Instance.soundTrackManager.CheckPoint2();
            }
            if (percentage < 0.25 & !checkpoint3reached)
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                checkpoint3reached = true;
                FightManager.Instance.cardChosing = true;
                FightManager.Instance.soundTrackManager.CheckPoint3();
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " è morto!");

        // Chiama Game Over solo se è il giocatore principale
        if (gameObject.CompareTag("Character"))
        {
            FightManager.Instance.GameOver();
        }

        if (gameObject.CompareTag("Enemy"))
        {
            FightManager.Instance.GameWon();
        }

        
    }

    
}