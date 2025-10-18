using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        //audioManager.PlayDefaultAudio();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
