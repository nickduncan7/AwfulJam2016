using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
