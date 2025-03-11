using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for global access
    public GameObject deathScreenUI;  // Assign in Inspector
    public GameObject resistButton;   // Assign UI button
    public GameObject giveUpButton;   // Assign UI button

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerDied()
    {
        // Activate death screen
        deathScreenUI.SetActive(true);

        // Pause the game while waiting for the player's decision
        Time.timeScale = 0;
        
    }

    

    public void Resist()
    {
        // Load the survival challenge scene
        Time.timeScale = 1; // Resume game
        SceneManager.LoadScene("ResistChallengeScene"); // Change this to your actual scene name
    }
}