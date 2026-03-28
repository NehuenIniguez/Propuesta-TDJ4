using UnityEngine;

public class CerrarCerca : MonoBehaviour
{
    public GameObject cerca;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Vaca")) {
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            cerca.SetActive(true);
        }
    }
}
