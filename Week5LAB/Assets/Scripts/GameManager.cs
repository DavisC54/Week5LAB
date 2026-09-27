using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns the player and handles game over and restart
/// <summary>

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public bool gameOver = false;

    private void Awake()
    {
        // doing this in awake so the ship is there before the camera looks for it
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        Meteor.onPlayerHit += GameOver;
        PlayerInputHandler.onRestart += Restart;
    }

    private void OnDisable()
    {
        Meteor.onPlayerHit -= GameOver;
        PlayerInputHandler.onRestart -= Restart;
    }

    private void GameOver()
    {
        gameOver = true;
    }

    private void Restart()
    {
        if (!gameOver)
        {
            return;
        }

        SceneManager.LoadScene("Week5Lab");
    }
}