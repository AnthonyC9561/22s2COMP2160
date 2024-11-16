using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private float playerTurnSpeed = 4f;
    [SerializeField] private float jumpMaxHeight = 1.2f; //max jump height
    [SerializeField] private float terminalVelocity = 20f; //max falling speed
    [SerializeField] private float fallDeathPosition = -10f; //y coord that causes player death when exceeded.

    //use of rigidbody to simulate movement of player using physics 
    private Rigidbody playerRigidbody;

    //input values
    private Vector3 jumpForce;
    private Vector3 playerMovement;
    private float horizontal;
    private float vertical;
    private bool jump = false; //whether or not player jumped
    public bool Jump
    {
        set
        {
            jump = value;
        }
    }
    //collision states
    private bool onTeleporter = false;
    public bool OnTeleporter
    {//to check if player is on teleporter or not
        set
        {
            onTeleporter = value;
        }
    }

    private bool onPlatform = false;
    public bool OnPlatform
    {//to check if player is on platform or not
        set
        {
            onPlatform = value;
        }
    }

    //for players jumping states
    private bool onGround = true;
    public bool OnGround
    {
        set
        {
            onGround = value;
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        //jump to certain height based on kinematic equation from constant acceleration
        jumpForce = new Vector3(0f, Mathf.Sqrt(jumpMaxHeight * -2f * Physics.gravity.y), 0f);
    }

    void Update()
    {//reads player input
        if (onGround || (Input.GetButton(ConstantStrings.Horizontal) == true ||
            Input.GetButton(ConstantStrings.Vertical) == true))
        {//if condition accounts for player when off ground and input not held 
            horizontal = Input.GetAxis(ConstantStrings.Horizontal);
            vertical = Input.GetAxis(ConstantStrings.Vertical);
        }

        jump = jump || Input.GetButtonDown(ConstantStrings.Jump);

        Teleport(); //checks if can be teleported
        FallOffStage(); //checks if player fell off stage
    }
    void FixedUpdate()
    {//fixed update used for physics based movement
        MovePlayer();
        PlayerJump();
        TerminalVelocity(); //max falling velocity
    }

    private void MovePlayer()
    {
        playerMovement = new Vector3(horizontal, 0f, vertical);

        if (horizontal != 0 || vertical != 0)//helps maintain player facing position when inputs == 0
        {//player movement using WASD as inputs while input != 0 
            if (!onPlatform)
            {
                playerRigidbody.MovePosition(transform.position + playerSpeed * Time.fixedDeltaTime * playerMovement);
                playerRigidbody.rotation = Quaternion.LookRotation(playerMovement * playerTurnSpeed, Vector3.up);
            }
            else
            {//to fix camera jitter and inability to move the player when on platform + FixedUpdate change on MovingPlatforms
                playerRigidbody.AddForce(playerMovement * playerSpeed, ForceMode.Impulse);
            }
            //rotate player to look at direction
            playerRigidbody.rotation = Quaternion.LookRotation(playerMovement * playerTurnSpeed, Vector3.up);
        }
    }

    private void PlayerJump()
    {
        if (jump && onGround && !onTeleporter)
        {//jumps only while player is on ground and not on teleporter
            playerRigidbody.AddForce(jumpForce, ForceMode.Impulse);
            jump = false;
            onGround = false;
        }
    }

    private void TerminalVelocity()
    {//clamps player max falling speed to terminalVelocity
        playerRigidbody.velocity = Vector3.up * Mathf.Max(playerRigidbody.velocity.y, -terminalVelocity);
    }

    private void Teleport()
    {//when on platform and jumped (pressed space bar), teleport
        if (onTeleporter && jump)
        {
            GameManager.Instance.NextLevel();
            jump = false;
        }
    }

    private void FallOffStage()
    {//if player fell off stage, dies
        if(transform.position.y <= fallDeathPosition)
        {
            GameManager.Instance.Die();
        }
    }
}
