using UnityEngine;
using TMPro;

public class TextoSuave : MonoBehaviour
{
    private TextMeshProUGUI texto;

    public float velocidad = 2f;

    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * velocidad, 1f);

        // Interpola entre blanco y gris
        texto.color = Color.Lerp(Color.white, Color.gray, t);
    }
}