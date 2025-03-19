using UnityEngine;
using UnityEngine.SceneManagement;

public class NextFloor : MonoBehaviour
{
    public string sceneToLoad; // Scene to reload
    private bool playerInRange = false;
    public FloorManager floorManager; // Assign this in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered trigger (2D).");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left trigger (2D).");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed. Advancing floor and reloading scene.");
            EnemyDetection.ResetHiveMind();
            if (floorManager != null)
            {
                floorManager.EnterNextFloor();
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}