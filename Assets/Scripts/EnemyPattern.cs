using UnityEngine;

public class EnemyPattern : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;

    // Tamaño del rectángulo de spawn
    public float width = 20f;  // X
    public float height = 10f;  // Y

    void Start()
    {
        SpawnEnemies();
        Destroy(gameObject, 10f); // limpiar el patrón vacío después
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-width / 2f, width / 2f),
                Random.Range(-height / 2f, height / 2f),
                0f
            );
            Instantiate(enemyPrefab, transform.position + randomOffset, Quaternion.identity);
        }
    }

    // Visualizar el rectángulo en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}