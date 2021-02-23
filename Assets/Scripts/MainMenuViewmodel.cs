using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using UnityEngine.SceneManagement;

[Binding]
public class MainMenuViewmodel : MonoBehaviour
{
    [Binding]
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

}
