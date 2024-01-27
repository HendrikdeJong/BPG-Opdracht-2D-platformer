using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public int totalcoins;
    public int totallives;
    public int proggress;
    public bool paused;
    public GameObject pausemenuUi;


    public TextMeshProUGUI GameTimerText;
    public float GameTimer = 120;
    public Image timerImage;

    public Image[] heartImages;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;


    public TextMeshProUGUI coinText;
    public TextMeshProUGUI lifeText;
    

    public static GameManager manager;

    private void Awake() {
        manager = this;

        totalcoins = PlayerPrefs.GetInt("coins");
        totallives = PlayerPrefs.GetInt("lives");
        proggress = PlayerPrefs.GetInt("proggress");
        coinText.text = totalcoins.ToString();
        lifeText.text = totallives.ToString(); 
    }

    private void Update() {
        coinText.text = totalcoins.ToString();
        lifeText.text = totallives.ToString(); 

        HandleGameTime();
        pausemenuUi.SetActive(paused); 
        if(!paused)
            GameTimer -= Time.deltaTime;
    }

    private void HandleGameTime(){
        
        if(GameTimer < 0){
            Debug.Log("game over by time");
            RemoveLife();
            PlayerPrefs.SetInt("lifes",totallives);
            SceneLoader.Loader.LoadScene(0);
        }
        GameTimerText.text = math.round(GameTimer).ToString();
        timerImage.fillAmount = GameTimer / 120;
    }

    public void AddLife(){totallives++;}
    public void RemoveLife(){totallives--;}
    public void AddCoin(){totalcoins++;}

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

    public void ToggleCursorState() {
        paused = !paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        // Time.timeScale = paused ? Time.timeScale = 1 : Time.timeScale = 0;
        // GameManager.manager.paused = paused ? true : false;
        // Cursor.visible = !paused;
    }
}
