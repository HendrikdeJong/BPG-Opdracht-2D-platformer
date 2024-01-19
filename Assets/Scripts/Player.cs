using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int health = 3;
    
        
    private float resetinvistimer = 3f;
    private float invisTimer;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Vector2 move;
    [SerializeField] private bool isGrounded;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        float sideMovement = Input.GetAxis("Horizontal");
        move = new Vector2(sideMovement, rb.velocity.y);
        animator.SetFloat("MoveSpeed", math.abs(sideMovement));

        // Check for jump input
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //animator
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Velocity", math.abs(rb.velocity.x / 5));

        if(move.x < 0){
            spriteRenderer.flipX = true;
        }else
            spriteRenderer.flipX = false;
            
        
        //sprint
        if (Input.GetButton("Fire3")){
            speed = 7f;
        }else
            speed = 5f;
        
        //timer
        invisTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("EnemyTop")) {
            other.transform.parent.GetComponent<Enemy>().Die();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(other.CompareTag("EnemySide")){
            PlayerHit();
        }
        if(other.CompareTag("Coin")){
            GameManager.manager.AddCoin();
            Destroy(other.gameObject);
        }
    }

    private void FixedUpdate() {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);

        // Apply movement without Time.deltaTime
        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
    }

    private void PlayerHit(){
        if(invisTimer < 0){
            invisTimer = resetinvistimer;
        }

        if(health < 1){
                Debug.Log("player is dead");
            }else
                health --;
                GameManager.manager.UpdateHealthUI(health);
    }

}
