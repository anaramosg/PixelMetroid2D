using TMPro;
using UnityEngine;

public class ControlHUD : MonoBehaviour
{
    public TMP_Text vidasTxt;
    public TMP_Text powerUpTxt;
    public TMP_Text tiempoTxt;

    private void Awake()
    {
        if (vidasTxt == null || powerUpTxt == null || tiempoTxt == null)
            Debug.LogError("Asigna los textos de TMP del HUD al componente controlHud");
    }

    public void SetVidas(int cantidad)
    {
        if(vidasTxt != null)
        {
            vidasTxt.text = "Vidas : " + cantidad;
        }
    }

    public void SetPowerUP(int cantidad)
    {
        if(powerUpTxt != null)
        {
            powerUpTxt.text = "Objetos : " + cantidad;
        }
    }

    public void SetTiempo(int segundosRestantes)
    {
        if (tiempoTxt == null) return;

        int minutos = segundosRestantes / 60;
        int segundos = segundosRestantes % 60;

        tiempoTxt.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }
}
