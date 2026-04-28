using UnityEngine;

public class MisionCombate : MisionBase
{
    [Header("Objetivo")]
    public int enemigosObjetivo = 3;

    private int enemigosActuales = 0;
    private bool completada = false;

    public override void IniciarMision()
    {
        enemigosActuales = 0;
        completada = false;

        Debug.Log("Derrotá " + enemigosObjetivo + " enemigos");
    }

    public void EnemigoDerrotado()
    {
        if (completada) return;

        enemigosActuales++;

        Debug.Log("Enemigos derrotados: " + enemigosActuales + "/" + enemigosObjetivo);

        if (enemigosActuales >= enemigosObjetivo)
        {
            completada = true;
            Debug.Log("Misión de combate completada");
        }
    }

    public override bool EstaCompletada()
    {
        return completada;
    }

    public override void FinalizarMision()
    {
        Debug.Log("Fin de misión combate");
    }
}