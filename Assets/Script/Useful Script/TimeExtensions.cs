using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://forum.unity.com/threads/c-coroutine-waitforseconds-garbage-collection-tip.224878/
public static class TimeExtensions
{
    class FloatComparer : IEqualityComparer<float> {
        bool IEqualityComparer<float>.Equals (float x, float y) {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode (float obj) {
            return obj.GetHashCode();
        }
    }
    static Dictionary<float, WaitForSeconds> timeInterval = new Dictionary<float, WaitForSeconds>(100, new FloatComparer());
    static Dictionary<float, WaitForSecondsRealtime> realTimeInterval = new Dictionary<float, WaitForSecondsRealtime>(100, new FloatComparer());
    static WaitForEndOfFrame endOfFrameInterval = new WaitForEndOfFrame();
    static WaitForFixedUpdate fixedUpdateInterval = new WaitForFixedUpdate();

    public static WaitForEndOfFrame     WaitForEndOfFrame {get => endOfFrameInterval;}
    public static WaitForFixedUpdate    WaitForFixedUpdate { get => fixedUpdateInterval; }
    public static WaitForSeconds        WaitForSeconds(float seconds) {
        WaitForSeconds wfs;
        if(!timeInterval.TryGetValue(seconds, out wfs))
            timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds) {
        WaitForSecondsRealtime wfsr;
        if(!realTimeInterval.TryGetValue(seconds, out wfsr))
            realTimeInterval.Add(seconds, wfsr = new WaitForSecondsRealtime(seconds));
        return wfsr;
    }
}
