using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Loader;
    public int totalCoins;
    public int totallifes;

    private void Awake() {
        Loader = this;
    }

    public void LoadScene(int scene){
        PlayerPrefs.SetInt("lifes", totallifes);
        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene().buildIndex == 0)
            Cursor.lockState = CursorLockMode.Locked;
    }
    // public void ReloadScene(){
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }
    public void LoadNextScene(int totalCoins){
        // TotalCoins = totalCoins;
        PlayerPrefs.SetInt("coins", totalCoins);
        PlayerPrefs.SetInt("lifes", totallifes);
        if(SceneManager.sceneCount >= SceneManager.GetActiveScene().buildIndex)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadScene(0);
    }
}
