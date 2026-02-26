using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    bool enteredPlayer1;
    bool enteredPlayer2;

    public bool isCleared;

    int keyCount;
    [SerializeField] GameObject clearUI;
    LevelLoader levelLoader;
    KeyCounter keyCounter;

    NewPlayer1 player1;
    NewPlayer2 player2;
    [SerializeField]
    AudioClip clip1;
    [SerializeField]
    AudioClip clip2;
    private void Start()
    {
        keyCounter = FindObjectOfType<KeyCounter>();
        levelLoader = FindObjectOfType<LevelLoader>();
        Key[] currentKeys = FindObjectsOfType<Key>();
        keyCount = currentKeys.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<NewPlayer1>(out var player1))
        {
            enteredPlayer1 = true;
            this.player1 = player1;
        }
        if(collision.TryGetComponent<NewPlayer2>(out var player2))
        {
            enteredPlayer2 = true;
            this.player2 = player2;
        }

        if (enteredPlayer1 && enteredPlayer2)
            StartCoroutine(NextStage());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<NewPlayer1>() != null)
        {
            enteredPlayer1 = false;
        }
        if (collision.GetComponent<NewPlayer2>() != null)
        {
            enteredPlayer2 = false;
        }
    }

    IEnumerator NextStage()
    {
        if (isCleared)
            yield break;

        if(keyCounter.KeyCount == keyCount)
        {
            isCleared = true;
            player1.Cleared();
            player2.Cleared();
            SoundManager.Instance.SFXPlay("Clear", clip1);
            yield return new WaitForSeconds(1.5f);
            SoundManager.Instance.SFXPlay("Clear", clip2);
            clearUI.SetActive(true);
        }
    }
}
