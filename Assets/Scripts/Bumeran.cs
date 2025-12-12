using UnityEngine;

public class Bumeran : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public float speed = 15f;      // Velocidad de vuelo
    public int damage = 20;        // Daño al impactar
    public float tiempoIda = 1.0f; // Tiempo que viaja recto antes de volver
    
    // Nivel actual del arma (recibido desde el lanzador)
    public int nivelArma = 1; 

    // REFERENCIAS Y VARIABLES DE CONTROL
    private Transform player;        // Para saber dónde está el jugador y volver a él
    private bool volviendo = false;  // ESTADO: ¿Se aleja (false) o regresa (true)?
    private float timer = 0f;        // Cronómetro para controlar el tiempo de ida
    private int rebotesRestantes;    // Mecánica Única: Cuántas veces vuelve a salir

    void Start()
    {
        // 1. Localizamos al jugador por su etiqueta para tener un objetivo de retorno
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // 2. CONFIGURACIÓN DE LA MECÁNICA ÚNICA (Rebotes)
        // Calculamos los rebotes basándonos en el nivel.
        // Nivel 1 = 0 rebotes extra. Nivel 3 = 2 rebotes extra, etc.
        rebotesRestantes = nivelArma - 1; 
        
        // 3. Seguridad: Se autodestruye a los 10s si se pierde por el mapa
        Destroy(gameObject, 10f); 
    }

    void Update()
    {
        timer += Time.deltaTime; // Actualizamos el reloj interno

        // --- LÓGICA DE MOVIMIENTO (MÁQUINA DE ESTADOS) ---

        // ESTADO 1: FASE DE IDA (El bumerán se aleja)
        if (!volviendo)
        {
            // Se mueve hacia adelante (Forward) en línea recta
            transform.position += transform.forward * speed * Time.deltaTime;

            // Si se acaba el tiempo de ida, cambiamos de estado
            if (timer >= tiempoIda)
            {
                volviendo = true; // Activamos la vuelta
            }
        }
        // ESTADO 2: FASE DE VUELTA (El bumerán persigue al jugador)
        else
        {
            if (player != null)
            {
                // Calculamos la dirección hacia el jugador (Vector resta)
                Vector3 direccion = (player.position - transform.position).normalized;
                transform.position += direccion * speed * Time.deltaTime;

                // COMPROBACIÓN DE LLEGADA (Distancia < 1 metro)
                if (Vector3.Distance(transform.position, player.position) < 1.0f)
                {
                    // --- LÓGICA DE REBOTE (Efecto Único) ---
                    if (rebotesRestantes > 0)
                    {
                        // Si le quedan rebotes, lo "reiniciamos" para que salga disparado otra vez
                        rebotesRestantes--; // Restamos un uso
                        volviendo = false;  // Lo devolvemos al ESTADO 1 (Ida)
                        timer = 0f;         // Reiniciamos el cronómetro
                        
                        // Truco visual: Copiamos la rotación del jugador para salir hacia donde él mire
                        transform.rotation = player.rotation; 
                    }
                    else
                    {
                        // Si no quedan rebotes, el jugador lo atrapa (se destruye)
                        Destroy(gameObject);
                    }
                }
            }
        }

        // FEEDBACK VISUAL: Rotación constante sobre su eje Y
        transform.Rotate(Vector3.up * 800 * Time.deltaTime);
    }

    // GESTIÓN DE COLISIONES
    private void OnTriggerEnter(Collider other)
    {
        // Si chocamos con algo que tenga la etiqueta "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Buscamos su script de control para restarle vida
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Recibirdano(damage);
            }
            // NOTA: No destruimos el bumerán aquí porque atraviesa enemigos (Piercing)
        }
    }
}