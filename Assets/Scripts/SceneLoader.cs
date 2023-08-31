using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SceneLoad(int num)
    {
        SceneManager.LoadScene(num);
    }

    public void SceneReLoad()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ExitProgram()
    {
        Application.Quit();
    }
}
