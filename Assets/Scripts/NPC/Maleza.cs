using UnityEngine;

public class Maleza : MonoBehaviour
{
    public int vida = 2;

    public void RecibirGolpe()
    {
        vida--;

        if (vida <= 0)
        {
            DestruirMaleza();
        }
    }

    void DestruirMaleza()
    {
        // 🔔 Avisar a la misión (si existe)
        MisionMaleza mision = FindObjectOfType<MisionMaleza>();
        if (mision != null)
        {
            mision.MalezaEliminada();
        }

        // 💀 Desactivar objeto
        gameObject.SetActive(false);
    }
}