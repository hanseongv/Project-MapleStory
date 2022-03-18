using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;

    int changeModelNum;
    // private void Awake() 
    // {
  
    // }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            changeModelNum++;
            if(player.bodyObjs.Length<=changeModelNum)
                changeModelNum=0;
            player.ChangeModel(changeModelNum);
        }
    }
}
