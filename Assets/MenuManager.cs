using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Player Control Rig")]
    [Tooltip("Drag the FirstPersonController GameObject here to disable camera/mouse capture while in menus.")]
    public MonoBehaviour playerMovementScript;

    [Header("Panels")]
    public GameObject startMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;
    public GameObject gameOverPanel;

    [Header("HUD")]
    public GameObject healthBar;
    public GameObject crosshair;

    [Header("Audio")]
    public Slider masterVolumeSlider;

    private bool isPlaying = false;
    private bool isPaused = false;
    private GameObject previousMenu;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.value = AudioListener.volume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        // Boot directly to Start Menu state
        ShowStartMenu();
    }

    void Update()
    {
        // Only allow Escape to pause if actively playing and not in game over
        if (isPlaying && (gameOverPanel == null || !gameOverPanel.activeSelf))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    // --- Core State Transitions ---

    public void StartGame()
    {
        HideAllPanels();
        SetHUDActive(true);

        isPlaying = true;
        isPaused = false;
        Time.timeScale = 1f;

        LockCursor();

        // Enable character controller look and movement
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // Disable character controller so clicks don't lock the mouse
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        UnlockCursor();

        HideAllPanels();
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        HideAllPanels();
        SetHUDActive(true);

        LockCursor();

        // Re-enable character controller
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }

    public void ShowStartMenu()
    {
        isPlaying = false;
        isPaused = false;
        Time.timeScale = 0f;

        // Keep character controller off while browsing menus
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        UnlockCursor();

        HideAllPanels();
        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        SetHUDActive(false);
    }

    public void ShowGameOver()
    {
        isPlaying = false;
        Time.timeScale = 0f;

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        UnlockCursor();

        HideAllPanels();
        SetHUDActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // --- Submenu Navigation ---

    public void OpenOptionsMenu(GameObject fromPanel)
    {
        previousMenu = fromPanel;
        HideAllPanels();
        if (optionsMenu != null) optionsMenu.SetActive(true);
    }

    public void OpenCreditsMenu(GameObject fromPanel)
    {
        previousMenu = fromPanel;
        HideAllPanels();
        if (creditsMenu != null) creditsMenu.SetActive(true);
    }

    public void OpenControlsMenu(GameObject fromPanel)
    {
        previousMenu = fromPanel;
        HideAllPanels();
        if (controlsMenu != null) controlsMenu.SetActive(true);
    }

    public void BackToPrevious()
    {
        HideAllPanels();
        if (previousMenu != null)
        {
            previousMenu.SetActive(true);
        }
        else if (isPlaying)
        {
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        }
        else
        {
            if (startMenuPanel != null) startMenuPanel.SetActive(true);
        }
    }

    // --- Actions & Settings ---

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void SetHUDActive(bool active)
    {
        if (healthBar != null) healthBar.SetActive(active);
        if (crosshair != null) crosshair.SetActive(active);
    }

    private void HideAllPanels()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (creditsMenu != null) creditsMenu.SetActive(false);
        if (controlsMenu != null) controlsMenu.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
