using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    private int lifes;
    private int proggress;

    public Button[] buttons;

    private void Awake() {
        lifes = PlayerPrefs.GetInt("lifes");
        proggress = PlayerPrefs.GetInt("proggress");

        for (int i = 0; i < buttons.Length; i++) {
            if (i < proggress) {
                buttons[i].interactable = true;
            } else {
                buttons[i].interactable = false;
            }
        }

        if (lifes <= 0) {
            PlayerPrefs.SetInt("lifes", 3);
            PlayerPrefs.SetInt("proggress", 0);

            for (int i = 0; i < buttons.Length; i++) {
                buttons[i].interactable = false;
            }
        }
    }
    public void quit(){
        Application.Quit();
    }
}
