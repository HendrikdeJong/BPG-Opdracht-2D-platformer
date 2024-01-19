using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour{

    public int totalcoins;

    public TextMeshProUGUI coinText;

    public static CoinManager manager;

    private void Awake() {
        manager = this;
    }

    public void AddCoin(){
        totalcoins++;
    }

    private void Update() {
        coinText.text = "Total: " + totalcoins.ToString(); 
    }
}
