using UnityEngine;

public class EscudoOrbital : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadRotacion = 100f; // Velocidad de giro
    public int damage = 15;

    // Detectar colisiones
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Recibirdano(damage);
            }
        }
    }

    void Update()
    {
        // Gira alrededor del objeto padre (el Player)
        if (transform.parent != null)
        {
            // Usamos RotateAround para orbitar sobre el eje Y (arriba) del padre
            transform.RotateAround(transform.parent.position, Vector3.up, velocidadRotacion * Time.deltaTime);
        }
    }
}