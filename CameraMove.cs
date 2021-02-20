using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform player;
    private float sceneNum;

    private const float MoveSpeed = 200f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        sceneNum = Mathf.RoundToInt(player.position.x / 40f);
        Vector3 newPosition = this.transform.position;
        newPosition.x = PlayerControl3.Approach(newPosition.x, sceneNum * 40f, MoveSpeed * Time.deltaTime);
        this.transform.position = newPosition;
    }
}
