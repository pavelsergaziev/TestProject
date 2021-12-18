using System;
using Codebase.Logic.Buildings;
using UnityEngine;

namespace Codebase.Logic.Bots.Commands
{
  public  class BotWaitAndGiveItemToFactoryCommand : ICommand
  {
    private readonly Inventory _bot;
    private readonly Factory _factory;
    
    public event Action OnExecutionComplete;

    
    public BotWaitAndGiveItemToFactoryCommand(Inventory bot, Factory factory)
    {
      _bot = bot;
      _factory = factory;
    }
    

    public void Execute()
    {
      if(!_bot.IsCurrentlyHoldingAnItem)
        CompleteExecutionSuccessfully();
      
      else if (!_factory.InventoryIsCurrentlyHoldingAnItem)
        TransferItem();

      else
        _factory.OnInventoryHasBecomeVacant += ResolveFactoryInventoryHasBecomeVacant;
    }

    public void StopExecution() => 
      _factory.OnInventoryHasBecomeVacant -= ResolveFactoryInventoryHasBecomeVacant;

    private void ResolveFactoryInventoryHasBecomeVacant()
    {
      if (!_factory.InventoryIsCurrentlyHoldingAnItem)
        TransferItem();
    }

    private void TransferItem()
    {
      _bot.TakeItemOutOfInventory(out Transform item);
      _factory.PutItemIntoFactoryInventory(item);
      
      CompleteExecutionSuccessfully();
    }

    private void CompleteExecutionSuccessfully()
    {
      StopExecution();
      OnExecutionComplete?.Invoke();
    }
  }
}