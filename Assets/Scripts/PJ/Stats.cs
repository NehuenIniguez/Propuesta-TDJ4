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

    void Start()
    {
        // Ebriedad
        ebriedadActual = 0f;
        ebriedadSlider.maxValue = maxEbriedad;
        ebriedadSlider.value = ebriedadActual;

        // Sed
        sedActual = 0f;
        sedSlider.maxValue = maxSed;
        sedSlider.value = sedActual;
    }

    void Update()
    {
        // 🍺 Ebriedad baja sola
        if (ebriedadActual > 0)
        {
            ebriedadActual -= velocidadBajadaEbriedad * Time.deltaTime;
            ebriedadActual = Mathf.Clamp(ebriedadActual, 0, maxEbriedad);
            ebriedadSlider.value = ebriedadActual;
        }

        // 💧 Sed sube sola
        if (sedActual < maxSed)
        {
            sedActual += velocidadSubidaSed * Time.deltaTime;
            sedActual = Mathf.Clamp(sedActual, 0, maxSed);
            sedSlider.value = sedActual;
        }

        // 🔧 TEST
        if (Input.GetKeyDown(KeyCode.E))
        {
            TomarAlcohol(20f, 30f);
        }
    }

    // 🍺 Alcohol
    public void TomarAlcohol(float aumentoEbriedad, float reduccionSed)
    {
        ebriedadActual += aumentoEbriedad;
        sedActual -= reduccionSed;

        ebriedadActual = Mathf.Clamp(ebriedadActual, 0, maxEbriedad);
        sedActual = Mathf.Clamp(sedActual, 0, maxSed);

        ebriedadSlider.value = ebriedadActual;
        sedSlider.value = sedActual;
    }

    // 💧 Agua
    
}
