using UnityEngine;

public class EscudoOrbital : MonoBehaviour
{
    [Header("Estadísticas Base")]
    public float velocidadBase = 100f; // Velocidad de giro inicial
    public int damageBase = 15;        // Daño inicial por golpe

    // Referencia al script del jugador para leer el Nivel actual
    private LanzadorArma lanzadorJugador;

    void Start()
    {
        // 1. CONEXIÓN JERÁRQUICA
        // Buscamos el componente en el PADRE. 
        // ¿Por qué? Porque el Escudo se instancia como un hijo (Child) del Player 
        // para que le siga automáticamente cuando se mueve.
        lanzadorJugador = GetComponentInParent<LanzadorArma>();
    }

    void Update()
    {
        // Seguridad: Solo giramos si tenemos un "centro" sobre el que orbitar
        if (transform.parent != null)
        {
            // --- LÓGICA DE ESCALADO (NIVEL) ---
            // Obtenemos el nivel actual. Si por error es null, asumimos nivel 1.
            int nivel = (lanzadorJugador != null) ? lanzadorJugador.level : 1;

            // FÓRMULA DE PROGRESIÓN (Mecánica Única):
            // La velocidad aumenta drásticamente con el nivel.
            // A más velocidad = Más vueltas por segundo = Más veces golpea a los enemigos.
            float velocidadActual = velocidadBase + (nivel * 50f);

            // --- MOVIMIENTO ORBITAL ---
            // Función clave de Unity: RotateAround.
            // 1. Punto: La posición del padre (el Player).
            // 2. Eje: Vector3.up (El eje Y, vertical), para que gire como un satélite horizontal.
            // 3. Ángulo: Velocidad * Time.deltaTime.
            transform.RotateAround(transform.parent.position, Vector3.up, velocidadActual * Time.deltaTime);
        }
    }

    // GESTIÓN DE IMPACTOS
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // CÁLCULO DE DAÑO DINÁMICO
                // El daño también sube con el nivel (+2 por nivel).
                int nivel = (lanzadorJugador != null) ? lanzadorJugador.level : 1;
                int danioFinal = damageBase + (nivel * 2); 
                
                enemy.Recibirdano(danioFinal);
            }
        }
    }
}