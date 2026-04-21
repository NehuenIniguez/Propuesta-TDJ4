using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("EBRIEDAD")]
    public Slider ebriedadSlider;
    public float ebriedadActual = 0f;
    public float maxEbriedad = 100f;
    public float velocidadBajadaEbriedad = 5f;

    [Header("SED")]
    public Slider sedSlider;
    public float sedActual = 0f;
    public float maxSed = 100f;
    public float velocidadSubidaSed = 1f;

    [Header("Dinero")]
    public int dineroActual = 0;
    public TextMeshProUGUI textoDineroUI;

    void Start()
    {
        dineroActual = PlayerManager.instance.dinero;
        ActualizarTextoDinero();

        ebriedadActual = 0f;
        ebriedadSlider.maxValue = maxEbriedad;

        sedActual = 0f;
        sedSlider.maxValue = maxSed;

        ActualizarUI();
    }

    void Update()
    {
        if (ebriedadActual > 0)
        {
            ebriedadActual -= velocidadBajadaEbriedad * Time.deltaTime;
            ebriedadActual = Mathf.Clamp(ebriedadActual, 0, maxEbriedad);
        }

        if (sedActual < maxSed)
        {
            sedActual += velocidadSubidaSed * Time.deltaTime;
            sedActual = Mathf.Clamp(sedActual, 0, maxSed);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TomarAlcohol(20f, 30f);
        }

        if (sedActual >= maxSed)
        {
            Time.timeScale = 0f;
        }
        Inventario inv = GetComponent<Inventario>();

    if (inv != null)
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            inv.UsarAgua();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            inv.UsarComida();
        }
    }
            ActualizarUI();
    }  
    

    void ActualizarUI()
    {
        ebriedadSlider.value = ebriedadActual;
        sedSlider.value = sedActual;
    }

    public void TomarAlcohol(float aumentoEbriedad, float reduccionSed)
    {
        ebriedadActual += aumentoEbriedad;
        sedActual -= reduccionSed;

        ebriedadActual = Mathf.Clamp(ebriedadActual, 0, maxEbriedad);
        sedActual = Mathf.Clamp(sedActual, 0, maxSed);

        ActualizarUI();
    }

    public void TomarAgua(float reduccionSed, float reduccionEbriedad)
    {
        sedActual -= reduccionSed;
        ebriedadActual -= reduccionEbriedad;

        sedActual = Mathf.Clamp(sedActual, 0, maxSed);
        ebriedadActual = Mathf.Clamp(ebriedadActual, 0, maxEbriedad);

        ActualizarUI();
    }

    public void ActualizarTextoDinero()
    {
        if (textoDineroUI != null)
        {
            textoDineroUI.text = dineroActual.ToString();
        }
    }

    public void ModificarDinero(int cantidad)
    {
        PlayerManager.instance.ModificarDinero(cantidad);

        dineroActual = PlayerManager.instance.dinero;
        ActualizarTextoDinero();
    }

    public float GetPorcentajeEbriedad()
    {
        return ebriedadActual / maxEbriedad;
    }
    
}
