using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // --- REFERENCIAS Y DATOS ---
    
    // Referencia al Player para saber hacia dónde moverse
    private GameObject player;

    // Referencia al archivo de datos (ScriptableObject)
    // Esto es muy profesional: Separa los datos (Stats) del comportamiento (Controller).
    public EnemyStats Stats;

    // Variables locales. Copiamos los datos aquí para que si modificamos
    // la vida de un enemigo en concreto, no afecte al archivo original.
    private int maxHP;
    private int currentHP;
    private int damage;
    private int defense;
    public float speed;
    
    // VARIABLES DE CONTROL DE ATAQUE (Cooldown)
    private float tiempoUltimoAtaque = 0f; // Marca de tiempo del último golpe
    private float cooldownAtaque = 1.0f;   // Frecuencia: Solo ataca cada 1 segundo

    // --- CICLO DE VIDA ---

    void Awake()
    {
        // 1. INICIALIZACIÓN DE ESTADÍSTICAS
        // En el Awake (antes de empezar), leemos el archivo "Stats" y volcamos
        // los números a las variables del enemigo.
        if (Stats != null)
        {
            maxHP = Stats.MaxHP;
            currentHP = maxHP; // Empezamos con la vida llena
            damage = Stats.Damage;
            defense = Stats.Defense;
            speed = Stats.Speed;
        }
    }
    
    void Start()
    {
        // 2. BUSCAR OBJETIVO
        // Buscamos al jugador por su etiqueta (Tag) para perseguirlo.
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 3. INTELIGENCIA ARTIFICIAL (SEGUIMIENTO)
        if (player != null)
        {
            // A. Calculamos el vector dirección: (Destino - Origen)
            Vector3 direccion = player.transform.position - transform.position;
            
            // B. Normalizamos: Nos quedamos solo con la dirección (flecha de tamaño 1),
            // ignorando la distancia. Si no hacemos esto, correría más rápido cuanto más lejos esté.
            direccion.Normalize();

            // C. Movemos: Dirección * Velocidad * Time.deltaTime
            // Usamos Time.deltaTime para que se mueva suave independientemente de los FPS.
            transform.position += direccion * speed * Time.deltaTime;
        }
    }

    // --- SISTEMA DE ATAQUE (HACER DAÑO) ---

    // Usamos OnCollisionStay porque el enemigo suele quedarse "pegado" al jugador empujando.
    private void OnCollisionStay(Collision collision)
    {
        // 1. FILTRO: ¿He chocado contra el Jugador?
        if (collision.gameObject.CompareTag("Player"))
        {
            // 2. LOGICA DE COOLDOWN (ENFRIAMIENTO)
            // Comprobamos si ha pasado más de 1 segundo desde el último golpe.
            // Time.time es el reloj global del juego.
            if (Time.time > tiempoUltimoAtaque + cooldownAtaque)
            {
                // 3. Buscamos el componente de vida del jugador
                PlayerStats statsJugador = collision.gameObject.GetComponent<PlayerStats>();
                
                if (statsJugador != null)
                {
                    // 4. Ejecutamos el daño
                    statsJugador.RecibirDmg(damage);
                    
                    // 5. Reiniciamos el reloj para que tenga que esperar otro segundo
                    tiempoUltimoAtaque = Time.time;
                }
            }
        }
    }

    // --- SISTEMA DE DEFENSA (RECIBIR DAÑO) ---
    public void Recibirdano(int danio)
    {
        // Cálculo de mitigación de daño (Daño - Defensa)
        int danioFinal = danio - defense;
        
        // Evitamos que el daño sea negativo (que nos cure)
        if (danioFinal < 0)
        {
            danioFinal = 0;
        }

        currentHP -= danioFinal;

        // Si la vida llega a cero, muere
        if (currentHP <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        // Aquí podríamos poner animaciones o partículas antes de destruir
        Destroy(gameObject);
    }
}