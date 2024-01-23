using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Sign : MonoBehaviour
{
    
    public GameObject textBox;

    public TextMeshProUGUI signText;

    public string info;

    public Sprite image;

    public SpriteRenderer imageSprite;

    IEnumerator DisplayText() {
        if(image != null) {
            imageSprite.enabled = true;
            imageSprite.sprite = image;
        } else {
            imageSprite.enabled = false; 
        }

        signText.text = "";
        Char[] chars = info.ToCharArray();
        string finalString = "";
        for(int i = 0; i < chars.Length; i++) {
            finalString += chars[i];
            signText.text = finalString;
            yield return new WaitForSeconds(.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Player>(out Player player)) {
            textBox.SetActive(true);
            signText.enabled = true;
            StartCoroutine(DisplayText());
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.TryGetComponent<Player>(out Player player)) {
            signText.enabled = false;
            imageSprite.enabled = false; 
            textBox.SetActive(false);
        }
    }

}
