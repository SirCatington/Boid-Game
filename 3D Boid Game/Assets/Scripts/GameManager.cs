using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject planets;
    public Flock flock;
    public GameObject target;

    public Button playButton;
    public Text playButtonText;
    public Text verdictText;
    public Text boidNumberText;
    public Text GameNameText;

    public enum GameState   
    {
        Start,             
        Playing,            
        GameOver             
    };

    private GameState gameState;  
    public GameState State { get { return gameState; } }

    private void Awake()
    {
        gameState = GameState.Start;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameNameText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        playButtonText.text = "Play";
        planets.SetActive(false);
        flock.gameObject.SetActive(false);
        target.SetActive(false);
        verdictText.gameObject.SetActive(false);
        boidNumberText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Playing:
                if (flock.BoidNumber() == 0)
                {
                    gameState = GameState.GameOver;
                    GameNameText.gameObject.SetActive(true);
                    playButton.gameObject.SetActive(true);
                    planets.SetActive(false);
                    flock.gameObject.SetActive(false);
                    target.SetActive(false);
                    verdictText.gameObject.SetActive(true);

                    verdictText.text = "Amazing! You Won!";
                    playButtonText.text = "Play Again";
                }
                else if (!flock.playerAlive())
                {
                    gameState = GameState.GameOver;
                    GameNameText.gameObject.SetActive(true);
                    playButton.gameObject.SetActive(true);
                    planets.SetActive(false);
                    flock.gameObject.SetActive(false);
                    target.SetActive(false);
                    verdictText.gameObject.SetActive(true);

                    verdictText.text = "Amazing! You Lost!";
                    playButtonText.text = "Play Again";
                }
                boidNumberText.text = string.Format("Boids: {0:D2}/{1:D2}", flock.BoidNumber(), flock.startingCount + 3);

                break;
        }
    }

    public void OnPlay()
    {
        GameNameText.gameObject.SetActive(false);
        planets.SetActive(true);
        flock.gameObject.SetActive(true);
        target.SetActive(true);
        playButton.gameObject.SetActive(false);
        boidNumberText.gameObject.SetActive(true);
        verdictText.gameObject.SetActive(false);
        gameState = GameState.Playing;
    }

}