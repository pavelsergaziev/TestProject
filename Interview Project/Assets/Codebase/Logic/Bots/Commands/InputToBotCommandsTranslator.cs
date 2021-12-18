using System;
using Codebase.Logic.Buildings;
using Codebase.Logic.Clickable;
using Codebase.Services.Input;
using UnityEngine;

namespace Codebase.Logic.Bots.Commands
{
  public class InputToBotCommandsTranslator : MonoBehaviour
  {
    [SerializeField] private Selector _selector;
    [SerializeField] private BotsCommandsIssuer _botsCommandsIssuer;
    
    private IInputService _inputService;

    private Bot _selectedBot;
    private IContextActionTarget _contextActionTarget;

    
    public void Construct(IInputService inputService) => 
      _inputService = inputService;

    
    private void Start()
    {
      _inputService.OnNewContextActionButtonClicked += ResolveNewOverridingContextActionIsTryingToInitiate;
      _inputService.OnAddContextActionButtonClicked += ResolveAddContextActionIsTryingToInitiate;
    }

    private void OnDestroy()
    {
      _inputService.OnNewContextActionButtonClicked -= ResolveNewOverridingContextActionIsTryingToInitiate;
      _inputService.OnAddContextActionButtonClicked -= ResolveAddContextActionIsTryingToInitiate;
    }
    

    private void ResolveNewOverridingContextActionIsTryingToInitiate()
    {
      if (!TrySelectBotAndActionTarget())
        return;

      _botsCommandsIssuer.ClearAllCommandsForBot(_selectedBot);
      
      IssueCommandAndTryInitiatingExecution();
    }

    private void ResolveAddContextActionIsTryingToInitiate()
    {
      if (!TrySelectBotAndActionTarget())
        return;
      
      IssueCommandAndTryInitiatingExecution();
    }

    private void IssueCommandAndTryInitiatingExecution()
    {
      IssueCorrespondingCommand(_contextActionTarget, _selectedBot);
      _botsCommandsIssuer.StartExecutingCommandsForBot(_selectedBot);
    }

    private bool TrySelectBotAndActionTarget()
    {
      if (_selector.CurrentSelection == null
          || _selector.CurrentSelection.SelectableObjectTypeId != SelectableObjectTypeId.Bot)
        return false;

      _selectedBot = _selector.CurrentSelection as Bot;
      _contextActionTarget = _inputService.HitTransform.GetComponent<IContextActionTarget>();

      return _contextActionTarget != null;
    }

    private void IssueCorrespondingCommand(IContextActionTarget contextActionTarget, Bot selectedBot)
    {
      switch (contextActionTarget.ContextActionTargetTypeId)
      {
        case ContextActionTargetTypeId.None:
          break;
        case ContextActionTargetTypeId.WalkableSurface:
          _botsCommandsIssuer.CommandBotToGoToPosition(selectedBot, _inputService.HitPoint);
          break;
        case ContextActionTargetTypeId.Factory:
          _botsCommandsIssuer.CommandBotToGoDeliverItemToFactory(selectedBot, contextActionTarget as Factory);
          break;
        case ContextActionTargetTypeId.Warehouse:
          _botsCommandsIssuer.CommandBotToGoWaitForItemAtWarehouse(selectedBot, contextActionTarget as Warehouse);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}