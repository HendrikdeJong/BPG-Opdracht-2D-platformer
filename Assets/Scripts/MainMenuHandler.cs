using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {
    // private int lifes;
    private int proggress;

    public Button[] buttons;

    private void Awake() {
        Cursor.lockState = CursorLockMode.None;
        // lifes = PlayerPrefs.GetInt("lives");
        proggress = PlayerPrefs.GetInt("proggress");

        for (int i = 0; i < buttons.Length; i++) {
            if (i < proggress) {
                buttons[i].interactable = true;
            } else {
                buttons[i].interactable = false;
            }
        }
    }
    public void Quit(){
        Application.Quit();
    }
}
