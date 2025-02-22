﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField, Tooltip("PowerUp icons.")]
    GameObject[] _icons;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    public int score = 0;

    private int wings = 0, hardened = 0;
    private float airAccel = 1, jump = 1;
    private IEnumerator coroutine;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _icons[0].GetComponent<Animator>().SetBool("RepeatWings", false);
        _icons[1].GetComponent<Animator>().SetBool("RepeatHardened", false);
        if (grounded)
        {
            velocity.y = 0;
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y) * jump);
            }
        } 
        velocity.y += Physics2D.gravity.y * Time.deltaTime * gravity;

        float moveInput = Input.GetAxisRaw("Horizontal");
        float acceleration = grounded ? walkAcceleration : airAcceleration * airAccel;
        float deceleration = grounded ? groundDeceleration : airDeceleration;
        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
            score = Mathf.Max(score, (int)Mathf.FloorToInt(GameObject.Find("Character").transform.position[0]));
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

            if(hit.tag == "PowerUp")
            {
                coroutine = powerUp(hit.gameObject);
                StartCoroutine(coroutine);
                DestroyImmediate(hit.gameObject, true);
            }

            if(hit.tag == "Finish") {
                SaveSystem.endGame(this);
            }

            if(hit.tag == "Enemy" && hardened == 0)
            {
                SaveSystem.endGame(this);
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

    private IEnumerator powerUp(GameObject obj)
    {
        string name = obj.GetComponent<SpriteRenderer>().sprite.name;
        if(name == "air")
        {
            wings++;
            airAccel = 5f;
            jump = 1.5f;
            if(wings == 1)
            {
                _icons[0].GetComponent<Animator>().SetBool("Wings", true);
            }
            else
            {
                _icons[0].GetComponent<Animator>().SetBool("RepeatWings", true);
            }
        }
        else if (name == "rock")
        {
            hardened++;
            if (hardened == 1)
            {
                _icons[1].GetComponent<Animator>().SetBool("Hardened", true);
            }
            else
            {
                _icons[1].GetComponent<Animator>().SetBool("RepeatHardened", true);
            }
        }
        yield return new WaitForSeconds(10f);
        if(name == "air")
        {
            wings--;
            if (wings == 0)
            {
                airAccel = 1f;
                jump = 1f;
                _icons[0].GetComponent<Animator>().SetBool("Wings", false);
            }
        }
        else if(name == "rock")
        {
            hardened--;
            if(hardened == 0)
            {
                _icons[1].GetComponent<Animator>().SetBool("Hardened", false);
            }
        }
    }
}

