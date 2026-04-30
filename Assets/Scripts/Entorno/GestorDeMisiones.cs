using System.Collections;
using System.Collections.Generic;
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
    private List<MisionBase> misionesRestantes;

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
    private int misionesPorDia;
    private int misionesCompletadas = 0;

    private Coroutine corrutinaEscritura;

    public PatronFinal patron;

    void Start()
    {
        int diaActual = PlayerManager.instance.diaActual;

        // 🔥 Día 2 tiene 3 misiones
        if (diaActual == 2)
            misionesPorDia = 3;
        else
            misionesPorDia = Mathf.Clamp(diaActual, 1, 5);

        misionesCompletadas = 0;

        // 🔥 lista para random sin repetir
        misionesRestantes = new List<MisionBase>(misionesDisponibles);

        ActualizarTextoUI("Día " + diaActual + " - Habla con el Capataz");

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
            statsJugador = jugador.GetComponent<Stats>();
    }

    void Update()
    {
        if (misionActiva)
        {
            tiempoActual -= Time.deltaTime;

            if (tiempoActual <= 0f)
                PerderMision("¡Fallaste!");

            if (misionActual != null && misionActual.EstaCompletada())
            {
                estadoActual = EstadoMision.EsperandoPago;
                misionActiva = false;

                ActualizarTextoUI("Volvé con el Capataz");
            }
        }

        if (textoTimerUI != null)
        {
            textoTimerUI.gameObject.SetActive(misionActiva);

            if (misionActiva)
                textoTimerUI.text = "Tiempo: " + Mathf.CeilToInt(tiempoActual);
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

        if (misionesRestantes.Count == 0)
        {
            Debug.Log("No quedan más misiones");
            return;
        }

        int index = Random.Range(0, misionesRestantes.Count);
        misionActual = misionesRestantes[index];
        misionesRestantes.RemoveAt(index);

        misionActual.IniciarMision();

        tiempoActual = tiempoLimite;
        misionActiva = true;

        ActualizarTextoUI(misionActual.descripcion);
    }

    void RecibirPago()
    {
        if (misionActual != null)
            misionActual.FinalizarMision();

        int pagoFinal = pagoPorMision;

        if (statsJugador != null)
        {
            statsJugador.ModificarDinero(pagoFinal);
            PlayerManager.instance.ModificarConfianza(10);
        }

        Conversacion("Cobrás $" + pagoFinal);

        misionesCompletadas++;

        if (misionesCompletadas >= misionesPorDia)
            FinDelDia();
        else
        {
            estadoActual = EstadoMision.NoEmpezada;
            ActualizarTextoUI("Otra más...");
        }
    }

    // 🔥 FINAL DEL DÍA
    void FinDelDia()
    {
        int diaActual = PlayerManager.instance.diaActual;

        if (diaActual == 2)
        {
            StartCoroutine(EventoFinal());
            return;
        }

        PlayerManager.instance.SiguienteDia();

        Conversacion("Buen trabajo. Andá a la pulpería.");
        Invoke("IrALaPulperia", 2f);
    }

    // 🎬 EVENTO FINAL NARRATIVO
    IEnumerator EventoFinal()
    {
        yield return StartCoroutine(EscribirMensaje("(Ya confía en mí...)"));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EscribirMensaje("(No sospecha nada.)"));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EscribirMensaje("(Es hora de recuperar lo mío.)"));
        yield return new WaitForSeconds(1.5f);

        ActivarEventoPatron();
    }

    void ActivarEventoPatron()
{
    Debug.Log("🔥 COMBATE FINAL ACTIVADO");

    if (patron != null)
    {
        patron.vulnerable = true;
    }
}

    void PerderMision(string mensaje)
    {
        misionActiva = false;
        estadoActual = EstadoMision.NoEmpezada;

        if (misionActual != null)
            misionActual.FinalizarMision();

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