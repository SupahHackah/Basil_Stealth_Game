using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoose : MonoBehaviour
{

    Animator ani;

    public Text yh;

    void Start()
    {
        ani = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ani.Play("Win");
        }
    }

    public void playagain()
    {
        SceneManager.LoadScene(0);
    }

    public void end(bool won)
    {
        if (won)
        {
            if (PlayerPrefs.GetInt("hasGotPS5") == 1)
            {

                SceneManager.LoadScene(1);

                Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
            }

        }
        else
        {
            yh.text = "You Lost";
            ani.Play("Lost");
        }
    }
}
