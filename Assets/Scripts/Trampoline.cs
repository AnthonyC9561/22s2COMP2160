using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceForce = 10f;
    private Vector3 bounce;

    private void Start()
    {
        bounce = new Vector3(0f, bounceForce, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {//when colliding with player and above the trampoline, add upward impulse onto player
        if (collision.transform.position.y > transform.position.y)
        {
            Rigidbody playerBody = collision.gameObject.GetComponent<Rigidbody>();
            playerBody.AddForce(bounce, ForceMode.Impulse);
        }
    }
}
