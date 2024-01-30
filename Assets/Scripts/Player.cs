using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour{
    AudioSource source => GetComponent<AudioSource>();
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    Animator animator => GetComponent<Animator>();
    SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

    public AudioClip[] sfx;
    private bool HasKey = false;
    private Vector2 startpos;
    private Vector2 move;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private bool isGrounded;

    private void Start() {
        startpos = transform.position;
        GameManager.manager.paused = false;
    }

    void Update() {
        if (!GameManager.manager.paused) {
            HandleMovement();
            HandleJump();
        }
            TogglePauseState();
            HandleActions();
            // Always update animations and sprite direction
            UpdateAnimationsAndSpriteDirection();
        }

    void FixedUpdate() {
        if (!GameManager.manager.paused) {
            ApplyMovement();
        }
    }

    private void HandleMovement() {
        float sideMovement = Input.GetAxis("Horizontal");
        move = new Vector2(sideMovement, rb.velocity.y);
        animator.SetFloat("MoveSpeed", Mathf.Abs(sideMovement));
    }

    private void HandleJump() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            source.PlayOneShot(sfx[5]);
        }
    }

    private void HandleActions() {
        if (Input.GetButtonDown("Cancel") && !GameManager.manager.paused) {
            GameManager.manager.ToggleCursorState();
        }
    }

    private void ApplyMovement() {
        isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);
        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
    }

    private void UpdateAnimationsAndSpriteDirection() {
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Velocity", Mathf.Abs(rb.velocity.x / 5));
        spriteRenderer.flipX = move.x < 0;
        speed = Input.GetButton("Fire3") ? 7f : 5f;
    }

    private void TogglePauseState() {
        if (GameManager.manager.paused) {
            // When paused, make the Rigidbody kinematic
            rb.isKinematic = true;
            rb.velocity = Vector2.zero; // Optionally, clear any existing velocity
        } else {
            // When unpaused, revert the Rigidbody to non-kinematic
            rb.isKinematic = false;
        }
    }

    public IEnumerator bluePotion(){
        spriteRenderer.color = new Color(r: 0.3165378f, g:0.3165378f, b:1, a:1);
        jumpForce = 15;
        yield return new WaitForSeconds(3);
        spriteRenderer.color = new Color(r: 1, g:1, b:1, a:1);
        jumpForce = 10;
        // SceneLoader.Loader.LoadScene(0);
    }

    //collision & items
    private void OnTriggerStay2D(Collider2D other) {
        switch (other.name) {
            case "TopCollider":
                other.transform.parent.GetComponent<Enemy>().Die();
                source.PlayOneShot(sfx[4]);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                GameManager.manager.levelScore += 25;
                break;

            case "SideCollider":
                GameManager.manager.PlayerHit();
                // PlayerHit();
                break;
            case "Death":
                // SceneLoader.Loader.ReloadScene();
                GameManager.manager.PlayerHit();
                transform.position = startpos;
                // PlayerHit();

                break;

            case "Door":
                if (HasKey) {
                    SceneLoader.Loader.LoadNextScene();
                }
                break;
            case "Items":
                Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
                TileBase tile = tilemap.GetTile(cellPosition);

                if (tile != null) {
                    switch (tile.name)
                    {
                        case "Coin":
                            GameManager.manager.AddCoin();
                            GameManager.manager.levelScore += 10;
                            source.PlayOneShot(sfx[0]);
                            tilemap.SetTile(cellPosition, null);
                            break;

                        case "Heart":
                            if (GameManager.manager.health < 3) {
                                GameManager.manager.health++;
                                source.PlayOneShot(sfx[1]);
                                tilemap.SetTile(cellPosition, null);
                            }
                            break;

                        case "JumpPad":
                            source.PlayOneShot(sfx[6]);
                            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f);
                            break;

                        case "Key":
                            HasKey = true;
                            source.PlayOneShot(sfx[2]);
                            tilemap.SetTile(cellPosition, null);
                            break;

                        case "LifeStation":
                            source.PlayOneShot(sfx[1]);
                            GameManager.manager.AddLife();
                            tilemap.SetTile(cellPosition, null);
                            break;

                        case "Potion":
                            source.PlayOneShot(sfx[2]);
                            StartCoroutine(bluePotion());
                            tilemap.SetTile(cellPosition, null);
                            break;

                    }
                }
            break;
        }
    }
}
