using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndState : MonoBehaviour
{
    public static GameEndState instance;
    public TMP_Text WinText;

    public bool GameEnd = false;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        WinText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameEnd && Time.timeScale != 0)
        {
            AudioManager.instance.GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.LoseSound);
            Time.timeScale = 0;
            WinText.gameObject.SetActive(true);
        }
    }

}
