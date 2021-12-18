using UnityEngine;

namespace Codebase.Logic
{
  public class VfxPlayer : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _particleSystem;

    public bool IsCurrentlyPlaying => _particleSystem.isPlaying;
    
    private void Start() => StopVFX();
    
    public void PlayVFX() => _particleSystem.Play();
    
    public void StopVFX() => _particleSystem.Stop();
  }
}