using UnityEngine;
using UnityEngine.InputSystem; // ⚠️ VITAL: Sin esto no funciona el sistema nuevo

public class DebugLevel : MonoBehaviour
{
    [Header("Conexiones")]
    public LanzadorArma lanzador;       // Referencia al script que dispara para poder modificarlo
    public InputActionAsset inputAsset; // El archivo azul (Input Actions) donde definimos las teclas
    
    [Header("Configuración de Trucos")]
    public GameObject prefabArmaNueva;  // El arma que daremos al pulsar la tecla (ej: Hacha)

    // VARIABLES DE CONTROL (Input System)
    // Guardamos las "Acciones" específicas para saber cuándo se pulsan
    private InputAction subirNivelAction;
    private InputAction darArmaAction;

    void Awake()
    {
        // 1. INICIALIZACIÓN DEL INPUT SYSTEM
        // Primero buscamos el "Mapa" de controles llamado "Player" dentro del archivo
        var actionMap = inputAsset.FindActionMap("Player");
        
        // 2. Buscamos las acciones por su nombre exacto (tal cual las escribimos en el editor)
        subirNivelAction = actionMap.FindAction("DebugLevelUp");    // Tecla 1
        darArmaAction = actionMap.FindAction("DebugGiveWeapon");    // Tecla 2
    }

    // OnEnable y OnDisable son OBLIGATORIOS cuando usas Input System por código.
    // Tienes que "encender" y "apagar" la escucha de teclas para evitar errores de memoria.
    void OnEnable()
    {
        // Encendemos las orejas (Empezamos a escuchar)
        subirNivelAction.Enable();
        darArmaAction.Enable();

        // SUSCRIPCIÓN A EVENTOS (El momento clave)
        // Le decimos: "Cuando esta acción sea 'performed' (realizada), ejecuta esta función".
        // La sintaxis '+= Context =>' es una expresión Lambda para conectar el evento.
        subirNivelAction.performed += Context => SubirNivel();
        darArmaAction.performed += Context => DarNuevaArma();
    }

    void OnDisable()
    {
        // Apagamos las orejas cuando el objeto se desactiva (limpieza)
        subirNivelAction.Disable();
        darArmaAction.Disable();
    }

    // --- LÓGICA DE LOS TRUCOS ---

    void SubirNivel()
    {
        // Comprobamos que tenemos la referencia para no generar errores (NullCheck)
        if(lanzador != null)
        {
            lanzador.level++; // Accedemos a la variable pública 'level' y la subimos
            Debug.Log($"⚡ TRUCO ACTIVADO: Nivel subido a {lanzador.level}");
        }
    }

    void DarNuevaArma()
    {
        if(lanzador != null && prefabArmaNueva != null)
        {
            // Intercambiamos el prefab del arma en tiempo real
            lanzador.armaPrefab = prefabArmaNueva;
            Debug.Log("🎁 TRUCO ACTIVADO: ¡Has recibido el " + prefabArmaNueva.name + "!");
        }
    }
}