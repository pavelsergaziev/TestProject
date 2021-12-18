using UnityEngine;
using UnityEngine.AI;

namespace Codebase.Logic.Bots
{
  public class BotAnimation : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;

    private const float MinSquareMagnitudeToTriggerMovementAnimation = .2f;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    
    private void Update()
    {
      if (!_animator.GetBool(IsMoving)
          && _navMeshAgent.velocity.sqrMagnitude > MinSquareMagnitudeToTriggerMovementAnimation)
        _animator.SetBool(IsMoving, true);
      
      else if (_animator.GetBool(IsMoving)
               && _navMeshAgent.velocity.sqrMagnitude < MinSquareMagnitudeToTriggerMovementAnimation)
        _animator.SetBool(IsMoving, false);
    }
  }
}