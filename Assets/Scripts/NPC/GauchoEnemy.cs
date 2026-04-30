using UnityEngine;

public class GauchoEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int vida = 40;
    public float velocidad = 2f;
    public int daño = 10;

    [Header("Combate")]
    public float rangoDeteccion = 5f;
    public float rangoAtaque = 1.2f;
    public float cooldownAtaque = 1.5f;

    private Transform jugador;
    private float tiempoSiguienteAtaque = 0f;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            jugador = obj.transform;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        // 👉 seguir jugador
        if (distancia < rangoDeteccion)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                jugador.position,
                velocidad * Time.deltaTime
            );
        }

        // 👉 atacar
        if (distancia < rangoAtaque && Time.time >= tiempoSiguienteAtaque)
        {
            Atacar();
            tiempoSiguienteAtaque = Time.time + cooldownAtaque;
        }
    }

    void Atacar()
    {
        Stats stats = jugador.GetComponent<Stats>();

        if (stats != null)
        {
            // 🔥 efecto simple: subir ebriedad o castigar
            stats.ebriedadActual += 10f;

            Debug.Log("El gaucho te pegó");
        }
    }

    // 👉 recibir daño del jugador
    public void RecibirDaño(int dañoRecibido)
    {
        vida -= dañoRecibido;

        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        MisionCombate mision = FindObjectOfType<MisionCombate>();
    
        if (mision != null)
        {
            mision.EnemigoDerrotado();
        }
    
        gameObject.SetActive(false);
    }

    // 👀 debug visual
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
