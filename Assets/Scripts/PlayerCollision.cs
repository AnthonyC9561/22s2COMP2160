using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    private void OnCollisionStay(Collision collision)
    {//when player lands on ground or platform, reset jump state
        player.OnGround = true;
        player.Jump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstantStrings.Platform))
        {//make player child of platform to make player move with the platform.
            transform.SetParent(collision.gameObject.transform);
            player.OnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstantStrings.Platform))
        {
            transform.parent = null;
            player.OnPlatform = false;
        }
        //if player is off a layer, cannot jump
        player.OnGround = false;
    }

    private void OnTriggerEnter(Collider other)
    { //Sets player on teleporter to true when player enters teleporter collider
        if (other.gameObject.CompareTag(ConstantStrings.Teleporter))
        {
            player.OnTeleporter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    { //Sets player on teleporter to true when player exits teleporter collider
        if (other.gameObject.CompareTag(ConstantStrings.Teleporter))
        {
            player.OnTeleporter = false;
        }
    }
}
