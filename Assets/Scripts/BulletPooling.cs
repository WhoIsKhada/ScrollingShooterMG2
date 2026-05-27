using UnityEngine;
using System.Collections.Generic;

// ÚNICO CAMBIO ESTRUCTURAL: hereda de BulletPoolBase en vez de MonoBehaviour.
// Toda la lógica interna es idéntica a tu versión original.
public class BulletPooling : BulletPoolBase
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int poolSize = 20;

    private readonly Queue<GameObject> availableBullets = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateBullet(i);
        }
    }

    private void CreateBullet(int i)
    {
        GameObject bullet = Instantiate(bulletPrefab, Vector3.down * 1000f, Quaternion.identity);
        bullet.GetComponent<BulletMovement>().bulletPool = this;
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }

    // GetBullet() sin parámetros para mantener compatibilidad con ShipController.
    public GameObject GetBullet()
    {
        if (availableBullets.Count > 0)
        {
            GameObject bullet = availableBullets.Dequeue();

            // Asegurar que la bala del jugador siempre viaje hacia adelante.
            bullet.GetComponent<BulletMovement>().Init(Vector3.forward);

            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            Debug.LogWarning("No bullets available in the pool!");
            return null;
        }
    }

    // Implementación requerida por BulletPoolBase (igual a tu ReturnBullet original).
    public override void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }
}
