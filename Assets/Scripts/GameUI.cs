using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Pantalla de Controles")]
    [Tooltip("El Canvas Group del panel de controles para hacer el fade")]
    [SerializeField] private CanvasGroup controlsPanel;
    [SerializeField] private float controlsDisplayTime = 3f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Corazones")]
    [SerializeField] private Transform heartContainer;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Sprite heartFullSprite;
    [SerializeField] private Sprite heartEmptySprite;

    [Header("Contador de Bajas")]
    [SerializeField] private TextMeshProUGUI killCounterText;

    [Header("Paneles de fin de partida")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [Header("Referencias")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameManager gameManager;

    private Image[] _heartImages;

    private void Start()
    {
        ValidateReferences();
        BuildHearts();

        if (playerHealth != null)
            UpdateHearts(playerHealth.CurrentHealth);

        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);

        if (controlsPanel != null)
        {
            controlsPanel.gameObject.SetActive(true);
            controlsPanel.alpha = 1f;
            StartCoroutine(FadeOutControlsRoutine());
        }
    }

    private void Update()
    {
        UpdateKillCounter();
    }

    private IEnumerator FadeOutControlsRoutine()
    {
        yield return new WaitForSeconds(controlsDisplayTime);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            controlsPanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        controlsPanel.alpha = 0f;
        controlsPanel.gameObject.SetActive(false);
    }

    private void BuildHearts()
    {
        if (heartContainer == null || heartPrefab == null) return;

        foreach (Transform child in heartContainer)
            Destroy(child.gameObject);

        if (playerHealth == null) return;

        _heartImages = new Image[playerHealth.MaxHealth];

        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            _heartImages[i] = heart.GetComponent<Image>();
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        if (_heartImages == null) return;

        for (int i = 0; i < _heartImages.Length; i++)
        {
            bool isAlive = i < currentHealth;

            if (heartEmptySprite != null)
            {
                _heartImages[i].sprite = isAlive ? heartFullSprite : heartEmptySprite;
                _heartImages[i].color = Color.white;
            }
            else
            {
                _heartImages[i].color = isAlive ? Color.white : new Color(1f, 1f, 1f, 0.2f);
            }
        }
    }

    private void UpdateKillCounter()
    {
        if (killCounterText == null || gameManager == null) return;
        killCounterText.text = $"Enemigos: {gameManager.CurrentKills} / {gameManager.TargetKills}";
    }

    public void ShowGameOver()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowWin()
    {
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // ── Botones de UI ──────────────────────────────────────────

    /// <summary>Asigna al botón "Reintentar" del panel Game Over o Win.</summary>
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        if (gameManager) gameManager.RestartGame();
    }

    /// <summary>NUEVO: Asigna al botón "Salir" del panel Game Over o Win.</summary>
    public void OnQuitButton()
    {
        Debug.Log("[GameUI] Saliendo del juego...");
        Application.Quit(); // Funciona en el juego compilado (.exe)

#if UNITY_EDITOR
        // Detiene la simulación si se ejecuta desde el editor de Unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ValidateReferences()
    {
        if (playerHealth == null) Debug.LogError("[GameUI] Falta asignar PlayerHealth en el Inspector.");
        if (gameManager == null) Debug.LogError("[GameUI] Falta asignar GameManager en el Inspector.");
    }
}