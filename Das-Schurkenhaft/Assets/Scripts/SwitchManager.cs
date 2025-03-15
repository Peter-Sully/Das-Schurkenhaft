using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private PlayerController playerController;
    public InputAction sceneSwitchAction;
    private bool isSceneSwitching = false;
    private float lastSwitchTime = 0f;
    private float switchCooldown = 0.5f; // Cooldown period in seconds

    // Singleton instance

    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void OnEnable()
    {
        sceneSwitchAction.Enable();
        sceneSwitchAction.performed += OnSceneSwitch;
    }

    void OnDisable()
    {
        sceneSwitchAction.performed -= OnSceneSwitch;
        sceneSwitchAction.Disable();
    }

    // The logic to switch between scenes
    private void OnSceneSwitch(InputAction.CallbackContext context)
    {
        if (context.performed && !isSceneSwitching && Time.time - lastSwitchTime > switchCooldown)
        {
            isSceneSwitching = true;
            lastSwitchTime = Time.time;
            sceneSwitchAction.Disable();

            // Determine which scene you're in, and switch accordingly
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerController = player.GetComponent<PlayerController>();
                    GameData.playerPosition = playerController.GetPosition();
                }
                // Switch to Scene2 (without player controller)
                SceneManager.LoadScene("DeckBuilderScene");
            }
            else if (SceneManager.GetActiveScene().name == "DeckBuilderScene")
            {
                // Switch to Scene1 (with player controller)
                //GameManager.Instance.SpawnEnemiesInMainScene(enemyPrefabs);
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start a coroutine to wait before re-enabling input
        sceneSwitchAction.Disable();
        StartCoroutine(EnableInputAfterDelay());
    }

    private IEnumerator EnableInputAfterDelay()
    {
        // Wait for 1 second (you can adjust this time)
        yield return new WaitForSeconds(1f);

        // Re-enable the input action after the delay
        sceneSwitchAction.Enable();

        // Reset the flag to allow further scene switches
        isSceneSwitching = false;
    }
}