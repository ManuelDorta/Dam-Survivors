using UnityEngine;
using UnityEngine.InputSystem; // Necesario para leer tus teclas nuevas

public class DebugLevel : MonoBehaviour
{
    [Header("Referencias")]
    public LanzadorArma lanzador;       // Tu script de disparo
    public InputActionAsset inputAsset; // El archivo azul del rayo (InputSystem)
    
    [Header("Arma de Regalo (Tecla 2)")]
    public GameObject prefabArmaNueva;  // El arma que te dará el truco (ej: Hacha)

    // Variables internas para las teclas
    private InputAction subirNivelAction;
    private InputAction darArmaAction;

    void Awake()
    {
        // 1. Buscamos el mapa de controles "Player" (el que sale a la izquierda en tu foto)
        var actionMap = inputAsset.FindActionMap("Player");
        
        // 2. Buscamos las acciones exactas que escribiste
        subirNivelAction = actionMap.FindAction("DebugLevelUp");
        darArmaAction = actionMap.FindAction("DebugGiveWeapon");
    }

    void OnEnable()
    {
        // Encendemos la escucha de teclas
        subirNivelAction.Enable();
        darArmaAction.Enable();

        // Les decimos qué hacer cuando las pulses
        subirNivelAction.performed += Context => SubirNivel();
        darArmaAction.performed += Context => DarNuevaArma();
    }

    void OnDisable()
    {
        // Apagamos la escucha si el objeto se desactiva
        subirNivelAction.Disable();
        darArmaAction.Disable();
    }

    // --- FUNCIONES DE LOS TRUCOS ---

    void SubirNivel()
    {
        if(lanzador != null)
        {
            lanzador.level++; // Suma 1 al nivel actual
            Debug.Log($"⚡ TRUCO: ¡Nivel Subido! Ahora es Nivel {lanzador.level}");
        }
    }

    void DarNuevaArma()
    {
        if(lanzador != null && prefabArmaNueva != null)
        {
            // Cambiamos el arma actual por la nueva (ej: cambia Bumerán por Hacha)
            lanzador.armaPrefab = prefabArmaNueva;
            Debug.Log("🎁 TRUCO: ¡Arma cambiada por " + prefabArmaNueva.name + "!");
        }
    }
}