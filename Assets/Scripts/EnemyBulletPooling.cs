using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pool de balas ENEMIGAS. Misma arquitectura que BulletPooling del jugador.
///
/// SETUP EN ESCENA:
///   1. Crea un GameObject vacío llamado "EnemyBulletPool".
///   2. Adjunta este script.
///   3. Asigna el prefab de bala enemiga (puede ser el mismo que el del jugador
///      pero con distinto material/color para diferenciarlos visualmente).
///   4. Pool Size: 30 es suficiente para la mayoría de casos.
/// </summary>
public class EnemyBulletPooling : BulletPoolBase
{
    // Singleton para que EnemyShooting lo encuentre sin necesidad de referencias manuales.
    public static EnemyBulletPooling Instance { get; private set; }

    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private int poolSize = 30;

    private readonly Queue<GameObject> availableBullets = new Queue<GameObject>();

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Pre-instanciar el pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab);
            bullet.GetComponent<BulletMovement>().bulletPool = this;
            bullet.SetActive(false);
            availableBullets.Enqueue(bullet);
        }
    }

    /// <summary>
    /// Obtiene una bala del pool y la lanza en dirección Vector3.back (hacia el jugador).
    /// Llamado desde EnemyShooting.
    /// </summary>
    public GameObject GetBullet(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject bullet;

        if (availableBullets.Count > 0)
        {
            bullet = availableBullets.Dequeue();
        }
        else
        {
            Debug.LogWarning("EnemyBulletPool: pool vacío, instanciando bala extra.");
            bullet = Instantiate(enemyBulletPrefab);
            bullet.GetComponent<BulletMovement>().bulletPool = this;
        }

        bullet.transform.SetPositionAndRotation(spawnPosition, spawnRotation);

        // Inicializar con dirección hacia atrás (hacia donde viene el jugador)
        bullet.GetComponent<BulletMovement>().Init(Vector3.back);

        bullet.SetActive(true);
        return bullet;
    }

    // Implementación requerida por BulletPoolBase
    public override void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }
}
