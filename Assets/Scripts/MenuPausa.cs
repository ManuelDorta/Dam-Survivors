using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el nuevo sistema de entrada

public class MenuPausa : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelPausa;       // El objeto visual (Canvas/Panel) que tiene los botones
    public InputActionAsset inputAsset; // El archivo de configuración de controles (.inputactions)

    // Variables internas
    private InputAction pauseAction; // La "tecla" específica que vamos a escuchar
    private bool estaPausado = false; // Estado del juego (¿Corriendo o Quieto?)

    void Awake()
    {
        // 1. BÚSQUEDA DE LA ACCIÓN
        // No hardcodeamos teclas (no ponemos "Escape"). 
        // Buscamos la acción abstracta "Pause" dentro del mapa "Player".
        // Así funcionará igual si pulsas ESC en teclado o START en un mando.
        var actionMap = inputAsset.FindActionMap("Player");
        pauseAction = actionMap.FindAction("Pause");
    }

    // OnEnable y OnDisable son OBLIGATORIOS para gestionar eventos de Input System
    void OnEnable()
    {
        pauseAction.Enable(); // Encendemos la escucha
        
        // SUSCRIPCIÓN AL EVENTO:
        // "Cuando se realice (performed) la acción de Pausa, ejecuta la función AlternarPausa".
        // Esto es mucho más eficiente que preguntar en el Update todo el rato.
        pauseAction.performed += Context => AlternarPausa();
    }

    void OnDisable()
    {
        pauseAction.Disable(); // Apagamos la escucha al salir
    }

    // --- LÓGICA DE DETENCIÓN DEL TIEMPO ---

    public void AlternarPausa()
    {
        estaPausado = !estaPausado; // Invertimos el valor (True <-> False)

        if (estaPausado)
        {
            // EL TRUCO DEL TIEMPO:
            // Poner timeScale a 0 congela todo lo que use Time.deltaTime.
            // (Animaciones, Físicas, Movimiento de enemigos...).
            Time.timeScale = 0f; 
            
            panelPausa.SetActive(true); // Mostramos el menú visual
        }
        else
        {
            Reanudar();
        }
    }

    public void Reanudar()
    {
        estaPausado = false;
        
        // Volvemos a la normalidad (1 = velocidad normal)
        Time.timeScale = 1f; 
        
        panelPausa.SetActive(false); // Ocultamos el menú
    }

    public void Salir()
    {
        Debug.Log("👋 Saliendo del juego...");
        Application.Quit(); // Cierra la ventana del juego (solo en la Build final)
    }
}