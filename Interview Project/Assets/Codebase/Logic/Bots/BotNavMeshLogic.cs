using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Codebase.Logic.Bots
{
  public class BotNavMeshLogic : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    private const float StoppingDistanceModifier = .1f;
    
    
    //remainingDistance, по всей видимости, апдейтится не прямо сразу после SetDestination
    // и может рассчитываться до старой Destination.
    //Поэтому поставил задержку.
    private const float NavMeshDestinationChangeDelayTime = .1f;

    private float _stoppingDistance;
    private bool _navMeshIsInDelay;

    
    public event Action OnDestinationReached;

    
    private void Start()
    {
      _stoppingDistance = _navMeshAgent.stoppingDistance + StoppingDistanceModifier;
      
      _navMeshAgent.isStopped = true;
    }

    private void Update()
    {
      if(_navMeshAgent.isStopped)
        return;

      if (!_navMeshIsInDelay && _navMeshAgent.remainingDistance < _stoppingDistance) 
        OnDestinationReached?.Invoke();
    }
    

    public void SetDestination(Vector3 targetPosition)
    {
      _navMeshAgent.SetDestination(targetPosition);
      StartCoroutine(NavMeshDelayCoroutine());
    }

    public void StartMoving() => 
      _navMeshAgent.isStopped = false;

    public void StopMoving() => 
      _navMeshAgent.isStopped = true;

    private IEnumerator NavMeshDelayCoroutine()
    {
      _navMeshIsInDelay = true;
      
      yield return new WaitForSeconds(NavMeshDestinationChangeDelayTime);

      _navMeshIsInDelay = false;
    }
  }
}