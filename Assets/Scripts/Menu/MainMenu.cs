using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        //audioManager.PlayDefaultAudio();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
