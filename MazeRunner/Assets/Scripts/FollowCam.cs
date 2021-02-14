using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z) + offset;
    }
}
