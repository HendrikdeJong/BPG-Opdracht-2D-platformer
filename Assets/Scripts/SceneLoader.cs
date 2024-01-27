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
            ResetProggress();
        }else{
            // PlayerPrefs.SetInt("lifes", GameManager.manager.totallives);
            // GameManager.manager.totallives = PlayerPrefs.GetInt("lives");
            SceneManager.LoadScene(scene);
        }
    }

    public void ResetProggress() {
        PlayerPrefs.SetInt("lives", 3);
        PlayerPrefs.SetInt("coins", 0);
        PlayerPrefs.SetInt("proggres",0);
        SceneManager.LoadScene(0);
    }
    // public void ReloadScene(){
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }
    public void LoadNextScene() {
        PlayerPrefs.SetInt("coins", GameManager.manager.totalcoins);
        PlayerPrefs.SetInt("lives", GameManager.manager.totallives);
        if(SceneManager.sceneCount >= SceneManager.GetActiveScene().buildIndex){
            PlayerPrefs.SetInt("proggress", SceneManager.GetActiveScene().buildIndex - 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else SceneManager.LoadScene(0);
    }
}
