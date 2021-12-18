using UnityEngine;

namespace Codebase.Logic.Buildings
{
  public class ParkingPlot : MonoBehaviour
  {
    [SerializeField] private Transform alarmTargetPositionForBots;
    public Transform AlarmTargetPositionForBots => alarmTargetPositionForBots;
  }
}