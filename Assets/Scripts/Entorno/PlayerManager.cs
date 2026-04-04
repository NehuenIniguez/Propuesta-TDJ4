using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public int dinero;
    public bool tieneFacon;

    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Cargar datos guardados
            dinero = PlayerPrefs.GetInt("DineroGaucho", 0);
            tieneFacon = PlayerPrefs.GetInt("TieneFacon", 0) == 1;
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
    }

    public void ComprarFacon(int precio)
    {
        if (dinero >= precio && !tieneFacon)
        {
            dinero -= precio;
            tieneFacon = true;

            PlayerPrefs.SetInt("DineroGaucho", dinero);
            PlayerPrefs.SetInt("TieneFacon", 1);
            PlayerPrefs.Save();
        }
    }

}
