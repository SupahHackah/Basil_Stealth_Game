using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject[] Characters;
    public Animator detectionMark;

    public Transform particleSpawnLocation;

    public int indexInListForBlur;

    //AI STUFF
    [Header("AI Stuff")]
    public float walkRadius;
    [SerializeField] float distanceToTarget;
    public float WalkSpeed = 2.5f;
    public float walkSpeedAddition;

    Vector3 Destination;
    Quaternion targetRot;

    NavMeshAgent agent;

    [HideInInspector]
    public Animator ani;

    public bool shouldSpawn = true;
    public bool HasBeenDetected;
    public bool isHostile;

    bool hasGottenPS5;

    bool hasArrived;
    bool isFalling;

    PlayerBehaviour player;

    Vector3 lastPos;

    Vector3 spawnPos;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (shouldSpawn)
        {
            Transform characterModel = Instantiate(Characters[Random.Range(0, Characters.Length)], transform).transform;

            characterModel.localPosition = new Vector3(0, 0, 0);

            float currentScale = characterModel.localScale.x;
            float scaleAmount = (currentScale / 10);

            float newScale = Random.Range(currentScale - scaleAmount, currentScale + scaleAmount);
            characterModel.localScale = new Vector3(newScale, newScale, newScale);
        }

        spawnPos = transform.position;

        player = FindObjectOfType<PlayerBehaviour>();
        agent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Animator>())
            {
                ani = transform.GetChild(i).GetComponent<Animator>();
            }
        }
        Invoke("startupSequence", 0);//Random.Range(0, 5));
    }

    void startupSequence()
    {



        //find the child with the animator
        /*
        Transform[] childsT = new Transform[transform.childCount];

        int j = 0;
        foreach (Transform child in transform)
        {
            childsT[j] = child;
            j++;
        }

        for (int i = 0; i < childsT.Length; i++)
        {
            if (childsT[i].GetComponent<Animator>())
            {
                ani = transform.GetChild(i).GetComponent<Animator>();
            }
        }
        */
        StartCoroutine(waitForPosition());
        InvokeRepeating("checkForMovement", 10, 0.5f);
    }

    void checkForMovement()
    {
        if (!isFalling)
        {
            if (!hasArrived)
            {
                if (Vector3.Distance(transform.position, lastPos) < 0.2f)
                {
                    LookForPosition();
                }
            }

            lastPos = transform.position;
        }
    }

    void FixedUpdate()
    {
        distanceToTarget = Vector3.Distance(transform.position, Destination);
        if (distanceToTarget < 0.1f && !hasArrived && !isFalling)
        {
            hasArrived = true;

            StartCoroutine(waitForPosition());
        }
        else
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.1f);
        }

        if (HasBeenDetected)
        {
            ani.SetBool("Detected", true);

            detectionMark.SetBool("Detected", true);
            detectionMark.gameObject.SetActive(true);

            agent.speed = WalkSpeed * walkSpeedAddition;
        }
        else
        {
            ani.SetBool("Detected", false);

            detectionMark.SetBool("Detected", false);
            detectionMark.gameObject.SetActive(false);

            agent.speed = WalkSpeed;
        }

        if (!hasGottenPS5)
        {
            if (PlayerPrefs.GetInt("hasGotPS5") == 1)
            {
                hasGottenPS5 = true;
            }
        }
    }

    void LookForPosition()
    {

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        Destination = hit.position;

        agent.SetDestination(Destination);

        Transform lookrot = new GameObject().transform;
        lookrot.position = transform.position;
        lookrot.LookAt(Destination);

        targetRot = lookrot.rotation;

        ani.SetTrigger("Walk");
        ani.SetBool("Destination", false);
        ani.SetInteger("IdleInt", Random.Range(0, 5));

        Destroy(lookrot.gameObject);

        hasArrived = false;
    }

    public void detectPlayer(Transform playerTransform)
    {
        if (PlayerPrefs.GetInt("isCaught") == 1)
        {
            LookForPosition();
        }

        if (!HasBeenDetected && hasGottenPS5 && isHostile)
        {
            ani.SetTrigger("Walk");

            //player.enemiesWhoKnowYou.Add(transform);
            // indexInListForBlur = player.enemiesWhoKnowYou.Count - 1;
        }
        if (!isFalling)
        {
            ani.SetBool("Destination", false);

            Destination = playerTransform.position;
            agent.destination = Destination;
            HasBeenDetected = true;
        }
    }

    public void fall()
    {
        ani.SetTrigger("Blast");

        Destination = transform.position;
        agent.destination = Destination;

        StartCoroutine(fallWait());
        isFalling = true;
    }

    public void catchPlayer()
    {
        if (PlayerPrefs.GetInt("isCaught") == 0)
        {
            if (Random.Range(0, 2) == 1)
            {
                FindObjectOfType<CaughtCanvas>().enter();

                // FindObjectOfType<PlayStationDodge>().play();
            }
            else
            {
                FindObjectOfType<PlayStationDodge>().play();
            }

            if (Vector3.Distance(transform.position, spawnPos) < 10)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = spawnPos;
            }
        }
    }

    IEnumerator waitForPosition()
    {
        ani.SetBool("Destination", true);

        yield return new WaitForSeconds(Random.Range(1f, 10f));
        // ani.SetTrigger("IsIdle", false);
        LookForPosition();
    }

    IEnumerator fallWait()
    {
        yield return new WaitForSeconds(Random.Range(8.7f, 9.3f));
        isFalling = false;
    }
}
