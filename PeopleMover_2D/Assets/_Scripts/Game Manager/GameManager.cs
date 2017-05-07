using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The game manager class for this
/// 
/// Author: Ben Hoffman
/// </summary>
public class GameManager : MonoBehaviour {

    // public static instance of this object for others to reach it
    public static GameManager Instance;

    private GameStates _currentState;

    public GameStates CurrentState { get { return _currentState; } }

    private void Awake()
    {
        // Set the instance of the game manager
        Instance = this;
    }

    /// <summary>
    /// Set the begging state of the game
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    void Start ()
    {
        // Set the current state to the main menu panel
        _currentState = GameStates.Menu;

        // Don't Destroy this object on load
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Check for if the player want's to pause the game
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    private void Update()
    {
        // If we want to press the pause button
        if (Input.GetButtonDown("Cancel"))
        {
            // Toggle if the menu scene is loaded or not
        }
    }

    /// <summary>
    /// To be attatched to the start button. Start the game
    /// and hide some of the UI
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void StartGame()
    {
        // Set the current game starte
        _currentState = GameStates.Playing;

        SceneManager.LoadScene("Bens_Scene", LoadSceneMode.Single);
        
    }

    /// <summary>
    /// Handles the game ending. Shows game over UI
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void GameOver()
    {
        // Change the current state of the game
        _currentState = GameStates.GameOver;

        // Load my game scene
        Debug.Log("Game Over!");

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

    }

    #region Utilities

    /// <summary>
    /// Provide a way for a button to quit the game from a 
    /// menu
    /// 
    /// Author: Ben Hoffman
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
