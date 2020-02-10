using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float PlayerHealth = 1.0f;
    public static float FireHealth = 0.5f;

    public static float PaperSceneStartHealth = 0.0f;
    public static float FireSceneStartHealth = 0.0f;


    public static void LoadScene(int pSceneNumber)
    {
        SceneManager.LoadScene(pSceneNumber);
    }

    public static void ReloadScene()
    {
        PlayerHealth = PaperSceneStartHealth;
        FireHealth = FireSceneStartHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
