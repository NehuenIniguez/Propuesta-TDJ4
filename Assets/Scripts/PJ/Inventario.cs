using UnityEngine;

public class Inventario : MonoBehaviour
{
    public int agua = 0;
    public int comida = 0;

    public void AgregarAgua(int cantidad)
    {
        agua += cantidad;
    }

    public void AgregarComida(int cantidad)
    {
        comida += cantidad;
    }

    public bool UsarAgua(Stats stats)
    {
        if (agua > 0)
        {
            agua--;
            stats.TomarAgua(40f);
            return true;
        }
        return false;
    }

    public bool UsarComida(Stats stats)
    {
        if (comida > 0)
        {
            comida--;

            // efecto simple por ahora
            stats.velocidadBajadaEbriedad += 2f;

            return true;
        }
        return false;
    }
}
