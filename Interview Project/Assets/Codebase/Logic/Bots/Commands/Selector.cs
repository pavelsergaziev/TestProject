using Codebase.Logic.Clickable;
using Codebase.Services.Input;
using UnityEngine;

namespace Codebase.Logic.Bots.Commands
{
  public class Selector : MonoBehaviour
  {
    public ISelectable CurrentSelection { get; private set; }

    private IInputService _inputService;


    public void Construct(IInputService inputService) => 
      _inputService = inputService;

    
    private void Start() => 
      _inputService.OnSelectButtonClicked += ResolveSelectButtonClicked;

    private void OnDestroy() =>
      _inputService.OnSelectButtonClicked -= ResolveSelectButtonClicked;

    
    private void ResolveSelectButtonClicked() => 
      CurrentSelection = _inputService.HitTransform.GetComponent<ISelectable>();
  }
}