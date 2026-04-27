using UnityEngine;

public class MisionVaca : MisionBase
{
    public GameObject vaca;

    private bool completada = false;

    public override void IniciarMision()
    {
        completada = false;
        vaca.SetActive(true);
    }

    public void VacaEnCorral()
    {
        completada = true;
    }

    public override bool EstaCompletada()
    {
        return completada;
    }

    public override void FinalizarMision()
    {
        vaca.SetActive(false);
    }
}