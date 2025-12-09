using UnityEngine;

public class Bumeran : MonoBehaviour
{
    [Header("Estadísticas del Bumerán")]
    public float speed = 15f;           // Velocidad de vuelo
    public int damage = 20;             // Daño al golpear
    public float tiempoIda = 1.0f;      // Tiempo que viaja recto antes de volver

    // Variables internas
    private Transform player;           // Para saber dónde está el jugador
    private bool volviendo = false;     // ¿Está en fase de vuelta?
    private float timer = 0f;           // Contador de tiempo

    void Start()
    {
        // Buscamos al Player automáticamente (igual que lo hace tu enemigo)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Destrucción de seguridad por si se pierde (5 segundos)
        Destroy(gameObject, 5f); 
    }

    void Update()
    {
        timer += Time.deltaTime;

        // FASE 1: IDA (Va recto hacia donde miraba al nacer)
        if (!volviendo)
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            // Si pasa el tiempo de ida, activamos la vuelta
            if (timer >= tiempoIda)
            {
                volviendo = true;
            }
        }
        // FASE 2: VUELTA (Persigue al jugador)
        else
        {
            if (player != null)
            {
                // Calculamos dirección hacia el jugador
                Vector3 direccion = (player.position - transform.position).normalized;
                transform.position += direccion * speed * Time.deltaTime;

                // Si está muy cerca del jugador (distancia < 1), lo "atrapamos" y se destruye
                if (Vector3.Distance(transform.position, player.position) < 1.0f)
                {
                    Destroy(gameObject);
                }
            }
        }

        // ROTACIÓN VISUAL: Hacemos que gire sobre sí mismo mientras vuela
        transform.Rotate(Vector3.up * 800 * Time.deltaTime);
    }

    // Lógica de daño (Copiada y adaptada de tu Hacha)
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
}