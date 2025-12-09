using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    ///////////////////////////////////// VARIABLES /////////////////////////////////
    private bool puedeMoverse = true;
    private float velocidadMovimiento = 5f;
    private Vector2 direccionPlana;

    public Controles control;

    ///////////////////////////////////// FUNCIONES UNITY /////////////////////////////////
    /// 
    private void Awake()
    {
        control = new Controles();
    }

    private void OnEnable()
    {
        control.Enable();
    }
    
    private void OnDisable()
    {
        control.Disable();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     if (puedeMoverse)
        {
            // Coger el valor del vector 2 gracias a control
            direccionPlana = control.Player.Move.ReadValue<Vector2>();
            //Cargamos el vector 3 a través del vector2
            Vector3 direccionMovimiento = new Vector3(direccionPlana.x, 0f, direccionPlana.y);
            direccionMovimiento.Normalize();

            //Movemos el personaje
            transform.position += direccionMovimiento * velocidadMovimiento * Time.deltaTime;

        }
    }
    ///////////////////////////////////// FUNCIONES PROPIAS /////////////////////////////////

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
