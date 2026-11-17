using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    public void jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void verCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
