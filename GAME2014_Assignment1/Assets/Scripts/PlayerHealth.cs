using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;

    [Header("Heart Sprites (5)")]
    [SerializeField] GameObject[] hearts; 

    void Start()
    {
        currentHealth = maxHealth;

     
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }

      
        UpdateHearts();
    }

   
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHearts();

        if (currentHealth <= 0)
        {
          
            SceneManager.LoadScene("GameOver"); 
        }
    }

    void UpdateHearts()
    {
       
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }

}
