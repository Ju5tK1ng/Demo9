using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1 : MonoBehaviour
{
    private float fireSpeed = 36f;
    private float existTime = 0.5f;
    public float damage;
    public float skillDir;

    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = skillDir > 0 ? true : false;
        Invoke("Remove", existTime);
    }

    void Update()
    {
        transform.position += Vector3.right * fireSpeed * skillDir * Time.deltaTime;
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
