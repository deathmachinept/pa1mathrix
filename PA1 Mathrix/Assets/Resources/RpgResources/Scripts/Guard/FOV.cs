using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class FOV : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    private Collider2D targetInViewRadius;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    
    public List<Transform> visibleTargets = new List<Transform>();

    void Update()
    {
        FindVisibleTargets();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        targetInViewRadius = Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y), viewRadius, targetMask);
        if (targetInViewRadius != null)
        {
            Transform target = targetInViewRadius.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    gameObject.GetComponent<EnemyController>().PlayerVisible = true;
                }
                else
                {
                    gameObject.GetComponent<EnemyController>().PlayerVisible = true;
                }
            }
        }
        else
        {
            gameObject.GetComponent<EnemyController>().PlayerVisible = false;
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
