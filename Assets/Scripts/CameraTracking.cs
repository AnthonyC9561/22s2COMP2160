using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraDelay;
    private Vector3 offset;

    void FixedUpdate() //used FixedUpdate to prevent camera jittering
    {//moves position of camera pivot to centre camera at player with delay
        offset = Vector3.Lerp(transform.position, player.position, cameraDelay);
        transform.position = offset;
    }
}
