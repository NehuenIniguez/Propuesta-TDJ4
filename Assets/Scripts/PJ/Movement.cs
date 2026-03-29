using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Stats statsPJ;
    public float speed = 5f;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        statsPJ = GetComponent<Stats>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        
    
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (statsPJ.ebriedadActual < 30f)
        {
            rb2D.linearVelocity = new Vector2(moveHorizontal * speed, rb2D.linearVelocity.y);
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, moveVertical * speed);
        } else if (statsPJ.ebriedadActual >= 30f && statsPJ.ebriedadActual < 70f)
        {
            moveHorizontal *= 0.75f; // Reduce horizontal speed by 25% when ebriedad is between 30 and 70
            moveVertical *= 0.75f;   // Reduce vertical speed by 25% when ebriedad is between 30 and 70
            rb2D.linearVelocity = new Vector2(moveHorizontal * speed, rb2D.linearVelocity.y);
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, moveVertical * speed);
        }
        else if (statsPJ.ebriedadActual >= 70f)
        {
            moveHorizontal *= 0.5f; // Reduce horizontal speed by 50% when ebriedad > 0
            moveVertical *= 0.5f;   // Reduce vertical speed by 50% when ebriedad > 0
            rb2D.linearVelocity = new Vector2(moveHorizontal * speed * -1, rb2D.linearVelocity.y);
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, moveVertical * speed * -1);
        }
    }
}
