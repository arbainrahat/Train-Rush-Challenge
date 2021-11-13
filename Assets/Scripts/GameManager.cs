using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { wait,play,end};
    public GameState currentGameState;

    //Reference
    public static GameManager instance;
    public Transform[] playerSpwanPoints;
    public Transform[] camSpwanPoints;
    public Transform[] levelsSpwanPoints;
    public GameObject[] levels;
    public Transform player;
    public Transform _camera;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject gamePlayPanel;
    public Text scoreText;
    public Text winScoreText;

   

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this) Destroy(this.gameObject);

        //Instiate Level 1
        for(int i=0; i<levels.Length; i++)
        {
            Instantiate(levels[i], levelsSpwanPoints[i].position, levelsSpwanPoints[i].rotation);
        }
        //Instiate Player
        Instantiate(player.gameObject, playerSpwanPoints[0].position, playerSpwanPoints[0].rotation);
        //Set Camera Position and Rotation
        _camera.SetPositionAndRotation(camSpwanPoints[0].position, camSpwanPoints[0].rotation);
    }
    
   
    public void startPlay()
    {
        currentGameState = GameState.play;
    }public void endPlay()
    {
        currentGameState = GameState.end;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelComplete()
    {
        winPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void GameLose()
    {
        losePanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }
}
