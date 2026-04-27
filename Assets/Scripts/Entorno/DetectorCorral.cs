using UnityEngine;

public class DetectorCorral : MonoBehaviour
{
   public MisionVaca misionVaca;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vaca"))
        {
            misionVaca.VacaEnCorral();
        }
    }
}
