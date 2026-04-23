using UnityEngine;

public class PulperoTienda : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelTienda;

    [Header("Precios")]
    public int precioAgua = 10;
    public int precioComida = 25;
    public int precioFacon = 500;

    private bool jugadorCerca = false;

    private Stats statsJugador;
    private Inventario inventarioJugador;

    void Start()
    {
        panelTienda.SetActive(false);

        // Buscar jugador directamente (NO depender del trigger)
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            statsJugador = jugador.GetComponent<Stats>();
            inventarioJugador = jugador.GetComponent<Inventario>();
        }
        else
        {
            Debug.LogError("No se encontró el jugador con tag 'Player'");
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.P))
        {
            if (!panelTienda.activeSelf)
                AbrirTienda();
            else
                CerrarTienda();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            CerrarTienda();
        }
    }

    void AbrirTienda()
    {
        panelTienda.SetActive(true);
        Time.timeScale = 0f;
    }

    void CerrarTienda()
    {
        panelTienda.SetActive(false);
        Time.timeScale = 1f;
    }

    // 🛒 COMPRAS

    public void ComprarAgua()
    {
        Debug.Log("Intentando comprar agua");

        if (PlayerManager.instance == null)
        {
            Debug.LogError("PlayerManager es NULL");
            return;
        }

        if (PlayerManager.instance.dinero >= precioAgua)
        {
            PlayerManager.instance.ModificarDinero(-precioAgua);

            if (inventarioJugador != null)
            {
                inventarioJugador.AgregarAgua(1);
                Debug.Log("Agua agregada al inventario");
            }

            ActualizarUI();
        }
        else
        {
            Debug.Log("No alcanza el dinero");
        }
    }

    public void ComprarComida()
    {
        Debug.Log("Intentando comprar comida");

        if (PlayerManager.instance == null)
        {
            Debug.LogError("PlayerManager es NULL");
            return;
        }

        if (PlayerManager.instance.dinero >= precioComida)
        {
            PlayerManager.instance.ModificarDinero(-precioComida);

            if (inventarioJugador != null)
            {
                inventarioJugador.AgregarComida(1);
                Debug.Log("Comida agregada al inventario");
            }

            ActualizarUI();
        }
        else
        {
            Debug.Log("No alcanza el dinero");
        }
    }
    public void ComprarFacon()
    {
        if (PlayerManager.instance.dinero >= precioFacon &&
            !PlayerManager.instance.tieneFacon &&
            PlayerManager.instance.confianza >= 50)
        {
            PlayerManager.instance.ModificarDinero(-precioFacon);
            PlayerManager.instance.tieneFacon = true;
    
            PlayerPrefs.SetInt("TieneFacon", 1);
            PlayerPrefs.Save();
    
            ActualizarUI();
    
            Debug.Log("Compraste el facón");
        }
        else
        {
            Debug.Log("No cumplís los requisitos (dinero/confianza)");
        }
    }


    void ActualizarUI()
    {
        if (statsJugador != null)
        {
            statsJugador.ActualizarTextoDinero();
        }
    }
}
