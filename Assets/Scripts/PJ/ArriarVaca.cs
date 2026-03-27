using System;
using Unity.VisualScripting;
using UnityEngine;

public class ArriarVaca : MonoBehaviour
{
    private VacaFollow vacaFollow;
    public float radioDeteccion = 3f; // Radio para detectar vacas cercanas

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (vacaFollow != null && vacaFollow.siguiendo)
            {
                // Si ya hay una vaca siguiendo, detenerla
                vacaFollow.DejarDeArriar();
                vacaFollow = null;
                Debug.Log("Vaca dejó de seguir.");
            }
            else
            {
                // Buscar la vaca más cercana
                VacaFollow vacaCercana = EncontrarVacaMasCercana();
                if (vacaCercana != null)
                {
                    vacaFollow = vacaCercana;
                    vacaFollow.ArriarVaca();
                    Debug.Log("Vaca empezó a seguir.");
                }
                else
                {
                    Debug.Log("No hay vacas cercanas.");
                }
            }
        }
    }
    private void OnDrawGizmos() 
  {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, radioDeteccion);    
  }
    private VacaFollow EncontrarVacaMasCercana()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radioDeteccion);
        VacaFollow masCercana = null;
        float distanciaMinima = Mathf.Infinity;
        

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Vaca"))
            {
                VacaFollow vf = hit.GetComponent<VacaFollow>();
                if (vf != null)
                {
                    float distancia = Vector2.Distance(transform.position, hit.transform.position);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        masCercana = vf;
                    }
                }
            }
        }
        return masCercana;
    }
}
