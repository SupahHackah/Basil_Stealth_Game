using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    Animator ani;


    void Start()
    {
        ani = GetComponent<Animator>();

        PlayerPrefs.SetInt("isCaught", 0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.SetTrigger("backflip");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ani.SetTrigger("frontflip");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerDetector") && PlayerPrefs.GetInt("isCaught") == 0)
        {
            other.transform.parent.GetComponent<EnemyBehaviour>().detectPlayer(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerDetector"))
        {
            other.transform.parent.GetComponent<EnemyBehaviour>().HasBeenDetected = false;
        }
    }
}
