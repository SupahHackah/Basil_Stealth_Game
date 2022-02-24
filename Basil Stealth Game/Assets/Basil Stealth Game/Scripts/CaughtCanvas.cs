using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaughtCanvas : MonoBehaviour
{
    Animator ani;
    public Slider progressSlider;
    [SerializeField] float currentValue;

    public float maxForce;

    int index;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void enter()
    {
        ani.Play("Caught In");

        currentValue = 0;

        progressSlider.value = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        InvokeRepeating("decrease", 0, 0.5f);

        PlayerPrefs.SetInt("isCaught", 1);
    }

    public void Exit()
    {
        ani.Play("Caught Out");

        CancelInvoke();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PlayerPrefs.SetInt("isCaught", 0);
    }

    public void buttonClick()
    {

        if (currentValue > progressSlider.maxValue)
        {
            Exit();
        }

        currentValue += 2;

        progressSlider.value = currentValue;

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
        currentValue--;

        progressSlider.value = currentValue;


        if (currentValue > progressSlider.maxValue)
        {
            Exit();
        }
    }
}
