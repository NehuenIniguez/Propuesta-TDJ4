using UnityEngine;

public class DetectorCorral : MonoBehaviour
{
    private GestorDeMisiones gestorMisiones;

    void Start()
    {
        gestorMisiones = FindObjectOfType<GestorDeMisiones>();
    }

    // Detectar si LA VACA entra en el corral
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vaca")) // Asegúrate de darle el Tag "Vaca" a tu objeto vaca
        {
            Debug.Log("La vaca entró al corral.");
            gestorMisiones.VacaLlegoAlCorral();
        }
    }
}
