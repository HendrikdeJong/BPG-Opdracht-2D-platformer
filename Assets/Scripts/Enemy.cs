using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private LayerMask groundLayer;
    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();

    private Vector2 lastVelocity;

    void Update()
    {
        if (!GameManager.manager.paused) {
            HandleMovement();
        }
        else {
            FreezeEnemy();
        }
    }

    private void HandleMovement() {
        if (rb.isKinematic){
            rb.isKinematic = false;
            rb.velocity = lastVelocity;
        }

        Vector2 moveDirection = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, moveDirection, 1f, groundLayer);
        Debug.DrawRay(transform.position, moveDirection);

        if (hit2D.collider != null) {
            flip();
        }

        rb.velocity = moveDirection * speed;
        lastVelocity = rb.velocity;
    }

    private void FreezeEnemy() {
        if (!rb.isKinematic)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }

    void flip() {
        movingRight = !movingRight;
        spriteRenderer.flipX = movingRight;
    }

    public void Die() {
        Destroy(gameObject);
    }
}
