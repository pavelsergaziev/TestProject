using UnityEngine;
using UnityEngine.AI;

namespace Codebase.Logic.Bots
{
  public class BotWalkingVFX : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private VfxPlayer _vfxPlayer;

    private const float ThresholdSquareMagnitudeToTriggerVFX = 1f;

    private void Update()
    {
      if (_vfxPlayer.IsCurrentlyPlaying
          && _navMeshAgent.velocity.sqrMagnitude < ThresholdSquareMagnitudeToTriggerVFX)
        _vfxPlayer.StopVFX();

      else if (!_vfxPlayer.IsCurrentlyPlaying
               && _navMeshAgent.velocity.sqrMagnitude > ThresholdSquareMagnitudeToTriggerVFX)
        _vfxPlayer.PlayVFX();
    }
  }
}