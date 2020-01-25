using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPageController : MonoBehaviour
{
    public Text PlyrNumText;

    public static int playerNum=1;
    
    public void OnStartButtonClicked()
    {
        string [] pn = PlyrNumText.text.Split(' ');
        playerNum=int.Parse(pn[0]);
        SceneManager.LoadScene("Main");
    }

}
