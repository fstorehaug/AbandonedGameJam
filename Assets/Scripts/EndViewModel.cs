using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using UnityEngine.SceneManagement;

[Binding]
public class EndViewModel : MonoBehaviour
{
    public SettingsScriptableObject settings;

    [Binding]
    public int playerScore
    {
        get
        {
            return settings.playerScore;
        }
    }

    [Binding]
    public void BackToStart()
    {
        SceneManager.LoadScene(0);
    }
}
