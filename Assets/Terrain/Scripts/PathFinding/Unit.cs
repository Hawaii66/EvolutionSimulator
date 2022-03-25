using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed = 5;
    public Vector3[] path;
    int targetIndex;
    public bool hasPath = false;
    public void SetDestination(Vector3 target)
    {
        if (hasPath == false)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
            hasPath = true;
        }
    }

    public void ResetHasPath(bool newState)
    {
        hasPath = newState;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesfull)
    {
        if(pathSuccesfull)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        } else
        {
            //GetComponent<Animal>().currentAction = Animal.CreatureActions.idle;
            Debug.Log("CANT FIND PATH");
            GetComponent<CreatureBrain>().ErrorCreatingPath();
            hasPath = false;
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(currentWaypoint.x, currentWaypoint.z));
            //float dist = Vector3.Distance(transform.position, currentWaypoint);
            //Debug.Log(dist);
            if(dist < 0.1f)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    hasPath = false;
                    GetComponent<CreatureBrain>().ReachedGoal(true);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.3f);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]); ;
                } else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
