using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private Transform firePoint;

    [Header("Inteligencia Artificial")]
    [SerializeField] private float shootRange = 40f; // Distancia máxima para empezar a disparar
    [SerializeField] private bool onlyShootIfInFront = true; // Si es true, dejará de disparar cuando lo pases

    private float _fireTimer;
    private Transform _player;

    private void OnEnable()
    {
        _fireTimer = Random.Range(0f, fireRate);
        _player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        // 1. Calculamos la distancia entre el enemigo y tú
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        // 2. Comprobamos si estás en frente (tu posición Z es menor a la del enemigo)
        bool playerIsInFront = _player.position.z < transform.position.z;

        // 3. Solo reducimos el timer y disparamos SI estás en rango, y (opcionalmente) si estás al frente
        if (distanceToPlayer <= shootRange && (!onlyShootIfInFront || playerIsInFront))
        {
            _fireTimer -= Time.deltaTime;
            if (_fireTimer <= 0f)
            {
                Shoot();
                _fireTimer = fireRate;
            }
        }
    }

    private void Shoot()
    {
        if (EnemyBulletPooling.Instance == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject bullet = EnemyBulletPooling.Instance.GetBullet(spawnPos, Quaternion.identity);

        if (bullet != null)
        {
            BulletMovement bm = bullet.GetComponent<BulletMovement>();
            if (bm != null)
            {
                bm.Init(Vector3.back);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujamos el punto de disparo
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.5f);
        }

        // Dibujamos el rango de visión para que puedas ajustarlo visualmente en el Inspector
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}