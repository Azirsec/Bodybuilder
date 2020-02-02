using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    [SerializeField] int scene;

    [SerializeField] Image healthbar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        float width = (float)health / (float)maxHealth;
        print(width);

        healthbar.rectTransform.sizeDelta = new Vector2(width * 200, 20);

        if (transform.position.y < -5)
        {
            takeDamage(100);
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        print("Player Health: " + health);
        if (health <= 0)
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void groundSlammed(int damage)
    {
        if (GetComponent<PlayerMovement>().getGrounded())
        {
            takeDamage(damage);
        }
    }
}
