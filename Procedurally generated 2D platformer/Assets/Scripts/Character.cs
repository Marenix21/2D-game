using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour
{
    private bool grounded;

    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 5;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 30;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 5;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Deceleration applied when character is in the air and.")]
    float airDeceleration = 5;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity.")]
    float jumpHeight = 2;

    [SerializeField, Tooltip("Define gravity.")]
    float gravity = 2;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    public int score = 0;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (grounded)
        {
            velocity.y = 0;
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
<<<<<<< HEAD
        } 
        velocity.y += Physics2D.gravity.y * Time.deltaTime * gravity;
=======
        }
        velocity.y += Physics2D.gravity.y * Time.deltaTime * 1.5f;
>>>>>>> f2f2901 (added start menu and player death)

        float moveInput = Input.GetAxisRaw("Horizontal");
        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : airDeceleration;
        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
            score += 1;
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }
        transform.Translate(velocity * Time.deltaTime);

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        grounded = false;
        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider)
                continue;

            if(hit.tag == "Finish") {
                this.endGame();
            }

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                }
            }
        }
    }

    private void endGame() {
        Debug.Log("Dead.");
        SaveSystem.SaveScore(this);
        SceneManager.LoadScene(0);
    }
}

