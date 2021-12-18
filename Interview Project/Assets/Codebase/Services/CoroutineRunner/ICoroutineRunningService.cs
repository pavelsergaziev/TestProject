using System.Collections;
using UnityEngine;

namespace Codebase.Services.CoroutineRunner
{
  public interface ICoroutineRunningService : IService
  {
    Coroutine StartCoroutine(IEnumerator coroutine);
    void StopCoroutine(IEnumerator coroutine);
  }
}