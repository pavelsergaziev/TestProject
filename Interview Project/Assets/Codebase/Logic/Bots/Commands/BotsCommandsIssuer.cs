using System;
using System.Collections.Generic;
using Codebase.Logic.Buildings;
using UnityEngine;

namespace Codebase.Logic.Bots.Commands
{
  public class BotsCommandsIssuer : MonoBehaviour
  {
    [SerializeField] private BotsAlarm _alarm;
    
    private ParkingPlot _parkingPlot;
    
    private Dictionary<Bot, BotCommandsSequence> _botCommandsDictionary;
    private BotCommandsSequence _currentBotCommandsSequence;

    
    public void Construct(ParkingPlot parkingPlot) => 
      _parkingPlot = parkingPlot;
    

    private void Start() => 
      _alarm.OnAlarmStateChanged += ResolveOnAlarmStateChanged;

    private void OnDestroy()
    {
      if(_alarm)
        _alarm.OnAlarmStateChanged -= ResolveOnAlarmStateChanged;
    }
    

    public void AddBot(Bot bot)
    {
      if(_botCommandsDictionary == null)
        _botCommandsDictionary = new Dictionary<Bot, BotCommandsSequence>();
      
      _botCommandsDictionary.Add(bot, new BotCommandsSequence());
    }

    public void StartExecutingCommandsForBot(Bot bot)
    {
      if(_alarm.AlarmIsCurrentlyOn)
        return;
      
      DetermineCurrentBotCommandSequence(bot);
      _currentBotCommandsSequence.StartCommandsExecution();
    }

    public void ClearAllCommandsForBot(Bot bot)
    {
      if(_alarm.AlarmIsCurrentlyOn)
        return;
      
      DetermineCurrentBotCommandSequence(bot);

      _currentBotCommandsSequence.ClearCommands();
    }

    public void CommandBotToGoToPosition(Bot bot, Vector3 targetPosition)
    {
      if(_alarm.AlarmIsCurrentlyOn)
        return;
      
      DetermineCurrentBotCommandSequence(bot);
      
      _currentBotCommandsSequence.AddCommand(new BotMovementCommand(bot.NavMeshLogic, targetPosition));
    }

    public void CommandBotToGoWaitForItemAtWarehouse(Bot bot, Warehouse warehouse)
    {
      if(_alarm.AlarmIsCurrentlyOn)
        return;
      
      DetermineCurrentBotCommandSequence(bot);

      MacroCommand command = new MacroCommand();
      
      command
        .AddCommand(new BotMovementCommand(bot.NavMeshLogic, warehouse.CharacterInteractionPointForItemSupplying.position))
        .AddCommand(new BotWaitForItemAtWarehouseCommand(bot.Inventory, warehouse));
      
      _currentBotCommandsSequence.AddCommand(command);
    }

    public void CommandBotToGoDeliverItemToFactory(Bot bot, Factory factory)
    {
      if(_alarm.AlarmIsCurrentlyOn)
        return;

      DetermineCurrentBotCommandSequence(bot);
      
      MacroCommand command = new MacroCommand();

      command
        .AddCommand(new BotMovementCommand(bot.NavMeshLogic, factory.CharacterInteractionPointForItemSupplying.position))
        .AddCommand(new BotWaitAndGiveItemToFactoryCommand(bot.Inventory, factory));
      
      _currentBotCommandsSequence.AddCommand(command);
    }

    private void ResolveOnAlarmStateChanged()
    {
      if(_alarm.AlarmIsCurrentlyOn)
        SendBotsIntoEmergencyMode();
      else
        StopEmergencyModeForBots();
    }

    private void SendBotsIntoEmergencyMode()
    {
      foreach (KeyValuePair<Bot, BotCommandsSequence> keyValuePair in _botCommandsDictionary)
      {
        keyValuePair.Value.StopExecutingCurrentCommand();
        keyValuePair.Key.NavMeshLogic.SetDestination(_parkingPlot.AlarmTargetPositionForBots.position);
        keyValuePair.Key.NavMeshLogic.StartMoving();
      }
    }

    private void StopEmergencyModeForBots()
    {
      foreach (KeyValuePair<Bot, BotCommandsSequence> keyValuePair in _botCommandsDictionary)
      {
        keyValuePair.Key.NavMeshLogic.StopMoving();
        keyValuePair.Value.StartCommandsExecution();
      }
    }

    private void DetermineCurrentBotCommandSequence(Bot bot)
    {
      if (!_botCommandsDictionary.TryGetValue(bot, out _currentBotCommandsSequence))
        throw new Exception("Selected bot is not in the bots list");
    }
  }
}