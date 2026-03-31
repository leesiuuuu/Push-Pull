using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    LevelLoader levelLoader;

    private BGMScript bs;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        bs = FindObjectOfType<BGMScript>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<NewPlayer1>(out var player1))
        {
            player1.Die();
            bs.FadeOut();
            levelLoader.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (collision.gameObject.TryGetComponent<NewPlayer2>(out var player2))
        {
            player2.Die();
            bs.FadeOut();
            levelLoader.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
