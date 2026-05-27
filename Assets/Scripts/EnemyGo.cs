using UnityEngine;

public class EnemyGo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 5f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Destruir si quedó detrás del jugador
        if (transform.position.z < player.position.z - 20f)
        {
            Debug.Log("Enemigo destruido: quedó atrás del jugador");
            Destroy(gameObject);
        }
           
    }
}
