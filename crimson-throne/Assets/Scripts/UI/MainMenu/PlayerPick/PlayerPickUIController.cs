using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;
using System.Collections.Generic;

public class PlayerPickUIController : MonoBehaviour {
    public static PlayerPickUIController instance { get; private set; }
    [SerializeField] private List<GameObject> players;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Pick(int index)
    {
        if (players.Count == 0 || index >= players.Count || index < 0)
        {
            Close();
            return;
        }
        GameManager.instance.player = players[index];
        AudioManager.instance.PlaySFX(AudioManager.instance.startNewGame);
        GameManager.instance?.NextLevel();
    }

    public void Close()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        UIManager.instance.playerPickPanel.SetActive(false);
        UIManager.instance.mainPanel.SetActive(true);
    }
}