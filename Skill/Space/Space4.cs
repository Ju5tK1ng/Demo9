using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space4 : MonoBehaviour
{
    public Transform anotherSpace4;
    void Start()
    {

    }

    void Update()
    {

    }

    public void RemoveLater(float time)
    {
        Invoke("Remove", time);
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "FlySkill" && other.GetComponent<FlySkill>().Space4Timer <= 0)
        {
            other.GetComponent<FlySkill>().Space4Timer = 0.1f;
            other.transform.position += (anotherSpace4.position - transform.position);
        }
    }
}
