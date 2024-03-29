using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour{
    public int totalcoins;
    public int totallives;
    public int proggress;
    public int totalscore;
    public int levelScore;
    public bool paused;
    public int health = 3;
    public float GameTimer = 120;

    private float resetinvistimer = 1.5f;
    private float invisTimer;

    public GameObject pausemenuUi;
    public GameObject deathScreen;
    public AudioSource MusicSource;
    public CinemachineVirtualCamera cinemachineCamera;

    AudioSource source => GetComponent<AudioSource>();

    
    public AudioClip[] sfx;
    public Image[] heartImages;

    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI GameTimerText;
    public TextMeshProUGUI GameoverText;
    public TextMeshProUGUI Score;

    

    public static GameManager manager;

    private void Awake() {
        manager = this;
        totalscore = PlayerPrefs.GetInt("score");
        totalcoins = PlayerPrefs.GetInt("coins");
        totallives = PlayerPrefs.GetInt("lives");
        proggress = PlayerPrefs.GetInt("proggress");
        Score.text = "Score: " + levelScore.ToString();
        lifeText.text = totallives.ToString(); 
    }

    private void Update() {
        Score.text = "Score: " + levelScore.ToString();
        lifeText.text = totallives.ToString();
        PlayerPrefs.SetInt("lives", totallives);

        if(paused){
            MusicSource.volume = .05f;
            
        }else{
            MusicSource.volume = .5f;
        }
        HandleGameTime();
        pausemenuUi.SetActive(paused);
        if(!paused){
            GameTimer -= Time.deltaTime;
            invisTimer -= Time.deltaTime;
        }
    }

    public void saveprefs(){
        PlayerPrefs.SetInt("score",totalscore + levelScore);
        PlayerPrefs.SetInt("coins",totalcoins);
    }

    private void HandleGameTime(){
        if(GameTimer == 45)
            source.PlayOneShot(sfx[2]);

        if(GameTimer < 0){
            source.PlayOneShot(sfx[1]);
            RemoveLife();
            
            GameoverText.text = "ran out of time!";
            deathScreen.SetActive(true);
            paused = true;
        }
        GameTimerText.text = Mathf.Floor(GameTimer).ToString();
    }

    public void AddLife(){
        totallives++;
    }
    public void RemoveLife(){
        totallives--;
    }
    public void AddCoin(){
        totalcoins++;
    }

    public void UpdateHealthUI(){
        for (int i = 0; i < heartImages.Length; i++) {
            if (i < health){
                // Display full heart for remaining health
                heartImages[i].sprite = fullHeartSprite;
            }else{
                // Display empty heart for lost health
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

     public void PlayerHit() {
        if(health < 1 && invisTimer < 0) {
                invisTimer = resetinvistimer;
                StartCoroutine(PlayerDeath());
            }else if(invisTimer < 0) {
                source.PlayOneShot(sfx[0]);
                health --;
                UpdateHealthUI();
                invisTimer = resetinvistimer;
            }
    }
    public void changeFOV(){
        cinemachineCamera.m_Lens.OrthographicSize = 7;
    }
    

    public IEnumerator PlayerDeath(){
        source.PlayOneShot(sfx[1]);
        RemoveLife();
        GameoverText.text = "Player died";
        yield return new WaitForSeconds(1);
        deathScreen.SetActive(true);
        paused = true;
        // SceneLoader.Loader.LoadScene(0);
    }

    public void ToggleCursorState() {
        paused = !paused;
        // Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = !paused;
        // Time.timeScale = paused ? Time.timeScale = 1 : Time.timeScale = 0;
        // GameManager.manager.paused = paused ? true : false;
    }
}
