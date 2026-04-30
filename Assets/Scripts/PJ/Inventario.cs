using UnityEngine;
using System.Collections;

public class Inventario : MonoBehaviour
{
    private Stats stats;

    void Start()
    {
        stats = GetComponent<Stats>();
    }

    // 🔹 SOLO LECTURA (desde PlayerManager)
    public int Agua => PlayerManager.instance.agua;
    public int Comida => PlayerManager.instance.comida;

    // ➕ AGREGAR
    public void AgregarAgua(int cantidad)
    {
        PlayerManager.instance.ModificarAgua(cantidad);
        Debug.Log("Agua actual: " + PlayerManager.instance.agua);
    }

    public void AgregarComida(int cantidad)
    {
        PlayerManager.instance.ModificarComida(cantidad);
        Debug.Log("Comida actual: " + PlayerManager.instance.comida);
    }

    // 💧 USAR AGUA
    public bool UsarAgua()
    {
        if (PlayerManager.instance.agua > 0 && stats != null)
        {
            PlayerManager.instance.ModificarAgua(-1);

            stats.TomarAgua(40f, 25f);

            return true;
        }
        return false;
    }

    // 🍖 USAR COMIDA
    public bool UsarComida()
    {
        if (PlayerManager.instance.comida > 0 && stats != null)
        {
            PlayerManager.instance.ModificarComida(-1);

            StartCoroutine(BuffComida());

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
}