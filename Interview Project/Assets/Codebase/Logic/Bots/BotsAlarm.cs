using System;
using UnityEngine;

namespace Codebase.Logic.Bots
{
  public class BotsAlarm : MonoBehaviour
  {
    public bool AlarmIsCurrentlyOn { get; private set; }
    
    public event Action OnAlarmStateChanged;
    
    public void SwitchAlarmTo(bool value)
    {
      AlarmIsCurrentlyOn = value;
      OnAlarmStateChanged?.Invoke();
    }
  }
}