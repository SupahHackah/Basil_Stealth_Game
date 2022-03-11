using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iNTRO : MonoBehaviour
{
    public GameObject introCam;
    public GameObject Fade;
    public GameObject player;

    public GameObject introPlayer;

    public bool shouldIntro;
    public bool overridePlayerPos;

    public float initialDelay;
    public float afterDelay;

    void Start()
    {
        if (overridePlayerPos)
        {
            player.transform.position = new Vector3(-120.0242f, -14.36f, -17.18278f);
            player.transform.rotation = Quaternion.Euler(0, 71.159f, 0);
        }
        if (shouldIntro)
        {
            Invoke("fade", initialDelay);
            Invoke("removeCam", initialDelay + afterDelay);
            Invoke("spawnInPlayerFake", 10);

            player.SetActive(false);
            introCam.SetActive(true);
            Fade.SetActive(false);

            introPlayer.SetActive(false);

        }
        else
        {
            introCam.SetActive(false);
            Fade.SetActive(false);
            player.SetActive(true);

            introPlayer.SetActive(false);
        }
    }

    void removeCam()
    {
        Destroy(introCam);

        player.SetActive(true);
        introPlayer.SetActive(false);

    }

    void spawnInPlayerFake()
    {
        introPlayer.SetActive(true);
    }

    void fade()
    {
        Fade.SetActive(true);
    }
}
