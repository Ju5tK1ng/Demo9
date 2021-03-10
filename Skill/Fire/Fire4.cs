using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire4 : MonoBehaviour
{
    private float existTime = 0.1f;
    void Start()
    {
        Invoke("Remove", existTime);
    }

    void Update()
    {

    }

    void Remove()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "CreatedTile")
        {
            other.GetComponent<CreatedTile>().DamageTile(999f);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            other.GetComponent<DestroyTile>().DestroyOneTile(transform.position);
        }
    }
}
