using UnityEngine;

public class MisionMaleza : MisionBase
{
    [Header("Objetivo")]
    public int malezasObjetivo = 5;

    private int malezasActuales = 0;
    private bool completada = false;

    public override void IniciarMision()
    {
        malezasActuales = 0;
        completada = false;

        Debug.Log("Limpiá el campo: cortá " + malezasObjetivo + " malezas");
    }

    public void MalezaEliminada()
    {
        if (completada) return;

        malezasActuales++;

        Debug.Log("Maleza cortada: " + malezasActuales + "/" + malezasObjetivo);

        if (malezasActuales >= malezasObjetivo)
        {
            completada = true;
            Debug.Log("Misión de maleza completada");
        }
    }

    public override bool EstaCompletada()
    {
        return completada;
    }

    public override void FinalizarMision()
    {
        Debug.Log("Fin de misión maleza");
    }
}