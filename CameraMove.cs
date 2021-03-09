using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform player;
    private float sceneX;
    private float sceneY;

    private const float MoveSpeed = 200f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        sceneX = Mathf.RoundToInt(player.position.x / 40f);
        sceneY = Mathf.RoundToInt(player.position.y / 22.5f);
        Vector3 newPosition = this.transform.position;
        newPosition.x = PlayerControl3.Approach(newPosition.x, sceneX * 40f, MoveSpeed * Time.deltaTime);
        newPosition.y = PlayerControl3.Approach(newPosition.y, sceneY * 22f, MoveSpeed * Time.deltaTime);
        this.transform.position = newPosition;
    }
}
