using UnityEngine;
using TMPro;

public class ContarPasos : MonoBehaviour
{
    private int pasosTotales = 0;
    public TextMeshProUGUI textoPasosUI;

    public void RegistrarMovimiento()
    {
        pasosTotales++;
        ActualizarInterfaz();
    }

    public void ActualizarInterfaz()
    {
        if (textoPasosUI != null)
        {
            textoPasosUI.text = "Pasos: " + pasosTotales;
        }
    }

    public int GetPasos() => pasosTotales;

    public void ResetearPasos()
    {
        pasosTotales = 0;
        ActualizarInterfaz();
    }
}