using UnityEngine;
using System.Collections;

public class Inventario : MonoBehaviour
{
    public int agua = 0;
    public int comida = 0;

    private Stats stats;

    void Start()
    {
        stats = GetComponent<Stats>();
    }

    public void AgregarAgua(int cantidad)
    {
        agua += cantidad;
    }

    public void AgregarComida(int cantidad)
    {
        comida += cantidad;
    }

    // 💧 AGUA
    public bool UsarAgua()
    {
        if (agua > 0 && stats != null)
        {
            agua--;

            // baja sed y ebriedad (ajustá valores después)
            stats.TomarAgua(40f, 25f);

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

            return true;
        }
        return false;
    }

    IEnumerator BuffComida()
    {
        float original = stats.velocidadBajadaEbriedad;

        // mejora temporal
        stats.velocidadBajadaEbriedad += 3f;

        yield return new WaitForSeconds(10f);

        // vuelve a la normalidad
        stats.velocidadBajadaEbriedad = original;
    }
}