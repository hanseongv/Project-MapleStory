using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("----------BodyState----------")]
    // [SerializeField] GameObject currentBody;
    [SerializeField] GameObject[] bodyObjs;
    [Header("----------AnimState----------")]
    [SerializeField] PlayerAutoAnimation currentAnim;
    [SerializeField] PlayerAutoAnimation[] normalAnim;

   private void Awake() 
   {
       Init();
    //    currentBody=bodyObjs[0];
       StartGame();
   }

    private void Init()
    {
        normalAnim=bodyObjs[0].GetComponentsInChildren<PlayerAutoAnimation>(true);
 
        AnimChange(normalAnim[0]);    
   
    }

    void StartGame()
    {
        StartCoroutine(StartPlayerRoutine());
    }
    IEnumerator StartPlayerRoutine()
    {
        while(true)
        {
            MovePlayer();
            yield return null;
        }
    }

    void MovePlayer()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentAnim=normalAnim[1]; 
            transform.rotation=Quaternion.Euler(0,0,0);
            AnimChange(currentAnim);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentAnim=normalAnim[1]; 
            transform.rotation=Quaternion.Euler(0,-180,0);
            AnimChange(currentAnim);
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow)||Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentAnim=normalAnim[0];       
            AnimChange(currentAnim);     
        }
        
    }
    void AnimChange(PlayerAutoAnimation onAnim)
    {
        foreach (var item in normalAnim)
        {
            item.gameObject.SetActive(false);
        }
        onAnim.gameObject.SetActive(true);
        // onAnim.AnimStart();
    }
}
