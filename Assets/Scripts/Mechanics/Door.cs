using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool enteredPlayers;

    public bool isCleared;

    int keyCount;
    int lastKeyCount;

    [SerializeField] GameObject clearUI;
    LevelLoader levelLoader;
    KeyCounter keyCounter;

    List<GameObject> Players = new List<GameObject>();

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
        lastKeyCount = keyCounter.KeyCount;
    }

    private void Update()
    {
        if (lastKeyCount != keyCounter.KeyCount)
        {
            lastKeyCount = keyCounter.KeyCount;
            TryOpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<InputPlayer>(out var Player))
        {
            if (!Players.Contains(Player.gameObject))
            {
                Players.Add(Player.gameObject);
            }

            TryOpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<InputPlayer>(out var Player))
        {
            Players.Remove(Player.gameObject);
            enteredPlayers = Players.Count >= 2;
        }
    }

    private void TryOpenDoor()
    {
        enteredPlayers = Players.Count >= 2;

        if (enteredPlayers && !isCleared && keyCounter.KeyCount == keyCount)
        {
            StartCoroutine(NextStage());
        }
    }

    IEnumerator NextStage()
    {
        if (isCleared)
            yield break;

        if (keyCounter.KeyCount == keyCount)
        {
            isCleared = true;

            foreach (GameObject player in Players)
            {
                if (player.TryGetComponent<InputPlayer>(out var inputPlayer))
                {
                    inputPlayer.Cleared();
                }
            }

            SoundManager.Instance.SFXPlay("Clear", clip1);
            yield return new WaitForSeconds(1.5f);
            SoundManager.Instance.SFXPlay("Clear", clip2);
            clearUI.SetActive(true);
        }
    }
}
