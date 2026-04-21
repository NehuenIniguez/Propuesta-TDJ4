using UnityEngine;
using System.Collections;
using TMPro;

public class Inventario : MonoBehaviour
{
    public int agua = 0;
    public int comida = 0;

    private Stats stats;

    [Header("UI")]
    public TextMeshProUGUI textoAgua;
    public TextMeshProUGUI textoComida;

    void Start()
    {
        stats = GetComponent<Stats>();
        ActualizarUI(); // 🔥 importante al iniciar
    }

    public void AgregarAgua(int cantidad)
    {
        agua += cantidad;
        ActualizarUI();
    }

    public void AgregarComida(int cantidad)
    {
        comida += cantidad;
        ActualizarUI();
    }

    // 💧 AGUA
    public bool UsarAgua()
    {
        if (agua > 0 && stats != null)
        {
            agua--;

            stats.TomarAgua(40f, 25f);

            ActualizarUI();
            return true;
        }
        return false;
    }

    // 🍖 COMIDA (buff temporal)
    public bool UsarComida()
    {
        if (comida > 0 && stats != null)
        {
            comida--;

            StartCoroutine(BuffComida());

            ActualizarUI();
            return true;
        }
        return false;
    }

    IEnumerator BuffComida()
    {
        float original = stats.velocidadBajadaEbriedad;

        stats.velocidadBajadaEbriedad += 3f;

        yield return new WaitForSeconds(10f);

        stats.velocidadBajadaEbriedad = original;
    }

    // 🧠 UI
    void ActualizarUI()
    {
        if (textoAgua != null)
        {
            textoAgua.text = "Agua: " + agua;
        }

        if (textoComida != null)
        {
            textoComida.text = "Comida: " + comida;
        }
    }
}