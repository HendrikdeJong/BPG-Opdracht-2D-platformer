using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {
    // private int lifes;
    private int progress;

    public Button[] buttons;
    public Button saveAndResetBtn;
    public TextMeshProUGUI ScoreText1;
    public TextMeshProUGUI ScoreText2;
    public TextMeshProUGUI leaderboardText;

    public static MainMenuHandler handler;

    public TMP_InputField nameInputField;
    private string leaderboardKey = "Leaderboard";

    string saveFile => Application.persistentDataPath + "/scoreboard.data";

    public Board board = new();


    private void Start() {
        handler = this;
        ScoreText1.text = "Score: " + PlayerPrefs.GetInt("score");
        ScoreText2.text = "Score: " + PlayerPrefs.GetInt("score");
        Cursor.lockState = CursorLockMode.None;
        // lifes = PlayerPrefs.GetInt("lives");
        progress = PlayerPrefs.GetInt("proggress");
        buttons[0].interactable = true;
        for (int i = 0; i < buttons.Length; i++) {
            if (i <= progress) {
                buttons[i].interactable = true;
            } else {
                buttons[i].interactable = false;
            }
        }

        LoadScore();
    }

    public void checkIfResetable(){
        if(nameInputField.text != "" && PlayerPrefs.GetInt("score") > 0) {
            saveAndResetBtn.interactable = true;
        } else{
            saveAndResetBtn.interactable = false;
        }
    }

    public void NewRecord() {
        if(nameInputField.text != "" && PlayerPrefs.GetInt("score") > 0) {
            SaveScore(PlayerPrefs.GetInt("score"));
        }
    }

    public void SaveScore(int score){
        if(score <= 0)
            return;
        if(nameInputField.text == "")
            return;

        string playerName = nameInputField.text;

        Record newRecord = new Record(playerName, score);
        board.records.Add(newRecord);

        File.WriteAllText(saveFile, JsonUtility.ToJson(board));

        print(Application.persistentDataPath);

        DisplayLeaderboard();
    }

    public void LoadScore() {
        if(!File.Exists(saveFile))
            return;

        string loadedBoard = File.ReadAllText(saveFile);

        board = JsonUtility.FromJson<Board>(loadedBoard);

        DisplayLeaderboard();
    }

    void DisplayLeaderboard() {
        string finalText = "";
        foreach(Record record in board.records) {
            finalText += record.playerName + ": " + record.score + "\n";
        }
        leaderboardText.text = finalText;
    }

    public void Quit(){
        Application.Quit();
    }
}

[System.Serializable]
public class Board {
    public List<Record> records = new();
}

[System.Serializable]
public class Record {
    public string playerName = "No Value";
    public int score = 0;

    public Record(string playerName, int score) {
        this.playerName = playerName;
        this.score = score;
    }
}