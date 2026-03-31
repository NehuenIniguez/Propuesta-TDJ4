using UnityEngine;
using UnityEngine.UI;

public class GestorDeMisiones : MonoBehaviour
{
   // --- ESTADOS DE LA MISIÓN ---
    public enum EstadoMision { NoEmpezada, EnProgreso, EsperandoPago, Finalizada }
    public EstadoMision estadoActual = EstadoMision.NoEmpezada;

    // --- REFERENCIAS A OBJETOS ---
    public GameObject vaca;            // Objeto de la vaca en la escena
    public GameObject zonaCorral;      // Objeto que detecta la vaca en el corral
    public Text textoMisionUI;         // Opcional: Para mostrar la tarea en pantalla

    // --- REFERENCIA AL GAUCHO (JUGADOR) ---
    private Movement movimientoJugador; // Reemplaza 'PlayerMovement' por el nombre de TU script de movimiento

    void Start()
    {
        // Al inicio, la vaca está quieta o desactivada, como prefieras
        vaca.SetActive(false); 
        ActualizarTextoUI("Habla con el Capataz");
        
        // Busca el script de movimiento del jugador por tag para pausarlo luego
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            movimientoJugador = jugador.GetComponent<Movement>();
        }
    }

    // --- MÉTODOS PARA EL CAPATAZ (Se llaman desde su propio script) ---

    public void InteractuarConCapataz()
    {
        switch (estadoActual)
        {
            case EstadoMision.NoEmpezada:
                EmpezarMision();
                break;
            case EstadoMision.EsperandoPago:
                RecibirPago();
                break;
            case EstadoMision.EnProgreso:
                // El gaucho volvió a hablar antes de terminar la tarea
                Conversacion("¿Ya trajiste la vaca? ¡No pierdas tiempo, che!");
                break;
            case EstadoMision.Finalizada:
                Conversacion("Buen trabajo hoy. Mañana habrá más.");
                break;
        }
    }

    void EmpezarMision()
    {
        estadoActual = EstadoMision.EnProgreso;
        vaca.SetActive(true); // Aparece la vaca
        // Aquí podrías mover la vaca a una posición inicial específica:
        // vaca.transform.position = transform.position + Vector3.right * 2;
        
        Conversacion("¡Ey gaucho! Necesito que lleves esa vaca de allá al corral. ¡Apurate!");
        ActualizarTextoUI("Lleva la vaca al corral");
    }

    void RecibirPago()
    {
        estadoActual = EstadoMision.Finalizada;
        // La vaca desaparece (entró al corral y ya no la vemos)
        vaca.SetActive(false); 
        
        // --- LÓGICA DE PAGO ---
        // Aquí sumarías las monedas/pesos a la variable de dinero del gaucho
        // Ejemplo: GauchoStats.dinero += 100;

        Conversacion("¡Excelente trabajo! Aquí tienes tu paga. ¡A disfrutar la noche en la taberna!");
        ActualizarTextoUI("Misión Completada - Ve a la Taberna");
    }

    // --- DETECCIÓN DE TAREA COMPLETADA (Se llama desde el Corral) ---

    public void VacaLlegoAlCorral()
    {
        if (estadoActual == EstadoMision.EnProgreso)
        {
            estadoActual = EstadoMision.EsperandoPago;
            // Podrías detener el seguimiento de la vaca al gaucho aquí si lo tienes implementado
            Conversacion("¡Listo! La vaca está adentro. Vuelve a hablar con el Capataz para cobrar.");
            ActualizarTextoUI("Vuelve con el Capataz por tu paga");
        }
    }

    // --- FUNCIONES AUXILIARES DE FEEDBACK ---

    void Conversacion(string mensaje)
    {
        // Aquí podrías implementar tu sistema de diálogos completo.
        // Por ahora, lo mostramos en la consola de Unity.
        Debug.Log("CAPATAZ: " + mensaje);
    }

    void ActualizarTextoUI(string mensaje)
    {
        if (textoMisionUI != null)
        {
            textoMisionUI.text = mensaje;
        }
    }
}
