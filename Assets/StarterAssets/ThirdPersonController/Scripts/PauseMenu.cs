using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // The UI Panel for your pause menu
    private bool isPaused = false;

    void Update()
    {
        // Check if the player pressed Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f;          // Resume game time
        isPaused = false;
        
        // Relock the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Show the menu
        Time.timeScale = 0f;          // Freeze game time
        isPaused = true;

        // Unlock the cursor so you can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}