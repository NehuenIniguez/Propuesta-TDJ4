using Unity.VisualScripting;
using UnityEngine;

public class ArriarVaca : MonoBehaviour
{
    private GameObject vaca;
    void Start()
    {
        vaca = GameObject.FindGameObjectWithTag("Vaca");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vaca"))
        {
            VacaFollow vacaFollow = vaca.GetComponent<VacaFollow>();
            if ( Input.GetKeyDown(KeyCode.U) || vacaFollow.siguiendo == false )
            {
                vacaFollow.ArriarVaca();
            }
            else if ( Input.GetKeyDown(KeyCode.U) || vacaFollow.siguiendo == true )
            {
                vacaFollow.DejarDeArriar();
            }
        }
    }
}
