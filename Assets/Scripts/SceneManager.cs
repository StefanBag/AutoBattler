using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void Battle1()
    {
        SceneManager.LoadScene("BattleScene1");
    }
    public void Battle2()
    {
        SceneManager.LoadScene("BattleScene2");
    }
    public void Battle3()
    {
        SceneManager.LoadScene("BattleScene3");
    }

}
