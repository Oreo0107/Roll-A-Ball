using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowScript : MonoBehaviour
{
    public Transform player;
    public float yOffset = 12;

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = new Vector3();
        temp.x = player.position.x;
        temp.z = player.position.z;
        temp.y = yOffset;
        transform.position = temp;
    }
}
