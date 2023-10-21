using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private Club curClub;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int health;
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
        
    }

    public void takeDamage()
    {
        health--;
        if(health <= 0)
        {
            //do something
        }
    }

    public void heal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth);
    }
}
