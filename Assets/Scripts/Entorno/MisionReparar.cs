using UnityEngine;

public class MisionReparar : MisionBase
{
    [Header("Objetivo")]
    public int objetosObjetivo = 2;

    private int objetosReparados = 0;
    private bool completada = false;

    public override void IniciarMision()
    {
        objetosReparados = 0;
        completada = false;

        Debug.Log("Repará " + objetosObjetivo + " objetos");
    }

    public void ObjetoReparado()
    {
        if (completada) return;

        objetosReparados++;

        Debug.Log("Objetos reparados: " + objetosReparados + "/" + objetosObjetivo);

        if (objetosReparados >= objetosObjetivo)
        {
            completada = true;
            Debug.Log("Misión de reparar completada");
        }
    }

    public override bool EstaCompletada()
    {
        return completada;
    }

    public override void FinalizarMision()
    {
        Debug.Log("Fin de misión reparar");
    }
}
