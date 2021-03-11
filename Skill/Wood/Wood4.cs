using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood4 : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 startPosition;
    public Vector3 endPosition;
    public float pushTime;
    public float pullTime;
    private float curTime;
    public Transform playerTransform;
    public PlayerControl3 player;
    void Start()
    {
        startPosition = playerTransform.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        curTime = 0;
    }

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime < pushTime)
        {
            lineRenderer.SetPosition(1, Vector3.Lerp(startPosition, endPosition, curTime / pushTime));
        }
        else if (curTime - pushTime < pullTime)
        {
            player.playState = PlayerControl3.PlayState.Jump;
            playerTransform.position = Vector3.Lerp(startPosition, endPosition, (curTime - pushTime) / pullTime);
            lineRenderer.SetPosition(0, playerTransform.position);
        }
        else
        {
            player.woodSkill4 = false;
            Destroy(gameObject);
        }
    }
}
