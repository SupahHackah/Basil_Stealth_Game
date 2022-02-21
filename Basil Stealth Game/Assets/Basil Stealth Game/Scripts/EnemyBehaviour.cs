using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject[] Characters;
    public GameObject detectionParticles;

    public Transform particleSpawnLocation;

    //AI STUFF
    [Header("AI Stuff")]
    public float walkRadius;
    [SerializeField] float distanceToTarget;

    Vector3 Destination;
    Quaternion targetRot;

    NavMeshAgent agent;

    Animator ani;

    public bool shouldSpawn = true;
    public bool HasBeenDetected;
    bool hasArrived;

    void Start()
    {
        if (shouldSpawn)
        {
            Transform characterModel = Instantiate(Characters[Random.Range(0, Characters.Length)], transform).transform;

            characterModel.localPosition = new Vector3(0, 0, 0);
        }

        agent = GetComponent<NavMeshAgent>();

        //find the child with the animator
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


        StartCoroutine(waitForPosition());
    }

    void FixedUpdate()
    {
        distanceToTarget = Vector3.Distance(transform.position, Destination);
        if (distanceToTarget < 0.1f && !hasArrived)
        {
            hasArrived = true;
            
            StartCoroutine(waitForPosition());
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.1f);
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
        if (!HasBeenDetected)
        {
            GameObject particlesInstance = Instantiate(detectionParticles, particleSpawnLocation.position, particleSpawnLocation.rotation);
            particlesInstance.transform.parent = transform;

            Destroy(particlesInstance, 2f);
        }

        ani.SetBool("Destination", false);

        Destination = playerTransform.position;
        agent.destination = playerTransform.position;
        HasBeenDetected = true;
    }

    IEnumerator waitForPosition()
    {
        ani.SetBool("Destination", true);

        yield return new WaitForSeconds(Random.Range(1f, 10f));
        // ani.SetTrigger("IsIdle", false);
        LookForPosition();
    }
}
