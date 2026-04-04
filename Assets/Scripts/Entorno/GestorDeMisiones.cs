using System.Collections;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestorDeMisiones : MonoBehaviour
{
    // --- ESTADOS DE LA MISIÓN ---
    public enum EstadoMision { NoEmpezada, EnProgreso, EsperandoPago, Finalizada }
    public EstadoMision estadoActual = EstadoMision.NoEmpezada;

    [Header("Referencias de Objetos")]
    public GameObject vaca;            // Objeto de la vaca en la escena
    public GameObject zonaCorral;      // Objeto que detecta la vaca en el corral
    public TextMeshProUGUI textoMisionUI;         // Opcional: Para mostrar la tarea en pantalla

    [Header("Sistema de Diálogos")]
    public GameObject panelDialogo;    // El objeto que vas a activar/desactivar
    public TextMeshProUGUI textoDialogo; // El texto que está DENTRO del panel
    public float tiempoVisible = 4f;   // Cuánto tiempo dura el mensaje antes de irse
    public float velocidadEscritura = 0.03f; // Tiempo entre cada letra
    public float tiempoEsperaFinal = 3f;    // Cuánto se queda el texto al terminar de escribir
    private Coroutine corrutinaEscritura;

    [Header("Economía")]
    public int pagoPorMision = 100;
    private Stats statsJugador; // Referencia al script de Stats
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
            statsJugador = jugador.GetComponent<Stats>();
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
        
        Conversacion("¡Eh, Tucumano! Necesito que lleves esa vaca de allá al corral. ¡Apurate!");
        ActualizarTextoUI("Lleva la vaca al corral");
    }

    void RecibirPago()
    {
        estadoActual = EstadoMision.Finalizada;
        // La vaca desaparece (entró al corral y ya no la vemos)
        vaca.SetActive(false); 
        
        // --- LÓGICA DE PAGO ---
        if (statsJugador != null)
        {
            // Llamamos a la función de Stats para sumar dinero y actualizar la UI
            statsJugador.ModificarDinero(pagoPorMision);
        }

        Conversacion("¡Excelente trabajo! Aquí tenés tus $" + pagoPorMision + ". ¡Andá a la Pulperia!");
        ActualizarTextoUI("Misión Completada");

        // Esperamos un poco y cambiamos de escena
        Invoke("IrALaPulperia", tiempoEsperaFinal + 1f);

        
    }

    // --- DETECCIÓN DE TAREA COMPLETADA (Se llama desde el Corral) ---

    public void VacaLlegoAlCorral()
    {
        if (estadoActual == EstadoMision.EnProgreso)
        {
            estadoActual = EstadoMision.EsperandoPago;
          
            ActualizarTextoUI("Vuelve con el Capataz por tu paga");
        }
    }

    // --- FUNCIONES AUXILIARES DE FEEDBACK ---

    void Conversacion(string mensaje)
    {
        // Si ya se está escribiendo algo, lo detenemos para empezar el nuevo
        if (corrutinaEscritura != null)
        {
            StopCoroutine(corrutinaEscritura);
        }
        
        corrutinaEscritura = StartCoroutine(EscribirMensaje(mensaje));
    }
    IEnumerator EscribirMensaje(string mensaje)
    {
        // 1. Limpiamos el texto y activamos el panel
        textoDialogo.text = "";
        panelDialogo.SetActive(true);

        // 2. Recorremos el mensaje letra por letra
        foreach (char letra in mensaje.ToCharArray())
        {
            textoDialogo.text += letra;
            // Esperamos un poquito antes de la siguiente letra
            yield return new WaitForSeconds(velocidadEscritura);
        }

        // 3. Una vez terminado, esperamos un tiempo para que el jugador lea
        yield return new WaitForSeconds(tiempoEsperaFinal);

        // 4. Desaparece el cartel
        panelDialogo.SetActive(false);
    }
    void ActualizarTextoUI(string mensaje)
    {
        if (textoMisionUI != null)
        {
            textoMisionUI.text = mensaje;
        }
    }
    void IrALaPulperia()
    {
        SceneManager.LoadScene("Pulperia");
    }
}
