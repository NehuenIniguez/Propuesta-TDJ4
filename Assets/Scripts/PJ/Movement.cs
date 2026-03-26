using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float speed = 5f;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb2D.linearVelocity = new Vector2(moveHorizontal * speed, rb2D.linearVelocity.y);
        float moveVertical = Input.GetAxis("Vertical");
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, moveVertical * speed);
    }
}
