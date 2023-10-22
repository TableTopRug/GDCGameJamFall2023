using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private Club curClub;
    [SerializeField] private float invulnerableTime = 0.5f;
    [SerializeField] private HealthBar hpBar;

    public int maxHealth = 5;
    public int health;
    private float invulnerableTimer;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            curClub.swingClub();
        }
    }

    private void FixedUpdate()
    {
        invulnerableTimer -= Time.deltaTime;
    }

    public void takeDamage()
    {
        if(invulnerableTimer <= 0)
        {
            invulnerableTimer = invulnerableTime;
            health--;
            hpBar.updateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                //do something
            }
        }
    }

    public void heal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);
        hpBar.updateHealthBar(health, maxHealth);
    }
}
