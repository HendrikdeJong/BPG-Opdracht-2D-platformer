using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Vector2 startpos;
    
    private Vector2 move;
    [SerializeField] private bool isGrounded;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        startpos = transform.position;
    }
    void Update() {
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

        if(move.x < 0) {
            spriteRenderer.flipX = true;
        }else
            spriteRenderer.flipX = false;
            
        
        //sprint
        if (Input.GetButton("Fire3")) {
            speed = 7f;
        }else
            speed = 5f;
        
        //timer
        invisTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other) {
        switch (other.name) {
            case "TopCollider":
                other.transform.parent.GetComponent<Enemy>().Die();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                break;

            case "SideCollider":
                PlayerHit();
                break;

            case "Door":
                if (HasKey) {
                    Debug.Log("Key used on door");
                    SceneLoader.Loader.LoadNextScene(GameManager.manager.totalcoins);
                }
                break;

            case "Death":
                // SceneLoader.Loader.ReloadScene();
                transform.position = startpos;
                PlayerHit();

                break;

            case "Items":
                Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
                TileBase tile = tilemap.GetTile(cellPosition);

                if (tile != null) {
                    switch (tile.name)
                    {
                        case "Coin":
                            GameManager.manager.AddCoin();
                            tilemap.SetTile(cellPosition, null);
                            break;

                        case "Heart":
                            if (health < 3)
                            {
                                health++;
                                GameManager.manager.UpdateHealthUI(health);
                                tilemap.SetTile(cellPosition, null);
                            }
                            break;

                        case "JumpPad":
                            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 2);
                            break;

                        case "Key":
                            HasKey = true;
                            tilemap.SetTile(cellPosition, null);
                            break;
                        case "LifeStation":
                            Debug.Log("life bitch");
                            GameManager.manager.AddLife();
                            tilemap.SetTile(cellPosition, null);
                            break;
                    }
                }
                break;
        }
    }


    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);

        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
    }

    private void PlayerHit() {
        if(health < 1 && invisTimer < 0) {
                Debug.Log("player is dead");
                GameManager.manager.RemoveLife();
                PlayerPrefs.SetInt("lifes", GameManager.manager.totallifes);
                SceneLoader.Loader.LoadScene(0);
                // SceneLoader.Loader.ReloadScene();
            }else if(invisTimer < 0) {
                health --;
                GameManager.manager.UpdateHealthUI(health);
                invisTimer = resetinvistimer;
            }
    }

}
