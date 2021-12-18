using Codebase.Logic.Bots;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.UI
{
  public class AlarmButton : MonoBehaviour
  {
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    private const string SwitchAlarmOnText = "ТРЕВОГА";
    private const string SwitchAlarmOffText = "ОТБОЙ";
    
    private BotsAlarm _botsAlarm;

    
    public void Construct(BotsAlarm botsAlarm) => 
      _botsAlarm = botsAlarm;

    
    private void Start()
    {
      _button.onClick.AddListener(ResolveOnButtonClick);
      
      ShowCorrespondingText();

      _botsAlarm.OnAlarmStateChanged += ShowCorrespondingText;
    }

    private void OnDestroy()
    {
      if (_botsAlarm)
        _botsAlarm.OnAlarmStateChanged -= ShowCorrespondingText;
    }
    

    private void ResolveOnButtonClick()
    {
      _botsAlarm.SwitchAlarmTo(!_botsAlarm.AlarmIsCurrentlyOn);
      ShowCorrespondingText();
    }

    private void ShowCorrespondingText() => 
      _text.text = _botsAlarm.AlarmIsCurrentlyOn ? SwitchAlarmOffText : SwitchAlarmOnText;
  }
}