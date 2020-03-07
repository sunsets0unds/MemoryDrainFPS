using UnityEngine;
using System;

/*
 * StrafeMovement
 * Created by TheAsuro on Github
 * Modified by Michael Amatucci
 * Last modified: 2/26/2020
 */

public class StrafeMovement : MonoBehaviour
{
    #region Inspector Variables
    [Header("Speed Variables")]
    [SerializeField, Min(1)]
    private float maxGroundSpeed = 6.4f;      // Maximum player speed on the ground
    [SerializeField, Min(0.5f)]
    private float maxAirSpeed = 0.6f;   // "Maximum" player speed in the air
    [SerializeField]
    private float friction = 8f;        // How fast the player decelerates on the ground
    [SerializeField]
    private float jumpForce = 5f;       // How high the player jumps
    [SerializeField, Range(10, 100), Tooltip("The max speed the player can achieve when airstrafing")]
    private float speedLimit = 15f;     // The speed that is the absolute limit the player can reach
    [SerializeField, Tooltip("Choose whether to limit the player's speed")]
    private bool limitSpeed = false;
    [SerializeField, InspectorName("Ground Acceleration")]
    private float accel = 200f;         // How fast the player accelerates on the ground
    [SerializeField, InspectorName("Air Acceleration")]
    private float airAccel = 200f;      // How fast the player accelerates in the air

    [Space(20)]

    [SerializeField, Tooltip("Camera attached to player")]
    private GameObject camObj;
    [SerializeField, Tooltip("A script on a text UI object, diplays the speed of the player. Useful for testing, but not required.")]
    private SpeedDisplay speedDisplayObj;
    #endregion

    private float lastJumpPress = -1f;
    private float jumpPressDuration = 0.1f;
	private bool onGround = false;
    private Rigidbody body;

    private GroundCheck groundChecker;
    private WallCheck wallChecker;

    #region Monobehavior
    private void Awake()
    {
        body = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        groundChecker = GetComponentInChildren<GroundCheck>();
        try
        {
            speedDisplayObj = FindObjectOfType<SpeedDisplay>();
        }
        catch(NullReferenceException e)
        { }
        wallChecker = GetComponentInChildren<WallCheck>();
    }

    private void Update()
    {
        float newSpeed = new Vector3(body.velocity.x, 0f, body.velocity.z).magnitude;
        newSpeed = (float)System.Math.Round(newSpeed, 2);

        if(speedDisplayObj)
            speedDisplayObj.WriteSpeed(newSpeed);

        if (Input.GetButton("Jump"))
		{
			lastJumpPress = Time.time;
		}
	}

	private void FixedUpdate()
	{
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Get player velocity
        Vector3 playerVelocity = body.velocity;
        // Slow down if on ground
        playerVelocity = CalculateFriction(playerVelocity);
        // Add player input
        playerVelocity += CalculateMovement(input, playerVelocity);

        if (playerVelocity.magnitude > speedLimit && limitSpeed)
            playerVelocity = Vector3.ClampMagnitude(playerVelocity, speedLimit);

        //check if next step collides with wall
        playerVelocity = wallChecker.CheckStep(playerVelocity, body);

        // Assign new velocity to player object
        body.velocity = playerVelocity;
	}
    #endregion

    /// <summary>
    /// Slows down the player if on ground
    /// </summary>
    /// <param name="currentVelocity">Velocity of the player</param>
    /// <returns>Modified velocity of the player</returns>
    private Vector3 CalculateFriction(Vector3 currentVelocity)
	{
        onGround = CheckGround();
		float speed = currentVelocity.magnitude;

        if (!onGround || Input.GetButton("Jump") || speed == 0f)
            return currentVelocity;

        float drop = speed * friction * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }
    
    /// <summary>
    /// Moves the player according to the input. (THIS IS WHERE THE STRAFING MECHANIC HAPPENS)
    /// </summary>
    /// <param name="input">Horizontal and vertical axis of the user input</param>
    /// <param name="velocity">Current velocity of the player</param>
    /// <returns>Additional velocity of the player</returns>
	private Vector3 CalculateMovement(Vector2 input, Vector3 velocity)
	{
        onGround = CheckGround();

        //Different acceleration values for ground and air
        float curAccel = accel;
        if (!onGround)
            curAccel = airAccel;

        //Ground speed
        float curMaxSpeed = maxGroundSpeed;

        //Air speed
        if (!onGround)
            curMaxSpeed = maxAirSpeed;
        
        //Get rotation input and make it a vector
        Vector3 camRotation = new Vector3(0f, camObj.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(camRotation) *
                                new Vector3(input.x * curAccel, 0f, input.y * curAccel);

        //Ignore vertical component of rotated input
        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        //Get current velocity
        Vector3 currentVelocity = new Vector3(velocity.x, 0f, velocity.z);

        //How close the current speed to max velocity is (1 = not moving, 0 = at/over max speed)
        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / curMaxSpeed));

        //How perpendicular the input to the current velocity is (0 = 90°)
        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        //Scale the input to the max speed
        Vector3 modifiedVelocity = alignedInputVelocity * max;

        //The more perpendicular the input is, the more the input velocity will be applied
        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        //Apply jump
        correctVelocity += GetJumpVelocity(velocity.y);

        //Return
        return correctVelocity;
    }

    /// <summary>
    /// Calculates the velocity with which the player is accelerated up when jumping
    /// </summary>
    /// <param name="yVelocity">Current "up" velocity of the player (velocity.y)</param>
    /// <returns>Additional jump velocity for the player</returns>
	private Vector3 GetJumpVelocity(float yVelocity)
	{
		Vector3 jumpVelocity = Vector3.zero;

		if(Time.time < lastJumpPress + jumpPressDuration && yVelocity < jumpForce && CheckGround())
		{
			lastJumpPress = -1f;
			jumpVelocity = new Vector3(0f, jumpForce - yVelocity, 0f);
		}

		return jumpVelocity;
	}
	
    /// <summary>
    /// Checks if the player is touching the ground. Uses a small capsule collider located at the players "feet"
    /// </summary>
    /// <returns>True if the player touches the ground, false if not</returns>
	private bool CheckGround()
	{
        return groundChecker.isGround;
    }
}
