using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovementFunction : MonoBehaviour
{
    [SerializeField] private float navMeshSearchRange = 1.0f;
    //[SerializeField] private string walkAnimation = "isWalking";
    //private int _walkAnimationId;
    //private Animator _animator;
    private NavMeshAgent _agent;
    private NavMeshHit _hit;

    void Start()
    {

    }
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        //_animator = GetComponent<Animator>();
        //_walkAnimationId = Animator.StringToHash(walkAnimation);
    }
    //void Update()
    //{
        //if (_animator)
        //{
            //if (_agent.remainingDistance < 0.1f)
                //_animator.SetBool(_walkAnimationId, false);
            //else
                //_animator.SetBool(_walkAnimationId, true);
     //}

    //}
    public void MoveToRandom(Vector3 center, float range)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPoint, out _hit, navMeshSearchRange, NavMesh.AllAreas))
            MoveTo(_hit.position);
    }
    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);

    }
    private void OnDrawGizmosSelected()
    {
        if (_agent == null) return;
        if (_agent.isStopped) return;

        Vector3[] corners = _agent.path.corners;
        int size = corners.Length;

        for (var i = 1; i < size; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(corners[i - 1], corners[i]);
        }
    }
}
