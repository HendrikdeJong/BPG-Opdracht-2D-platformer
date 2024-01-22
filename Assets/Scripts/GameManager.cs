using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    public int totalcoins;
    public bool paused;
    public GameObject pausemenuUi;


    public TextMeshProUGUI GameTimerText;
    public float GameTimer = 120;
    public Image timerImage;

    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;


    public TextMeshProUGUI coinText;
    

    public static GameManager manager;

    private void Awake() {
        manager = this;
    }

    private void Update() {
        coinText.text = totalcoins.ToString(); 

        HandleGameTime();
    }

    private void HandleGameTime(){
        GameTimer -= Time.deltaTime;
        if(GameTimer < 0){
            Debug.Log("game over by time");
        }
        GameTimerText.text = math.round(GameTimer).ToString();
        timerImage.fillAmount = GameTimer / 120;
    }

    public void AddCoin(){
        totalcoins++;
    }

    public void TogglePause(){
        if(!paused){
            Time.timeScale = 0;
            pausemenuUi.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            paused = !paused;
        } else {
            Time.timeScale = 1;
            pausemenuUi.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            paused = !paused;
        }
    }

    public void UpdateHealthUI(int currentHealth){
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth){
                // Display full heart for remaining health
                heartImages[i].sprite = fullHeartSprite;
            }else{
                // Display empty heart for lost health
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }    
}
