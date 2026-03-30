using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Stats statsPJ;

    public float speed = 5f;

    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        statsPJ = GetComponent<Stats>();
    }

    void Update()
    {
        // Solo leemos input acá
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        float multiplicador = 1f;

        // 🧠 Estados según ebriedad
        if (statsPJ.ebriedadActual >= 30f && statsPJ.ebriedadActual < 70f)
        {
            multiplicador = 0.75f;
        }
        else if (statsPJ.ebriedadActual >= 70f)
        {
            multiplicador = -0.5f; // invertido + más lento
        }

        Vector2 movimiento = new Vector2(moveHorizontal, moveVertical) * speed * multiplicador;

        rb2D.linearVelocity = movimiento;
    }
}
