using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject[] enemyPattern;

    [Header("Límites del túnel")]
    public float tunnelHalfWidth = 15f;
    public float tunnelHalfHeight = 5f;
    public float spawnDistance = 100f;
    public float spawnDepthRange = 200f;

    public float spacingBetweenPatterns = 30f;

    private float _nextSpawnZ = 0f;
    private Transform _player;

    [Header("Dificultad")]
    [Range(0.5f, 5f)]
    public float difficulty = 1f;

    [Header("Spawn")]
    public int enemiesPerWave = 3; // cuántos patrones por llamada

    int currentPatternIndex = 0;
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _nextSpawnZ = _player.position.z + spawnDistance;
        InvokeRepeating("spawnNextPattern", 0f, 2f / difficulty);
    }

    void Update() { }

    void spawnNextPattern()
    {
        // Asegurarse que el próximo spawn siempre esté adelante del jugador
        if (_nextSpawnZ < _player.position.z + spawnDistance)
            _nextSpawnZ = _player.position.z + spawnDistance;

        for (int j = 0; j < enemiesPerWave; j++)
        {
            SpawnPattern(currentPatternIndex);
            currentPatternIndex = (currentPatternIndex + 1) % enemyPattern.Length;
            _nextSpawnZ += spacingBetweenPatterns;
        }
    }

    void SpawnPattern(int i)
    {
        Vector3 spawnPos = new Vector3(0f, 0f, _nextSpawnZ);
        Instantiate(enemyPattern[i], spawnPos, transform.rotation);
    }
}