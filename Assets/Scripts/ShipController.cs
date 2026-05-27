using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject bulletPrefab;
    public Transform firePoint;
    public BulletPooling bulletPool;

    public CrosshairController crosshairController;

    Vector2 inputMovement;

    public float moveSpeed = 5f;
    public float forwardSpeed = 20f;
    public float lateralSpeed = 5f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        // Avance automático
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Control del jugador solo en X/Y
        transform.Translate(
            -inputMovement.x * lateralSpeed * Time.deltaTime,
            -inputMovement.y * lateralSpeed * Time.deltaTime,
            0
        );
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -5f, 5f);
        pos.y = Mathf.Clamp(pos.y, -2f, 1f);
        transform.position = pos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject bullet = bulletPool.GetBullet();

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;

                // Dirección hacia la mira
                Vector3 targetPos = crosshairController.GetWorldPosition(100f);
                Vector3 direction = (targetPos - firePoint.position).normalized;

                BulletMovement bm = bullet.GetComponent<BulletMovement>();
                if (bm != null)
                    bm.Init(direction);
            }
        }
    }

}
