using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedTile : MonoBehaviour
{
    public float HP = 0;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void DamageTile(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
