using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void SafeStartCoroutine(this MonoBehaviour monoBehaviour, ref IEnumerator routineVariable, IEnumerator routineFunction)
    {
        if (routineVariable != null)
            monoBehaviour.StopCoroutine(routineVariable);
        routineVariable = routineFunction;
        monoBehaviour.StartCoroutine(routineVariable);
    }
    public static void SafeStopCoroutine(this MonoBehaviour monoBehaviour, ref IEnumerator routine)
    {
        if (routine != null)
            monoBehaviour.StopCoroutine(routine);

        routine = null;
    }

    public static void DelayExecute(this MonoBehaviour monoBehaviour, float delayTime, Action executionFunction, bool useRealTime = false)
    {
        monoBehaviour.StartCoroutine(DelayExecuteRoutine(delayTime, executionFunction, useRealTime));
    }
    public static void SafeDelayExecute(this MonoBehaviour monoBehaviour, ref IEnumerator routineVariable, float delayTime, Action action, bool useRealTime = false)
    {
        monoBehaviour.SafeStartCoroutine(ref routineVariable, DelayExecuteRoutine(delayTime, action, useRealTime));
    }
    public static IEnumerator DelayExecuteRoutine(float delayTime, Action executionFunction, bool useRealTime = false)
    {
        if (useRealTime == false)
            yield return TimeExtensions.WaitForSeconds(delayTime);
        else
            yield return TimeExtensions.WaitForSecondsRealtime(delayTime);

        executionFunction?.Invoke();
    }




    public static void SafeExecute(this MonoBehaviour monoBehaviour, Action executionFunction, Func<bool> checkConditionFunction, Action timeOutFunction = null, float timeOutTime = 0f)
    {
        monoBehaviour.StartCoroutine(SafeExecuteRoutine(executionFunction, checkConditionFunction, timeOutFunction, timeOutTime));
    }
    static IEnumerator SafeExecuteRoutine(Action executionFunction, Func<bool> checkConditionFunction, Action timeOutFunction = null, float timeOutTime = 0f)
    {
        float curTime = 0f;
        bool checkCondition = checkConditionFunction();

        while (checkCondition == false && curTime <= timeOutTime)
        {
            checkCondition = checkConditionFunction();
            yield return null;
            if (timeOutTime != 0f)
                curTime += Time.unscaledDeltaTime;
        }
        //조건 만족.
        if (checkCondition == true)
        {
            executionFunction();
            yield break;
        }
        //시간 초과.
        if (curTime > timeOutTime)
        {
            timeOutFunction?.Invoke();
            yield break;
        }
    }

    public static void HandleActive(this MonoBehaviour monoBehaviour, GameObject gameObject, bool value, float delayTime = 0f, bool useRealTime = false)
    {
        if (delayTime == 0f)
        {
            gameObject.SetActive(value);
        }
        else
        {
            monoBehaviour.StartCoroutine(HandleActiveRoutine(gameObject, value, delayTime, useRealTime));
        }
    }
    static IEnumerator HandleActiveRoutine(GameObject gameObject, bool value, float delayTime, bool useRealTime)
    {
        if (useRealTime == true)
            yield return TimeExtensions.WaitForSecondsRealtime(delayTime);
        else
            yield return TimeExtensions.WaitForSeconds(delayTime); // new WaitForSeconds(delayTime);

        gameObject.SetActive(value);
    }
    public static IEnumerator HandleActiveRoutine(this MonoBehaviour monoBehaviour, GameObject gameObject, bool value, float delayTime, bool useRealTime = false)
    {
        if (useRealTime == true)
            yield return TimeExtensions.WaitForSecondsRealtime(delayTime);
        else
            yield return TimeExtensions.WaitForSeconds(delayTime);

        gameObject.SetActive(value);
    }
}