using UnityEngine;
using System.Collections.Generic;

public class TunnelSpawner : MonoBehaviour
{
    [Header("Chunk")]
    public GameObject chunkPrefab;
    public float chunkLength = 100f;

    [Header("Spawn")]
    public int chunksAhead = 4;
    public int chunksBehind = 1;

    [Header("Material")]
    public Material tunnelMaterial;

    private Transform _player;
    private Queue<GameObject> _activeChunks = new Queue<GameObject>();
    private float _nextSpawnZ = 0f;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;

        for (int i = 0; i < chunksAhead + chunksBehind; i++)
            SpawnChunk();
    }

    public IEnumerable<GameObject> GetActiveChunks()
    {
        return _activeChunks;
    }

    void Update()
    {
        while (_nextSpawnZ < _player.position.z + chunksAhead * chunkLength)
            SpawnChunk();

        while (_activeChunks.Count > 0)
        {
            GameObject oldest = _activeChunks.Peek();
            if (oldest.transform.position.z + chunkLength < _player.position.z - chunksBehind * chunkLength)
            {
                _activeChunks.Dequeue();
                Destroy(oldest);
            }
            else break;
        }
    }



    void SpawnChunk()
    {
        Vector3 spawnPos = new Vector3(0f, 0f, _nextSpawnZ);
        GameObject chunk = Instantiate(chunkPrefab, spawnPos, Quaternion.identity);

        Material matInstance = new Material(tunnelMaterial);
        foreach (Renderer r in chunk.GetComponentsInChildren<Renderer>())
            r.material = matInstance;

        _activeChunks.Enqueue(chunk);
        _nextSpawnZ += chunkLength + 0.5f; // gap de 2 unidades entre chunks
    }
}