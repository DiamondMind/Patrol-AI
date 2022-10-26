using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DiamondMind.Prototypes.AIController
{
    public class dAIController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float movementSpeed = 4;
        [SerializeField] private float minimumDistanceToTarget = 1.5f;
        [SerializeField] private bool isInDestination;
        [SerializeField] private float waitTime = 3;

        [SerializeField] private bool iterateDestination;
        [SerializeField] private int waypointIndex;
        [SerializeField] private List<Transform> waypoints;
       
        private void Start()
        {            
            _agent = GetComponent<NavMeshAgent>();  // get navmesh agent component
            _agent.speed = movementSpeed;   // set agent speed
        }
        private void Update()
        {
            Move();
        }
        
        private void Move()
        {
            // Go to target destination
            if (waypoints.Count > -1 && waypoints[waypointIndex] != null)
            {
                _agent.SetDestination(waypoints[waypointIndex].position);
            }
            // Stop at target destination
            float distance = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
            if (distance < minimumDistanceToTarget && !isInDestination)
            {
                _agent.isStopped = true;
                StartCoroutine(Wait()); // start waiting
            }
        }
        // Wait at current destination
        private IEnumerator Wait()
        {
            isInDestination = true;
            if (isInDestination == true)
            {
                //Debug.Log("Is in destination" + waypoints[currentDestination].position);
                yield return new WaitForSeconds(waitTime);
            }
            // move to next destination after waiting
            if (waypoints.Count > 0)
            {
                SetNextDestination();
            }
        }
        // change to next destination
        private void SetNextDestination()
        {
            if (!iterateDestination)
            {
                waypointIndex++;
                if (waypointIndex == waypoints.Count - 1)
                {
                    iterateDestination = true;
                }
            }
            else if (iterateDestination == true)
            {
                waypointIndex--;
                if (waypointIndex == 0)
                {
                    iterateDestination = false;
                }
            }
            isInDestination = false;
            _agent.isStopped = false;
        }
    }
}   