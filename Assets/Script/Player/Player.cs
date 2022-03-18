using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("----------Ability----------")]
    [SerializeField] float jumpPower=1;
    [SerializeField] float moveSpeed=1;
    [Header("----------BodyState----------")]
    // [SerializeField] GameObject currentBody;
    [SerializeField] GameObject[] bodyObjs;
    [Header("----------AnimState----------")]
    [SerializeField] PlayerAutoAnimation currentAnim;
    [SerializeField] PlayerAutoAnimation[] normalAnim;

    Rigidbody2D playerRigid;
    [SerializeField]bool isJump;
    [SerializeField]bool isMove;
    [SerializeField]bool isIdle;
    private void Awake()
    {
        Init();
        //    currentBody=bodyObjs[0];
        StartGame();
    }

    private void Init()
    {
        normalAnim = bodyObjs[0].GetComponentsInChildren<PlayerAutoAnimation>(true);
        playerRigid = GetComponent<Rigidbody2D>();
        AnimChange(normalAnim[0]);

    }

    void StartGame()
    {
        StartCoroutine(StartPlayerRoutine());
    }
    IEnumerator StartPlayerRoutine()
    {
        while (true)
        {
            
            MovePlayer();  
            MoveAnimPlayer();  
            yield return null;
        }
    }
    void MovePlayer()
    {
        Vector3 moveVelocity=Vector3.zero;
          if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            moveVelocity=Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            moveVelocity=Vector3.right;
            
        }
        // if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow))
        // {
        //     moveVelocity=Vector3.left;
        // }
        transform.position+=moveVelocity*moveSpeed*Time.deltaTime;
        Jump();
    }
    void Jump()
    {
        if (isJump)
            return;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isJump = true;
            isIdle=false;
            playerRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    void LandingPlayer()
    {
 
        isJump = false;
        IdleAnim();
        if ((Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow))&&!Input.GetKey(KeyCode.LeftAlt))
        {
            isIdle=false;
            currentAnim = normalAnim[1];
            AnimChange(currentAnim);
        }

 
    }
    void MoveAnimPlayer()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            currentAnim = normalAnim[2];
            AnimChange(currentAnim);
        }
        if (isJump)
            return;
        MoveAnim();
        IdleAnim();

    }
    void MoveAnim()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow))
        {
            isIdle=false;
            currentAnim = normalAnim[1];
            AnimChange(currentAnim);
        }

    }
    void IdleAnim()
    {
         if(!Input.anyKey&&!isIdle)
        {
            isIdle=true;
            currentAnim = normalAnim[0];
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            LandingPlayer();
        }
    }
}
