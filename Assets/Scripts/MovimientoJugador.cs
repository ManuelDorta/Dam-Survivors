using UnityEngine;
// No necesitamos "using UnityEngine.InputSystem" aquí arriba porque 
// estamos usando la clase generada "Controles", que ya lo incluye dentro.

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuración")]
    private bool puedeMoverse = true; // Interruptor para detener al jugador (ej: en diálogos)
    private float velocidadMovimiento = 5f; // Velocidad en metros por segundo

    // Variables internas de control
    private Vector2 direccionPlana; // Input puro (X, Y) del mando/teclado
    public Controles control;       // Referencia a la clase generada por el Input System

    // --- CICLO DE VIDA DEL INPUT SYSTEM ---
    
    private void Awake()
    {
        // Instanciamos la clase de controles generada automáticamente.
        // Esto crea el mapa de teclas en memoria antes de empezar.
        control = new Controles();
    }

    private void OnEnable()
    {
        // IMPORTANTE: Con el nuevo Input System, debemos encender los controles
        // cuando el objeto se activa.
        control.Enable();
    }
    
    private void OnDisable()
    {
        // Y debemos apagarlos cuando se desactiva para liberar memoria y evitar errores.
        control.Disable();
    }

    void Update()
    {
        if (puedeMoverse)
        {
            // 1. LECTURA DE INPUT (2D)
            // Leemos el valor del Joystick o WASD. Devuelve un Vector2 (X, Y).
            // X = Izquierda/Derecha. Y = Arriba/Abajo.
            direccionPlana = control.Player.Move.ReadValue<Vector2>();

            // 2. CONVERSIÓN A 3D 
            // El suelo del juego es el plano X-Z. 
            // La "Y" del input (Arriba en el mando) se convierte en "Z" del mundo (Adelante).
            Vector3 direccionMovimiento = new Vector3(direccionPlana.x, 0f, direccionPlana.y);
            
            // 3. NORMALIZACIÓN (Matemáticas) 
            // Si pulsamos W+D a la vez, la longitud del vector sería 1.41 (más rápido).
            // .Normalize() hace que la longitud sea siempre 1, para no correr más en diagonal.
            direccionMovimiento.Normalize();

            // 4. APLICACIÓN DE MOVIMIENTO
            // Sumamos a la posición actual: Dirección * Velocidad * Tiempo entre frames.
            transform.position += direccionMovimiento * velocidadMovimiento * Time.deltaTime;
        }
    }
}