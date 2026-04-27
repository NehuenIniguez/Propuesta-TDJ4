using UnityEngine;

public abstract class MisionBase : MonoBehaviour
{
    [TextArea]
    public string descripcion;

    // 👉 se llama cuando arranca la misión
    public abstract void IniciarMision();

    // 👉 devuelve true cuando se completó
    public abstract bool EstaCompletada();

    // 👉 limpieza final
    public abstract void FinalizarMision();
}