using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// ////////////////////////////////// Variables ///////////////////////
    /// </summary>
    //Referencia al jugador//
    private GameObject player;

    //Info del SO//
    public EnemyStats Stats;

    //Stats propios//
    private int maxHP;
    private int currentHP;
    private int damage;
    private int defense;
    public float speed;
    
    /// <summary>
    /// /////////////////////////////////// Funciones Unity ///////////////////////////////
    /// </summary>
    void Awake()
    {
        maxHP = Stats.MaxHP;
        currentHP = maxHP;
        damage = Stats.Damage;
        defense = Stats.Defense;
        speed = Stats.Speed;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //Cojo la direccion
            Vector3 direccion = player.transform.position - transform.position;
            direccion.Normalize();

            //Moverme hacia el jugador
            transform.position += direccion * speed * Time.deltaTime;

        }
    }

    public void Recibirdano(int danio)
    {
        int danioFinal = danio - defense;
        if (danioFinal < 0)
        {
            danioFinal = 0;
        }
        currentHP -= danioFinal;
        if (currentHP <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Destroy(gameObject);
    }
}
