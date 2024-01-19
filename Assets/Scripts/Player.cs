using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int health = 1;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    
    private Vector2 move;
    [SerializeField] private bool isGrounded;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        float sideMovement = Input.GetAxis("Horizontal");
        move = new Vector2(sideMovement, rb.velocity.y);

        // Check for jump input
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButton("Fire3")){
            speed = 7f;
        }else{
            speed = 5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("EnemyTop")) {
            other.transform.parent.GetComponent<Enemy>().Die();
        }
        if(other.CompareTag("EnemySide")){
            PlayerHit();
        }
    }

    private void FixedUpdate() {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);

        // Apply movement without Time.deltaTime
        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
    }

    private void PlayerHit(){
        if(health < 1){
            Debug.Log("player is dead!");
        }else
            health --;
            Debug.Log(health);
    }

}
