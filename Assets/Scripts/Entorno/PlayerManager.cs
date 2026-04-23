using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public static PlayerManager instance;

    public int dinero;
    public bool tieneFacon;

    // 🤝 NUEVO
    public int confianza;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 💾 Cargar datos
            dinero = PlayerPrefs.GetInt("DineroGaucho", 0);
            tieneFacon = PlayerPrefs.GetInt("TieneFacon", 0) == 1;
            confianza = PlayerPrefs.GetInt("ConfianzaCapataz", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ModificarDinero(int cantidad)
    {
        dinero += cantidad;

        PlayerPrefs.SetInt("DineroGaucho", dinero);
        PlayerPrefs.Save();

        ActualizarUI();
    }

    public void ModificarConfianza(int cantidad)
    {
        confianza += cantidad;

        // opcional: limitar valores
        confianza = Mathf.Clamp(confianza, 0, 100);

        PlayerPrefs.SetInt("ConfianzaCapataz", confianza);
        PlayerPrefs.Save();

        Debug.Log("Confianza actual: " + confianza);
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
