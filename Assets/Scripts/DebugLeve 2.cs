using UnityEngine;
using UnityEngine.InputSystem; 

public class DebugLeve : MonoBehaviour
{
    [Header("Referencias")]
    public LanzadorArma lanzador;       
    public InputActionAsset inputAsset; 
    
    [Header("Arma de Regalo (Tecla 2)")]
    public GameObject prefabArmaNueva;  

    private InputAction subirNivelAction;
    private InputAction darArmaAction;

    void Awake()
    {
        // Buscamos el mapa de controles "Player"
        var actionMap = inputAsset.FindActionMap("Player");
        
        // Buscamos las acciones que creaste
        subirNivelAction = actionMap.FindAction("DebugLevelUp");
        darArmaAction = actionMap.FindAction("DebugGiveWeapon");
    }

    void OnEnable()
    {
        subirNivelAction.Enable();
        darArmaAction.Enable();

        subirNivelAction.performed += Context => SubirNivel();
        darArmaAction.performed += Context => DarNuevaArma();
    }

    void OnDisable()
    {
        subirNivelAction.Disable();
        darArmaAction.Disable();
    }

    // --- FUNCIONES ---

    void SubirNivel()
    {
        if(lanzador != null)
        {
            lanzador.level++; 
            Debug.Log($"⚡ TRUCO: ¡Nivel Subido! Ahora es Nivel {lanzador.level}");
        }
    }

    void DarNuevaArma()
    {
        if(lanzador != null && prefabArmaNueva != null)
        {
            lanzador.armaPrefab = prefabArmaNueva;
            Debug.Log("🎁 TRUCO: ¡Arma cambiada!");
        }
    }
}