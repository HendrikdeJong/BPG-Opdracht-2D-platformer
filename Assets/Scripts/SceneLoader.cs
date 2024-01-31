using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public static SceneLoader Loader;
    

    private void Awake() {
        Loader = this;
    }

    public void LoadScene(int scene) {
        if(PlayerPrefs.GetInt("lives") <= 0){
            ResetProgress();
        }else{
            // PlayerPrefs.SetInt("lifes", GameManager.manager.totallives);
            // GameManager.manager.totallives = PlayerPrefs.GetInt("lives");
            SceneManager.LoadScene(scene);
        }
    }

    public void ResetProgress() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("lives", 3);
        SceneManager.LoadScene(0);
    }

    public void SaveScoreAndReset(){
        MainMenuHandler.handler.SaveScore(PlayerPrefs.GetInt("score"));
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("lives", 3);
        SceneManager.LoadScene(0);
    }

    public void ReloadScene(){
        if(PlayerPrefs.GetInt("lives") <= 0){
            LoadScene(0);
        }else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void goToMainMenu(){
        SceneManager.LoadScene(0);
    }

    public void LoadNextScene() {
        if(SceneManager.sceneCount >= SceneManager.GetActiveScene().buildIndex){
            PlayerPrefs.SetInt("proggress", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else SceneManager.LoadScene(0);
    }
}
