using UnityEngine;
using UnityEngine.UI;

public class RepararObjeto : MonoBehaviour
{
    public float tiempoReparacion = 3f;
    private float progreso = 0f;

    public Slider barraUI;

    private bool jugadorCerca = false;
    private bool reparado = false;

    void Start()
    {
        barraUI.maxValue = tiempoReparacion;
        barraUI.value = 0;
        barraUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!jugadorCerca || reparado) return;

        if (Input.GetKey(KeyCode.E))
        {
            progreso += Time.deltaTime;
            barraUI.value = progreso;

            if (progreso >= tiempoReparacion)
            {
                CompletarReparacion();
            }
        }
        else
        {
            progreso = 0f;
            barraUI.value = 0f;
        }
    }

    void CompletarReparacion()
    {
        reparado = true;
        barraUI.value = tiempoReparacion;

        Debug.Log("Objeto reparado");

        MisionReparar mision = FindObjectOfType<MisionReparar>();
        if (mision != null)
        {
            mision.ObjetoReparado();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            barraUI.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            barraUI.gameObject.SetActive(false);
            progreso = 0f;
            barraUI.value = 0f;
        }
    }
}