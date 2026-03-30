using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public void SwapScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {

            Application.Quit();

    }
}
