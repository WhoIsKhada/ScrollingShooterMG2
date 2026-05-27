using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int maxHealth = 3;

    [Header("Invencibilidad tras recibir daño")]
    [SerializeField] private float invincibilityDuration = 1.5f;

    [Header("Eventos")]
    // Evento que dispara el GameManager cuando la vida llega a 0
    public UnityEvent OnPlayerDeath;

    // NUEVO: Evento que envía la vida actual a la UI
    public UnityEvent<int> OnHealthChanged;

    // Propiedad pública para que el GameManager o la UI puedan leerla
    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    private bool _isInvincible = false;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        // NUEVO: Le enviamos la vida máxima a la UI nada más empezar el nivel
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (_isInvincible) return;

        CurrentHealth -= amount;

        // NUEVO: Le avisamos a la UI exactamente cuánta vida nos queda
        OnHealthChanged?.Invoke(CurrentHealth);

        Debug.Log($"[PlayerHealth] Vida restante: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
        {
            // Período de invencibilidad para no recibir daño en cadena
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        _isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("[PlayerHealth] ¡Jugador muerto!");
        OnPlayerDeath?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}