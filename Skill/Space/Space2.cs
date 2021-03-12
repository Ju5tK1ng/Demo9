using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space2 : MonoBehaviour
{
    private Animator animator;
    public Transform playerTransform;
    private float existTime = 0.5f;
    public float damage;
    private bool changePosion;

    void Start()
    {
        animator = GetComponent<Animator>();
        changePosion = false;
    }

    void Update()
    {
        if (existTime > 0)
        {
            if (existTime < 0.2f && !changePosion)
            {
                changePosion = true;
                transform.position = playerTransform.position - Vector3.right * Mathf.Sign(playerTransform.localScale.x) * 3f;
                animator.SetBool("appear", true);
            }
            existTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("1");
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
