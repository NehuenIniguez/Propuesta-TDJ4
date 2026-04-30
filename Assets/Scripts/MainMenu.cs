using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public FadeController fade; // 👈 referencia al fade

    void Update()
    {
        if (Input.anyKeyDown)
        {
            fade.FadeOut("DiaUno"); // 👈 usamos fade en vez de cargar directo
        }
    }
}