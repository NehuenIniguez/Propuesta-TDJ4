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
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        float ebriedad = statsPJ.GetPorcentajeEbriedad();

        float velocidadFinal = speed;
        Vector2 input = new Vector2(moveHorizontal, moveVertical);

        // 🟢 SOBRIO
        //if (ebriedad < 0.3f)
        //{
        // normal
        //}
        // 🟡 ENTONADO (zona óptima)
        if (ebriedad < 0.6f)
        {
            velocidadFinal *= 1.2f; // más rápido
        }
        // 🟠 BORRACHO
        else if (ebriedad < 0.8f)
        {
            velocidadFinal *= 1.4f;

            // ruido en el movimiento (pierde precisión)
            input += Random.insideUnitCircle * 0.3f;
        }
        // 🔴 MUY BORRACHO
        else
        {
            velocidadFinal *= 1.6f;

            // control caótico
            input += Random.insideUnitCircle * 0.6f;
        }

        Vector2 movimiento = input.normalized * velocidadFinal;

        rb2D.linearVelocity = movimiento;
    }
}
