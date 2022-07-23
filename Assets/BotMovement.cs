using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BotMovement : MonoBehaviour
{
    private PlayerDetails playerDetails;
    private NavMeshAgent agent;
    private Animator animator;
    private static int ANIMATOR_PARAM_WALK_SPEED =Animator.StringToHash("WalkSpeed");
    Vector3 target;

     float minXPos;
     float maxXPos;

     float minYPos;
     float maxYPos;

     float minZPos;
     float maxZPos;

    public GameObject platform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerDetails = GetComponent<PlayerDetails>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Starting(5));

        minXPos = platform.transform.position.x - (platform.transform.localScale.x / 2 - 1);
        maxXPos = platform.transform.position.x + (platform.transform.localScale.x / 2 - 1);

        minYPos = platform.transform.position.y + (platform.transform.localScale.y / 2);
        maxYPos = platform.transform.position.y + (platform.transform.localScale.y / 2);

        minZPos = platform.transform.position.z - (platform.transform.localScale.z / 2 - 1);
        maxZPos = platform.transform.position.z + (platform.transform.localScale.z / 2 - 1);

    }
    IEnumerator Starting(int second)
    {
        animator.SetFloat(ANIMATOR_PARAM_WALK_SPEED, 0);
        playerDetails.iswalking = false;
        GenerateRandomPosition();
        yield return new WaitForSeconds(second);
        agent.SetDestination(target);
        while (agent.remainingDistance > 0.5f)
        {
            animator.SetFloat(ANIMATOR_PARAM_WALK_SPEED, agent.velocity.magnitude);
            playerDetails.iswalking = true;
            yield return null;
        }

        playerDetails.iswalking = false;
        // replacing while loop to 
        //yield return new WaitUntil(() => agent.remainingDistance > 0.5f);
        StartCoroutine(Starting(5));

    }

    void GenerateRandomPosition()
    {
        float spawnPointX = Random.Range(minXPos, maxXPos);
        float spawnPointZ = Random.Range(minZPos, maxZPos);
        target = new Vector3(spawnPointX, minYPos, spawnPointZ);
    }
}
