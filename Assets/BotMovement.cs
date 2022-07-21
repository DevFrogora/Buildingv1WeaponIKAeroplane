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

    public float minXPos;
    public float maxXPos;

    public float minYPos;
    public float maxYPos;

    public float minZPos;
    public float maxZPos;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerDetails = GetComponent<PlayerDetails>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Starting(5));
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
