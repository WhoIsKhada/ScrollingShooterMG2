using UnityEngine;

/// <summary>
/// Clase base para todos los pools de balas.
/// Permite que BulletMovement devuelva la bala sin importar
/// si fue creada por BulletPooling (jugador) o EnemyBulletPooling (enemigo).
/// </summary>
public abstract class BulletPoolBase : MonoBehaviour
{
    public abstract void ReturnBullet(GameObject bullet);
}
