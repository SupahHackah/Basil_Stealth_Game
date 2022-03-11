using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaughtCanvas : MonoBehaviour
{
    Animator ani;

    [Header("click to win")]
    public Slider progressSlider;
    [SerializeField] float currentValue;
    public float maxForce;

    [Header("ps Catch")]



    [Space]
    bool isCaught;

    int index;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void enter()
    {
        if (PlayerPrefs.GetInt("isCaught") == 0)
        {

            PlayerPrefs.SetInt("game", 1);
            ani.Play("Caught In");

            currentValue = 0;

            progressSlider.value = 0;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            InvokeRepeating("decrease", 0, 0.5f);

            PlayerPrefs.SetInt("isCaught", 1);

            isCaught = true;
        }
    }

    public void Exit()
    {
        ani.Play("Caught Out");

        CancelInvoke();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PlayerPrefs.SetInt("isCaught", 0);

        isCaught = false;

        PlayerPrefs.SetInt("game", 0);
    }

    void Update()
    {
        if (isCaught)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                buttonClick();
            }
        }
    }

    void FixedUpdate()
    {
        progressSlider.value = Mathf.Lerp(progressSlider.value, currentValue, 0.1f);

        if (progressSlider.value < progressSlider.minValue - 20)
        {
            ani.Play("Caught Out");
            FindObjectOfType<WinLoose>().end(false);
        }
    }

    public void buttonClick()
    {

        if (currentValue > progressSlider.maxValue)
        {
            Exit();
        }

        currentValue += 30;

        if (index % 2 == 0)
        {
            progressSlider.GetComponent<Rigidbody2D>().AddTorque(maxForce);
        }
        else
        {
            progressSlider.GetComponent<Rigidbody2D>().AddTorque(-maxForce);
        }

        index++;

    }

    void decrease()
    {
        currentValue -= 30f;


        if (currentValue > progressSlider.maxValue)
        {
            Exit();
        }
    }
}
