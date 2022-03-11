using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStationDodge : MonoBehaviour
{
    public RectTransform Controller;
    public Transform Arrow;

    public float defaultSpeed;

    bool isStopped;

    bool isLeft;

    public Vector3 controllerPos;

    public void play()
    {
        if (PlayerPrefs.GetInt("isCaught") == 0)
        {
            Arrow.localPosition = new Vector3(Random.Range(-150, 150), 68, 0);

            isStopped = false;

            controllerPos.x = 0;

            GetComponent<Animator>().Play("caught 2");

            PlayerPrefs.SetInt("isCaught", 1);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("isCaught") == 1 && PlayerPrefs.GetInt("game") == 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isStopped = true;

                if (Vector3.Distance(Arrow.localPosition, Controller.localPosition) < 90)
                {
                    GetComponent<Animator>().Play("caught 2 0");

                    PlayerPrefs.SetInt("isCaught", 0);
                }
                else
                {
                    GetComponent<Animator>().Play("caught 2 0");
                    FindObjectOfType<WinLoose>().end(false);
                }
            }
        }

    }

    void FixedUpdate()
    {
        if (isLeft)
        {
            if (controllerPos.x > -200)
            {
                controllerPos.x -= defaultSpeed;
            }
            else
            {
                isLeft = false;
            }
        }
        else
        {
            if (controllerPos.x < 200)
            {
                controllerPos.x += defaultSpeed;
            }
            else
            {
                isLeft = true;
            }
        }

        if (!isStopped)
        {
            Controller.localPosition = controllerPos;
        }
    }
}
