using System;
using DG.Tweening;
using UnityEngine;

namespace Codebase.Logic.Buildings
{
  public class FactoryAnimationTweener : MonoBehaviour
  {
    private const float ScalingAnimationDuration = 3;
    private const float RotationAnimationDuration = 1;
    private const int RotationAnimationLoopsCount = 3;
    
    private readonly Vector3 _scaleEndValue = new Vector3(.01f, .01f, .01f);
    private readonly Vector3 _rotationEndValue = new Vector3(0, 360, 0);
    
    private Sequence _currentAnimationSequence;
    private Action _currentCallback;

    public void AnimateItemDestruction(Transform item, Action callback)
    {
      _currentCallback = callback;

      _currentAnimationSequence = DOTween.Sequence()
        .Append(
          item
            .DOScale(_scaleEndValue, ScalingAnimationDuration)
            .SetEase(Ease.Linear))
        .Insert(
          0,
          item
            .DORotate(_rotationEndValue, RotationAnimationDuration, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetLoops(RotationAnimationLoopsCount, LoopType.Restart))
        .AppendCallback(TriggerAfterSequenceEndedCallback);

      _currentAnimationSequence.Play();
    }

    private void TriggerAfterSequenceEndedCallback()
    {
      _currentAnimationSequence.Kill();
      _currentCallback.Invoke();
    }
  }
}