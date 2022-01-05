using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class unit : MonoBehaviour
{
    [SerializeField]
    GameObject selectedVisual;

    NavMeshAgent Agent;

    Animator anim;

    bool selected;
    Vector3 target;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        select(false);
    }
    private void Update()
    {
        anim.SetFloat("speed", Agent.velocity.magnitude);

        bool closeToTarget = Vector3.Distance(transform.position, target) < 5;
        if (closeToTarget)
        {
            rotateTowardsTarget();
        }
    }

    private void rotateTowardsTarget()
    {
        Vector3 targetDirection = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
    }

    public void select( bool _selected)
    {
        selected = _selected;
        selectedVisual.SetActive(_selected);
    }

    public bool isSelected()
    {
        return selected;
    }

    public void goTo(Vector3 pos,Vector3 lookAtTarget)
    {
        Agent.SetDestination(pos);
        target = lookAtTarget;

    }
    public float getVelocity()
    {
        return Agent.velocity.magnitude;
    }
    public void setSpeed(float speed)
    {
        Agent.speed = speed;
    }
}
