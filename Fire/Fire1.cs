using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1 : MonoBehaviour
{
    // private Rigidbody2D fireRigidbody;
    public float fireSpeed = 16f;
    public float existTime = 2f;
    public float damage = 1f;
    private float skillDir;
    void Start()
    {
        // fireRigidbody = GetComponent<Rigidbody2D>();
        skillDir = -(transform.rotation.z - 0.5f) / 0.5f;
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
