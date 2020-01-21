using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPageController : MonoBehaviour
{
    public static int playerNum=1;
    
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }

}
