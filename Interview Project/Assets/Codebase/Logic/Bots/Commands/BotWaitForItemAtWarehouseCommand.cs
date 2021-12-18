using System;
using Codebase.Logic.Buildings;
using UnityEngine;

namespace Codebase.Logic.Bots.Commands
{
  public class BotWaitForItemAtWarehouseCommand : ICommand
  {
    private readonly Inventory _bot;
    private readonly Warehouse _warehouse;
    
    public event Action OnExecutionComplete;

    
    public BotWaitForItemAtWarehouseCommand(Inventory bot, Warehouse warehouse)
    {
      _bot = bot;
      _warehouse = warehouse;
    }
    

    public void Execute()
    {
      if(_bot.IsCurrentlyHoldingAnItem)
        CompleteExecutionSuccessfully();
      
      else if (_warehouse.Inventory.IsCurrentlyHoldingAnItem)
        TransferItem();

      else
        _warehouse.Inventory.OnInventoryContentsChanged += ResolveItemSpawnedAtWarehouse;
    }

    public void StopExecution() => 
      _warehouse.Inventory.OnInventoryContentsChanged -= ResolveItemSpawnedAtWarehouse;

    private void ResolveItemSpawnedAtWarehouse()
    {
      if (!_warehouse.Inventory.IsCurrentlyHoldingAnItem)
        return;
      
      TransferItem();
    }

    private void TransferItem()
    {
      _warehouse.Inventory.TakeItemOutOfInventory(out Transform item);
      GiveItemToBot(item, _bot);

      CompleteExecutionSuccessfully();
    }

    private static void GiveItemToBot(Transform warehouseCurrentAvailableItem, Inventory bot) => 
      bot.PutItemIntoInventory(warehouseCurrentAvailableItem);

    private void CompleteExecutionSuccessfully()
    {
      StopExecution();
      OnExecutionComplete?.Invoke();
    }
  }
}