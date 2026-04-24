using System.Collections;
using UnityEngine;
using TMPro;

public class JuegoTaba : MonoBehaviour
{
    [Header("Juego")]
    public int apuesta = 50;

    [Header("UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;

    [Header("Diálogo")]
    public float velocidadEscritura = 0.03f;

    private bool jugadorCerca = false;
    private bool enJuego = false;
    private bool esperandoRespuesta = false;

    private Coroutine corrutinaEscritura;

    void Start()
    {
        panelDialogo.SetActive(false);
    }

    void Update()
    {
        if (!jugadorCerca) return;

        // 👉 Iniciar interacción
        if (Input.GetKeyDown(KeyCode.T) && !enJuego)
        {
            enJuego = true;
            esperandoRespuesta = true;

            Conversacion("¿Querés jugar a la taba? (Y = Sí / N = No)");
        }

        // 👉 SOLO responde si está esperando respuesta
        if (esperandoRespuesta)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                esperandoRespuesta = false;
                Jugar();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                esperandoRespuesta = false;
                Conversacion("Dale, Cagón");
                CerrarDialogo();
            }
        }
    }

    void Jugar()
    {
        if (PlayerManager.instance.dinero < apuesta)
        {
            Conversacion("No tenés plata chango");
            CerrarDialogo();
            return;
        }

        bool gana = Random.value > 0.5f;

        if (gana)
        {
            PlayerManager.instance.ModificarDinero(apuesta);
            Conversacion("Que ocote, ganaste $" + apuesta);
        }
        else
        {
            PlayerManager.instance.ModificarDinero(-apuesta);
            Conversacion("Perdiste $" + apuesta);
        }

        CerrarDialogo();
    }

    void CerrarDialogo()
    {
        enJuego = false;
        Invoke(nameof(OcultarPanel), 1.5f); // pequeño delay para leer
    }

    void OcultarPanel()
    {
        panelDialogo.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Conversacion("Apretá T para jugar a la taba");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            enJuego = false;
            esperandoRespuesta = false;
            panelDialogo.SetActive(false);
        }
    }

    // 🧠 DIÁLOGO

    void Conversacion(string mensaje)
    {
        if (corrutinaEscritura != null)
        {
            StopCoroutine(corrutinaEscritura);
        }

        corrutinaEscritura = StartCoroutine(EscribirMensaje(mensaje));
    }

    IEnumerator EscribirMensaje(string mensaje)
    {
        textoDialogo.text = "";
        panelDialogo.SetActive(true);

        foreach (char letra in mensaje.ToCharArray())
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }
}