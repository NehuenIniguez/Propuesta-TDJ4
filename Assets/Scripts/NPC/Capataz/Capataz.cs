using UnityEngine;

public class Capataz : MonoBehaviour
{
   private bool jugadorCerca = false;
    private GestorDeMisiones gestorMisiones;

    void Start()
    {
        // Busca el GestorDeMisiones en la escena automáticamente
        gestorMisiones = FindObjectOfType<GestorDeMisiones>();
    }

    void Update()
    {
        // Si el jugador está cerca y presiona la tecla de interacción
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            gestorMisiones.InteractuarConCapataz();
        }
    }

    // Detectar si el Gaucho entra en la zona (Collider Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Presiona 'E' para hablar");
            // Aquí podrías mostrar un icono de '!' sobre el capataz
        }
    }

    // Detectar si el Gaucho se aleja
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}
