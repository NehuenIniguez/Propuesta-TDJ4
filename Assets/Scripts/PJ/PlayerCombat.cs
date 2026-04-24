using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float rangoAtaque = 1f;
    public int daño = 10;
    public float cooldown = 0.5f;

    private float tiempoSiguienteAtaque = 0f;

    public LayerMask capaEnemigos;
    private Stats stats;

    void Start()
    {
        stats = GetComponent<Stats>();
    }

    void Update()
    {
        if (Time.time >= tiempoSiguienteAtaque)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Atacar();
                tiempoSiguienteAtaque = Time.time + cooldown;
            }
        }
    }

    void Atacar()
    {
        float multiplicador = 1f;

        // 🍺 EBriedad afecta combate
        if (stats != null)
        {
            float ebriedad = stats.GetPorcentajeEbriedad();

            // 🔴 muy borracho → podés fallar
            if (ebriedad > 0.7f && Random.value < 0.3f)
            {
                Debug.Log("Fallaste el golpe por borracho");
                return;
            }

            // 🟠 borracho → menos daño
            if (ebriedad > 0.5f)
            {
                multiplicador = 0.7f;
            }
        }

        Collider2D[] enemigos = Physics2D.OverlapCircleAll(
            transform.position,
            rangoAtaque,
            capaEnemigos
        );

        foreach (Collider2D enemigo in enemigos)
        {
            GauchoEnemy enemy = enemigo.GetComponent<GauchoEnemy>();
            if (enemy != null)
            {
                int dañoFinal = Mathf.RoundToInt(daño * multiplicador);
                enemy.RecibirDaño(dañoFinal);
            }
        }

        Debug.Log("Golpe!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
