using UnityEngine;

public class Hacha : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    public float speed = 10f;      // Velocidad de desplazamiento
    public float tiempoVida = 5f;  // Tiempo antes de autodestruirse (para no llenar la memoria)
    public int damage = 25;        // Daño que aplica al enemigo

    void Start()
    {
        // LIMPIEZA AUTOMÁTICA:
        // Programamos su destrucción nada más nacer.
        // Si no golpea a nadie en 5 segundos, se borra para no saturar el juego.
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // LÓGICA DE MOVIMIENTO:
        // Usamos 'transform.forward' (el eje Z azul del propio objeto) en lugar de Vector3.forward.
        // Esto asegura que el hacha avance "hacia donde está mirando", no hacia el Norte del mapa.
        // Multiplicamos por Time.deltaTime para que la velocidad sea estable independientemente de los FPS.
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // GESTIÓN DE IMPACTOS
    private void OnTriggerEnter(Collider other) 
    {
        // 1. FILTRO: ¿Es un enemigo?
        if (other.CompareTag("Enemy"))
        {
            // 2. COMUNICACIÓN ENTRE SCRIPTS:
            // Buscamos el componente 'EnemyController' dentro del objeto golpeado.
            EnemyController enemy = other.GetComponent<EnemyController>();
            
            // 3. APLICACIÓN DE DAÑO:
            if (enemy != null)
            {
                enemy.Recibirdano(damage);
                
                // NOTA PARA DEFENSA: 
                // Tal y como está el código ahora, el Hacha ATRAVIESA enemigos (daño en área lineal).
                // Si quisieras que se destruya al chocar, añadirías aquí: Destroy(gameObject);
            }
        }
    }
}