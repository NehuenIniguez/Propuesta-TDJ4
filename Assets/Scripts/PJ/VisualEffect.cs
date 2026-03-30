using UnityEngine;
using UnityEngine.UI;

public class VisualEffect : MonoBehaviour
{
    public Image overlay;
    public Stats stats;

    [Header("Configuración")]
    public float intensidadMax = 0.5f; // qué tan oscuro puede llegar
    public float suavizado = 5f;       // qué tan rápido se adapta
    public float velocidadPulso = 3f;  // velocidad del "mareo"
    public float fuerzaPulso = 1f;  // intensidad del pulso

    void Update()
    {
        float intensidad = stats.ebriedadActual / stats.maxEbriedad;

        // 🎯 Alpha base según ebriedad
        float alphaObjetivo = intensidad * intensidadMax;

        // 💀 Pulso (mareo)
        float pulso = Mathf.Sin(Time.time * velocidadPulso) * fuerzaPulso * intensidad;

        alphaObjetivo += pulso;

        // 🎨 Aplicar suavizado
        Color c = overlay.color;
        c.a = Mathf.Lerp(c.a, alphaObjetivo, Time.deltaTime * suavizado);

        overlay.color = c;
    }
}
