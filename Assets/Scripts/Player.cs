using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int health = 3;
    
        
    private float resetinvistimer = 3f;
    private float invisTimer;

    private bool HasKey = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Tilemap tilemap;
    
    private Vector2 move;
    [SerializeField] private bool isGrounded;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        float sideMovement = Input.GetAxis("Horizontal");
        move = new Vector2(sideMovement, rb.velocity.y);
        animator.SetFloat("MoveSpeed", Mathf.Abs(sideMovement));

        // Check for jump input
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetButtonDown("Cancel")) {
            GameManager.manager.TogglePause();
        }


        //animator
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Velocity", Mathf.Abs(rb.velocity.x / 5));

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

    private void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("EnemyTop")) {
            other.transform.parent.GetComponent<Enemy>().Die();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(other.name == "SideCollider"){
            PlayerHit();
        }
        if(other.name == "Door" && HasKey == true){
            Debug.Log("key used on door");
        }
        if (other.CompareTag("ItemCollider")){
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

            TileBase tile = tilemap.GetTile(cellPosition);
            if (tile != null && tile.name == "Coin"){
                GameManager.manager.AddCoin();
                tilemap.SetTile(cellPosition, null); // dit haalt de tile weg
            }
            if (tile != null && tile.name == "Heart"){
                if(health < 3){
                    health++;
                    GameManager.manager.UpdateHealthUI(health);
                    tilemap.SetTile(cellPosition, null);
                }
            }
            if (tile != null && tile.name == "JumpPad"){
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 2);
            }
            if (tile != null && tile.name == "Key"){
                HasKey = true;
                tilemap.SetTile(cellPosition, null);
            }
            // if (tile != null && tile.name == "Door" && HasKey == true){
            //     Debug.Log("key used on door");
            // }
        }
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);

        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
    }

    private void PlayerHit(){
        if(health < 1 && invisTimer < 0){
                Debug.Log("player is dead");
                // reset scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }else if(invisTimer < 0){
                health --;
                GameManager.manager.UpdateHealthUI(health);
                invisTimer = resetinvistimer;
            }
    }

}
