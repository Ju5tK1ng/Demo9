using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Enemy
{
    float health = 4f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
