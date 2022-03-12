using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;


public class PlayerBehaviour : MonoBehaviour
{

    public List<Transform> enemiesWhoKnowYou = new List<Transform>();

    public Transform winTrain;
    public Transform particlePoint;
    public GameObject landParticle;

    float minimumDistance;
    [SerializeField] float currentBackflipTime;

    public float sphereSize;
    public float backflipTimer;
    public float minTrainDisance;

    [SerializeField] Transform closestEnemy;

    public AnimationCurve GrainFalloff;

    [SerializeField] UnityEngine.Rendering.Volume _volume;

    public Transform ps5;

    public Animator text1;
    public Animator text2;
    //   public Animator securityGuard;
    Animator ani;

    public Slider flipSlider1;
    public Slider flipSlider2;

    bool canBackflip = true;


    void Start()
    {
        ani = GetComponent<Animator>();

        PlayerPrefs.SetInt("isCaught", 0);

        minimumDistance = Mathf.Infinity;

        InvokeRepeating("updateList", 0, 0.1f);

        PlayerPrefs.SetInt("hasGotPS5", 0);

        InvokeRepeating("reduceTimer", 0, 0.05f);
    }

    void updateList()
    {
        enemiesWhoKnowYou.Clear();

        EnemyBehaviour[] enemies = GameObject.FindObjectsOfType<EnemyBehaviour>();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesWhoKnowYou.Add(enemies[i].transform);
        }


        foreach (Transform enemy in enemiesWhoKnowYou)
        {
            if (enemy.GetComponent<EnemyBehaviour>().isHostile)
            {
                float distance = Vector3.Distance(transform.position, enemy.position);
                if (distance < minimumDistance)
                {
                    closestEnemy = enemy;
                    minimumDistance = distance;
                }
            }
        }


        if (PlayerPrefs.GetInt("hasGotPS5") == 1)
        {
            _volume.weight = GrainFalloff.Evaluate(minimumDistance);
        }
        minimumDistance = Mathf.Infinity;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            flipSlider1.transform.parent.GetComponent<Animator>().Play("Appear");

            if (canBackflip)
            {
                ani.SetTrigger("backflip");

                StartCoroutine(flipEffect());

                currentBackflipTime = backflipTimer;

                ani.ResetTrigger("Stand");
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (PlayerPrefs.GetInt("hasGotPS5") == 0)
            {
                if (Vector3.Distance(transform.position, ps5.position) < 5)
                {
                    ps5.GetComponent<Animator>().Play("dissapear");
                    Destroy(ps5.gameObject, 2f);

                    text1.Play("Now Run");
                    text2.Play("PS5got");

                    Destroy(text1.gameObject, 2f);
                    Destroy(text1.gameObject, 2f);

                    PlayerPrefs.SetInt("hasGotPS5", 1);
                }
            }

            if (Vector3.Distance(transform.position, winTrain.position) < minTrainDisance)
            {
                FindObjectOfType<WinLoose>().end(true);
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerPrefs.SetInt("crouch", 1);
        }
        else
        {
            PlayerPrefs.SetInt("crouch", 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ani.SetTrigger("Crouch");

            ani.ResetTrigger("Stand");
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ani.SetTrigger("Stand");
        }

    }


    void FixedUpdate()
    {
        flipSlider1.value = currentBackflipTime;
        flipSlider2.value = currentBackflipTime;
    }


    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Train"))
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerDetector") && PlayerPrefs.GetInt("isCaught") == 0 && PlayerPrefs.GetInt("hasGotPS5") == 1 && PlayerPrefs.GetInt("crouch") == 0)
        {
            if (other.transform.parent.GetComponent<EnemyBehaviour>().isHostile)
            {
                other.transform.parent.GetComponent<EnemyBehaviour>().detectPlayer(transform);
            }
        }

        if (other.gameObject.CompareTag("Caught Detector") && PlayerPrefs.GetInt("hasGotPS5") == 1 && PlayerPrefs.GetInt("isCaught") == 0 && PlayerPrefs.GetInt("crouch") == 0)
        {
            other.transform.parent.GetComponent<EnemyBehaviour>().catchPlayer();
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerDetector"))
        {
            other.transform.parent.GetComponent<EnemyBehaviour>().HasBeenDetected = false;

            //  enemiesWhoKnowYou.Remove(other.transform.parent);

            other.transform.parent.GetComponent<EnemyBehaviour>().HasBeenDetected = false;
        }
    }

    IEnumerator flipEffect()
    {
        yield return new WaitForSeconds(1.2f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereSize);

        foreach (Collider nearbyObject in colliders)
        {
            //            Debug.Log(nearbyObject.gameObject.name);
            EnemyBehaviour enemy_ = nearbyObject.transform.parent.GetComponent<EnemyBehaviour>();
            if (enemy_)
            {
                enemy_.fall();
                GameObject particle_ = Instantiate(landParticle, particlePoint.position, landParticle.transform.rotation);//particlePoint.rotation);
                Destroy(particle_, 2f);
            }
        }
    }

    void reduceTimer()
    {
        if (currentBackflipTime > 0)
        {
            currentBackflipTime--;
            canBackflip = false;
        }
        else
        {
            canBackflip = true;
        }
    }
}
