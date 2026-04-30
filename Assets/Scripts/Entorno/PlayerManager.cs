using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int dinero;
    public bool tieneFacon;

    // 🤝 Confianza
    public int confianza;

    // 📅 Día
    public int diaActual;

    // 🧺 Inventario
    public int agua;
    public int comida;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 💾 Cargar datos generales
            dinero = PlayerPrefs.GetInt("DineroGaucho", 0);
            tieneFacon = PlayerPrefs.GetInt("TieneFacon", 0) == 1;
            confianza = PlayerPrefs.GetInt("ConfianzaCapataz", 0);

            agua = PlayerPrefs.GetInt("Agua", 0);
            comida = PlayerPrefs.GetInt("Comida", 0);

            // 📅 SISTEMA DE DÍAS (🔥 LO IMPORTANTE)
            string escenaActual = SceneManager.GetActiveScene().name;

            if (escenaActual == "DiaUno") // ⚠️ CAMBIAR si tu escena tiene otro nombre
            {
                diaActual = 1;

                PlayerPrefs.SetInt("DiaActual", diaActual);
                PlayerPrefs.Save();

                Debug.Log("Reiniciando a Día 1");
            }
            else
            {
                diaActual = PlayerPrefs.GetInt("DiaActual", 1);
            }

            Debug.Log("Día actual: " + diaActual);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 📅 AVANZAR DÍA
    public void SiguienteDia()
    {
        diaActual++;

        PlayerPrefs.SetInt("DiaActual", diaActual);
        PlayerPrefs.Save();

        Debug.Log("Nuevo día: " + diaActual);
    }

    // 💰 DINERO
    public void ModificarDinero(int cantidad)
    {
        dinero += cantidad;

        PlayerPrefs.SetInt("DineroGaucho", dinero);
        PlayerPrefs.Save();

        ActualizarUI();
    }

    // 🤝 CONFIANZA
    public void ModificarConfianza(int cantidad)
    {
        confianza += cantidad;
        confianza = Mathf.Clamp(confianza, 0, 100);

        PlayerPrefs.SetInt("ConfianzaCapataz", confianza);
        PlayerPrefs.Save();
    }

    // 🧺 INVENTARIO
    public void ModificarAgua(int cantidad)
    {
        agua += cantidad;
        agua = Mathf.Max(0, agua);

        PlayerPrefs.SetInt("Agua", agua);
        PlayerPrefs.Save();
    }

    public void ModificarComida(int cantidad)
    {
        comida += cantidad;
        comida = Mathf.Max(0, comida);

        PlayerPrefs.SetInt("Comida", comida);
        PlayerPrefs.Save();
    }

    void ActualizarUI()
    {
        Stats stats = FindObjectOfType<Stats>();
        if (stats != null)
        {
            stats.dineroActual = dinero;
            stats.ActualizarTextoDinero();
        }
    }
}