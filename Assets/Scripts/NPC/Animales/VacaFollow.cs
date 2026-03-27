using UnityEngine;

public class VacaFollow : MonoBehaviour
{
    public Transform jugador; // Asigna en el inspector
    public float distanciaSeguir = 3f; // Distancia a mantener
    public float velocidadSeguir = 2f;
    public bool siguiendo = false;
    void Update()
    {
        if (siguiendo)
        {
            float distancia = Vector3.Distance(transform.position, jugador.position);
            if (distancia > distanciaSeguir)
            {
                Vector3 direccion = (jugador.position - transform.position).normalized;
                transform.position += direccion * velocidadSeguir * Time.deltaTime;
            }
        }
    }
    public void ArriarVaca()
    {
        siguiendo = true;
    }
    public void DejarDeArriar()
    {
        siguiendo = false;
    }
}
