using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    const int MAINMENUINDEX = 0;
    const int CHARACTERCREATIONINDEX = 1;
    const int GAMEINDEX = 2;


   public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAINMENUINDEX);
    }

    public void LoadCharacterCreation()
    {
        SceneManager.LoadScene(CHARACTERCREATIONINDEX);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(GAMEINDEX);
    }
}
