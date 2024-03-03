using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float sprintDuration = 7f;
    public float sprintCooldown = 5f;
    public float jumpHeight = 4f;
    public float jumpCooldown = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private float sprintCooldownTimer = 0f;
    private float jumpTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor to center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        if (isSprinting && sprintTimer > 0f)
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
            sprintTimer -= Time.deltaTime;
        }
        else
        {
            controller.Move(move * speed * Time.deltaTime);

            if (sprintCooldownTimer > 0f)
            {
                sprintCooldownTimer -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    isSprinting = true;
                    sprintTimer = sprintDuration;
                    sprintCooldownTimer = sprintCooldown;
                }
            }
        }

        // Player jump
        if (Input.GetButtonDown("Jump") && isGrounded && jumpTimer <= 0f)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpTimer = jumpCooldown;
        }

        // Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
        }
    }
}

