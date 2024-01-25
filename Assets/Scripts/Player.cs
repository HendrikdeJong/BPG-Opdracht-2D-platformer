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
    private float resetinvistimer = 1.5f;
    private float invisTimer;
    private bool HasKey = false;
    private Vector2 startpos;
    private Vector2 move;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int health = 3;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private bool isGrounded;

    private void Start() {
        startpos = transform.position;
    }
    void Update() {
        float sideMovement = Input.GetAxis("Horizontal");
        move = new Vector2(sideMovement, rb.velocity.y);
        animator.SetFloat("MoveSpeed", Mathf.Abs(sideMovement));

        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            source.PlayOneShot(sfx[5]);
        }
        if (Input.GetButtonDown("Cancel")) {
            GameManager.manager.TogglePause();
        }

        //animator
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Velocity", Mathf.Abs(rb.velocity.x / 5));

        spriteRenderer.flipX = move.x < 0 ? true : false;
        speed = Input.GetButton("Fire3") ? 7f : 5f;
        
        invisTimer -= Time.deltaTime;
    }
    
    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(transform.position, 1f, groundLayer);

        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
    }
    
    //collision & items
    private void OnTriggerStay2D(Collider2D other) {
        switch (other.name) {
            case "TopCollider":
                other.transform.parent.GetComponent<Enemy>().Die();
                source.PlayOneShot(sfx[4]);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                break;

            case "SideCollider":
                PlayerHit();
                break;
            case "Death":
                // SceneLoader.Loader.ReloadScene();
                transform.position = startpos;
                PlayerHit();

                break;

            case "Door":
                if (HasKey) {
                    Debug.Log("Key used on door");
                    SceneLoader.Loader.LoadNextScene(GameManager.manager.totalcoins);
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
                            source.PlayOneShot(sfx[0]);
                            tilemap.SetTile(cellPosition, null);
                            break;

                        case "Heart":
                            if (health < 3) {
                                health++;
                                source.PlayOneShot(sfx[1]);
                                GameManager.manager.UpdateHealthUI(health);
                                tilemap.SetTile(cellPosition, null);
                            }
                            break;

                        case "JumpPad":
                            source.PlayOneShot(sfx[6]);
                            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 2);
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
                    }
                }
                break;
        }
    }

    private void PlayerHit() {
        if(health < 1 && invisTimer < 0) {
                invisTimer = resetinvistimer;
                StartCoroutine(PlayerDeath());
            }else if(invisTimer < 0) {
                source.PlayOneShot(sfx[3]);
                health --;
                GameManager.manager.UpdateHealthUI(health);
                invisTimer = resetinvistimer;
            }
    }

    private IEnumerator PlayerDeath(){
        source.PlayOneShot(sfx[7]);
        GameManager.manager.RemoveLife();
        PlayerPrefs.SetInt("lifes", GameManager.manager.totallifes);
        yield return new WaitForSeconds(2);
        
    }

}
