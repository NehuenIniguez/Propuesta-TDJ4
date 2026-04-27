using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorDeMisiones : MonoBehaviour
{
    public enum EstadoMision { NoEmpezada, EnProgreso, EsperandoPago }
    public EstadoMision estadoActual = EstadoMision.NoEmpezada;

    [Header("Misiones")]
    public MisionBase[] misionesDisponibles;
    private MisionBase misionActual;

    [Header("UI")]
    public TextMeshProUGUI textoMisionUI;
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;

    [Header("Tiempo")]
    public float tiempoLimite = 30f;
    private float tiempoActual;
    private bool misionActiva = false;

    [Header("Timer UI")]
    public TextMeshProUGUI textoTimerUI;

    [Header("Economía")]
    public int pagoPorMision = 100;
    private Stats statsJugador;

    [Header("Progresión")]
    public int diaActual = 1;
    private int misionesPorDia;
    private int misionesCompletadas = 0;

    private Coroutine corrutinaEscritura;

    void Start()
    {
        misionesPorDia = Mathf.Clamp(diaActual, 1, 5);
        misionesCompletadas = 0;

        ActualizarTextoUI("Día " + diaActual + " - Habla con el Capataz");

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            statsJugador = jugador.GetComponent<Stats>();
        }
    }

    void Update()
    {
        if (misionActiva)
        {
            tiempoActual -= Time.deltaTime;

            if (tiempoActual <= 0f)
            {
                PerderMision("¡Fallaste!");
            }

            // 🔥 detectar si la misión se completó
            if (misionActual != null && misionActual.EstaCompletada())
            {
                estadoActual = EstadoMision.EsperandoPago;
                misionActiva = false;

                ActualizarTextoUI("Volvé con el Capataz");
            }
        }

        // ⏱️ UI TIMER
        if (textoTimerUI != null)
        {
            textoTimerUI.gameObject.SetActive(misionActiva);

            if (misionActiva)
            {
                textoTimerUI.text = "Tiempo: " + Mathf.CeilToInt(tiempoActual);
            }
        }
    }

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
                Conversacion("Terminá la tarea primero.");
                break;
        }
    }

    void EmpezarMision()
    {
        estadoActual = EstadoMision.EnProgreso;

        // 🔥 elegimos misión al azar
        misionActual = misionesDisponibles[Random.Range(0, misionesDisponibles.Length)];

        misionActual.IniciarMision();

        tiempoActual = tiempoLimite;
        misionActiva = true;

        ActualizarTextoUI(misionActual.descripcion);
    }

    void RecibirPago()
    {
        if (misionActual != null)
        {
            misionActual.FinalizarMision();
        }

        int pagoFinal = pagoPorMision;

        if (statsJugador != null)
        {
            statsJugador.ModificarDinero(pagoFinal);
            PlayerManager.instance.ModificarConfianza(10);
        }

        Conversacion("Cobrás $" + pagoFinal);

        misionesCompletadas++;

        if (misionesCompletadas >= misionesPorDia)
        {
            FinDelDia();
        }
        else
        {
            estadoActual = EstadoMision.NoEmpezada;
            ActualizarTextoUI("Otra más...");
        }
    }

    void FinDelDia()
    {
        Conversacion("Buen trabajo. Andá a la pulpería.");
        Invoke("IrALaPulperia", 2f);
    }

    void PerderMision(string mensaje)
    {
        misionActiva = false;
        estadoActual = EstadoMision.NoEmpezada;

        if (misionActual != null)
        {
            misionActual.FinalizarMision();
        }

        Conversacion(mensaje);
        ActualizarTextoUI("Fallaste");
    }

    void Conversacion(string mensaje)
    {
        if (corrutinaEscritura != null)
            StopCoroutine(corrutinaEscritura);

        corrutinaEscritura = StartCoroutine(EscribirMensaje(mensaje));
    }

    IEnumerator EscribirMensaje(string mensaje)
    {
        textoDialogo.text = "";
        panelDialogo.SetActive(true);

        foreach (char letra in mensaje)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(2f);

        panelDialogo.SetActive(false);
    }

    void ActualizarTextoUI(string mensaje)
    {
        if (textoMisionUI != null)
            textoMisionUI.text = mensaje;
    }

    void IrALaPulperia()
    {
        SceneManager.LoadScene("Pulperia");
    }
}