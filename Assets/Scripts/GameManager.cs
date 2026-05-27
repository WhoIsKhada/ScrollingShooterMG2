using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameUI gameUI;

    [Header("Condición de Victoria")]
    [Tooltip("Cantidad de enemigos que el jugador debe eliminar para ganar")]
    [SerializeField] private int enemiesToKill = 5;

    [Header("Escenas (deben estar en Build Settings)")]
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string winScene = "Win";

    private int _currentKills = 0;
    private bool _gameEnded = false;

    // Propiedades públicas para que la UI pueda leer los datos
    public int CurrentKills => _currentKills;
    public int TargetKills => enemiesToKill;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (playerHealth == null)
        {
            Debug.LogError("[GameManager] ¡Asigna PlayerHealth en el Inspector!");
            return;
        }

        playerHealth.OnPlayerDeath.AddListener(TriggerGameOver);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnPlayerDeath.RemoveListener(TriggerGameOver);
    }

    // ── Método para sumar bajas ─────────────────────────
    public void AddKill()
    {
        if (_gameEnded) return;

        _currentKills++;
        Debug.Log($"[GameManager] Enemigo eliminado: {_currentKills}/{enemiesToKill}");

        if (_currentKills >= enemiesToKill)
        {
            TriggerWin();
        }
    }

    // ── Condiciones de fin ─────────────────────────────────────
    public void TriggerGameOver()
    {
        if (_gameEnded) return;
        _gameEnded = true;

        Debug.Log("[GameManager] GAME OVER");
        Time.timeScale = 0f;

        // --- LIBERAMOS EL RATÓN ---
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (gameUI != null) gameUI.ShowGameOver();
        else if (!string.IsNullOrEmpty(gameOverScene)) SceneManager.LoadScene(gameOverScene);
    }

    public void TriggerWin()
    {
        if (_gameEnded) return;
        _gameEnded = true;

        Debug.Log("[GameManager] ¡VICTORIA!");
        Time.timeScale = 0f;

        // --- LIBERAMOS EL RATÓN ---
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (gameUI != null) gameUI.ShowWin();
        else if (!string.IsNullOrEmpty(winScene)) SceneManager.LoadScene(winScene);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}