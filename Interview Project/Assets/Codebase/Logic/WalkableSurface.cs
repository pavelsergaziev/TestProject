using Codebase.Logic.Clickable;
using UnityEngine;

namespace Codebase.Logic
{
  public class WalkableSurface : MonoBehaviour, IContextActionTarget
  {
    public ContextActionTargetTypeId ContextActionTargetTypeId => ContextActionTargetTypeId.WalkableSurface;
  }
}