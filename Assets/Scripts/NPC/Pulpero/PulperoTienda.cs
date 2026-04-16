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

    void Start()
    {
        panelTienda.SetActive(false);
    }

    void Update()
    {
        // Abrir tienda con P
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
            statsJugador = other.GetComponent<Stats>();
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
        Time.timeScale = 0f; // pausa el juego
    }

    void CerrarTienda()
    {
        panelTienda.SetActive(false);
        Time.timeScale = 1f;
    }

    // 🛒 BOTONES DE COMPRA

    public void ComprarAgua()
    {
        if (PlayerManager.instance.dinero >= precioAgua)
        {
            PlayerManager.instance.ModificarDinero(-precioAgua);

            if (statsJugador != null)
                statsJugador.TomarAlcohol(0f, 40f); // baja sed sin ebriedad

            ActualizarUI();
        }
    }

   public void ComprarComida()
    {
        if (PlayerManager.instance.dinero >= precioComida)
        {
            PlayerManager.instance.ModificarDinero(-precioComida);

            Inventario inv = statsJugador.GetComponent<Inventario>();

            if (inv != null)
            {
                inv.AgregarComida(1);
            }

            ActualizarUI();
        }
    }

   

    void ActualizarUI()
    {
        if (statsJugador != null)
        {
            statsJugador.ModificarDinero(0); // solo refresca el texto
        }
    }
}
