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

    [Header("Tiempo de Misión")]
    public float tiempoLimite = 30f;
    private float tiempoActual;
    private bool misionActiva = false;

    [Header("UI Timer")]
    public TextMeshProUGUI textoTimerUI;

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

    void Update()
    {
        if (misionActiva)
        {
            tiempoActual -= Time.deltaTime;

            // ⏱️ Se acabó el tiempo
            if (tiempoActual <= 0f)
            {
                PerderMision("¡Se te fue la vaca, inútil!");
            }

            // 💀 Demasiado borracho = perder
            if (statsJugador != null && statsJugador.GetPorcentajeEbriedad() >= 1f)
            {
                PerderMision("¡Estás demasiado borracho para trabajar!");
            }
        }

        if (textoTimerUI != null)
        {
            textoTimerUI.gameObject.SetActive(misionActiva);
            textoTimerUI.text = "Tiempo: " + Mathf.CeilToInt(tiempoActual);
        
            if (tiempoActual < 10f)
                textoTimerUI.color = Color.red;
            else if (tiempoActual < 20f)
                textoTimerUI.color = Color.yellow;
            else
                textoTimerUI.color = Color.white;
        }

        if (EstadoMision.NoEmpezada == estadoActual)
        {
            if (textoTimerUI != null)
            {
                textoTimerUI.gameObject.SetActive(false);
            }
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
        tiempoActual = tiempoLimite;
        misionActiva = true;
        // Aquí podrías mover la vaca a una posición inicial específica:
        // vaca.transform.position = transform.position + Vector3.right * 2;
        
        Conversacion("¡Eh, Tucumano! Necesito que lleves esa vaca de allá al corral. ¡Apurate!");
        ActualizarTextoUI("Lleva la vaca al corral");
    }

    void RecibirPago()
    {
         estadoActual = EstadoMision.Finalizada;
    misionActiva = false;

    vaca.SetActive(false);

    int pagoFinal = pagoPorMision;
    int confianzaGanada = 10; // base

    if (statsJugador != null)
    {
        float ebriedad = statsJugador.GetPorcentajeEbriedad();

        // 🟠 Borracho → menos pago y menos confianza
        if (ebriedad >= 0.5f && ebriedad < 0.75f)
        {
            pagoFinal = Mathf.RoundToInt(pagoPorMision * 0.7f);
            confianzaGanada = 5;
        }
        // 🔴 Muy borracho → casi nada y casi sin confianza
        else if (ebriedad >= 0.75f)
        {
            pagoFinal = Mathf.RoundToInt(pagoPorMision * 0.3f);
            confianzaGanada = 2;
        }

        // ⏱️ BONUS por rapidez
        float porcentajeTiempo = tiempoActual / tiempoLimite;

        if (porcentajeTiempo > 0.5f)
        {
            confianzaGanada += 5;
        }

        // 💰 dinero
        statsJugador.ModificarDinero(pagoFinal);

        // 🤝 confianza
        PlayerManager.instance.ModificarConfianza(confianzaGanada);
    }

    Conversacion("Trabajo hecho. Cobrás $" + pagoFinal + " | Confianza +" + confianzaGanada);
    ActualizarTextoUI("Misión Completada");

    if (textoTimerUI != null)
    {
        textoTimerUI.text = "";
    }

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
        misionActiva = false;
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
    void PerderMision(string mensaje)
    {
        misionActiva = false;
        estadoActual = EstadoMision.NoEmpezada;

        if (textoTimerUI != null)
        {
            textoTimerUI.text = "";
        }
        
        vaca.SetActive(false);
    
        Conversacion(mensaje);
        ActualizarTextoUI("Fallaste la misión");
        Time.timeScale = 0;
    }
}
