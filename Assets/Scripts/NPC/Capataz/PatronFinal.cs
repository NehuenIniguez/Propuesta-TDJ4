using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatronFinal : MonoBehaviour
{
    public bool vulnerable = false;

    public void RecibirDaño(int daño)
    {
        if (!vulnerable) return;

        Morir();
    }

    void Morir()
    {
        Debug.Log("💀 Mataste al patrón");

        // Podés poner animación acá
        gameObject.SetActive(false);
        StartCoroutine(PasoFinal());
        
    }

    IEnumerator PasoFinal()
    {
        yield return new WaitForSeconds(1f);
       SceneManager.LoadScene("Final");
    }
}