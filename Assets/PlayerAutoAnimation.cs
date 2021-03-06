using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAnimation : MonoBehaviour
{
    [SerializeField] Transform[] animationObjs;
    [SerializeField] float animationTime = 0.5f;
    [SerializeField] internal int animationNum;
    [SerializeField] bool notRepeated;
    [SerializeField] bool onAlert;
    Player player;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        Transform[] tempObjs = GetComponentsInChildren<Transform>(true);
        animationObjs = new Transform[tempObjs.Length - 1];
        for (int i = 1; i < tempObjs.Length; i++)
        {
            animationObjs[i - 1] = tempObjs[i];
        }

    }
    private void OnEnable()
    {
        animationNum=0;
        StartCoroutine(AnimRoutine());
        
    }
    // internal void AnimStart()
    // {
    //     StartCoroutine(AnimRoutine());
    // }
    IEnumerator AnimRoutine()
    {
        while (true)
        {
            AnimObjSetActive(animationObjs, animationObjs[animationNum]);

            yield return TimeExtensions.WaitForSeconds(animationTime);

            animationNum++;

            if (animationObjs.Length - 1 < animationNum)
            {
                animationNum = 0;
                if (notRepeated)
                {
                    if (onAlert)
                    {                       
                        player.AlertAnim();
                    }
                    break;
                }

            }
        }
    }
    void AnimObjSetActive(Transform[] offObjs, Transform onObj)
    {
        foreach (var item in offObjs)
        {
            item.gameObject.SetActive(false);
        }
        onObj.gameObject.SetActive(true);
    }
}
