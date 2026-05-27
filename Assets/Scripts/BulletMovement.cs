using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public BulletPoolBase bulletPool;
    private Vector3 _direction = Vector3.forward;

    [Header("Homing")]
    public bool isHoming = false;
    public float homingStrength = 3f; // qué tan rápido gira hacia el jugador
    private Transform _player;

    // NUEVO: El interruptor para apagar el homing
    private bool _hasPassedPlayer = false;

    public void Init(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("ReturnToPool", lifeTime);

        // NUEVO: Reiniciamos el interruptor cada vez que la bala sale del pool
        _hasPassedPlayer = false;

        if (isHoming)
            _player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        // NUEVO: Modificamos la condición para que solo gire si AÚN NO pasó al jugador
        if (isHoming && _player != null && !_hasPassedPlayer)
        {
            // Verificamos si la bala ya cruzó la línea Z del jugador
            if (transform.position.z < _player.position.z)
            {
                _hasPassedPlayer = true; // ¡Apagamos el radar!
            }
            else
            {
                // Tu lógica original de giro matemático
                Vector3 dirToPlayer = (_player.position - transform.position).normalized;
                _direction = Vector3.Lerp(_direction, dirToPlayer, homingStrength * Time.deltaTime).normalized;
            }
        }

        // Calculamos cuánto se va a mover en este frame
        float stepDistance = speed * Time.deltaTime;

        // Lanzamos un rayo para predecir si vamos a atravesar un collider en este frame
        if (Physics.Raycast(transform.position, _direction, out RaycastHit hit, stepDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            ProcessHit(hit.collider);
        }
        else
        {
            // Si no hay nada en el camino, nos movemos normalmente
            transform.Translate(_direction * stepDistance, Space.World);
        }

        // Desactivar si quedó muy atrás del jugador (esto ya lo tenías y funciona perfecto)
        if (_player != null && transform.position.z < _player.position.z - 30f)
            ReturnToPool();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessHit(other);
    }

    private void ProcessHit(Collider other)
    {
        if (!gameObject.activeInHierarchy) return;

        if (gameObject.CompareTag("PlayerBullet"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                ReturnToPool();
            }
            return;
        }

        if (gameObject.CompareTag("EnemyBullet"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                ReturnToPool();
            }
        }
    }

    void ReturnToPool()
    {
        if (bulletPool != null)
            bulletPool.ReturnBullet(gameObject);
        else
            gameObject.SetActive(false);
    }
}