using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    [Header("----------Ability----------")]
    [SerializeField] float jumpPower = 1;
    [SerializeField] float moveSpeed = 1;
    [Header("----------BodyState----------")]
    // [SerializeField] GameObject currentBody;
    [SerializeField] internal GameObject[] bodyObjs;
    [Header("----------AnimState----------")]
    [SerializeField] PlayerAutoAnimation tempAnim;
    [SerializeField] PlayerAutoAnimation[] currentAnim;
    // [SerializeField]  PlayerAutoAnimation[] normalAnim;
    // [SerializeField]  PlayerAutoAnimation[] fullAnim;
    [Header("----------Base----------")]
    [SerializeField] GameObject baseModel;
    Rigidbody2D playerRigid;
    bool isJump;
    bool isMove;
    [SerializeField] bool isIdle;
    internal bool isAttack;
    bool isAlert;
    private void Awake()
    {
        Init();
        StartGame();
    }

    private void Init()
    {
        baseModel.SetActive(false);
        // normalAnim = bodyObjs[0].GetComponentsInChildren<PlayerAutoAnimation>(true);
        // fullAnim = bodyObjs[1].GetComponentsInChildren<PlayerAutoAnimation>(true);
        currentAnim = bodyObjs[0].GetComponentsInChildren<PlayerAutoAnimation>(true);
        playerRigid = GetComponent<Rigidbody2D>();
        AnimChange(currentAnim[0]);

    }
    void AnimChange(PlayerAutoAnimation onAnim)
    {
        foreach (var item in currentAnim)
        {
            item.gameObject.SetActive(false);
        }
        onAnim.gameObject.SetActive(true);
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
        Vector3 moveVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            moveVelocity = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            moveVelocity = Vector3.right;

        }
        // if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow))
        // {
        //     moveVelocity=Vector3.left;
        // }
        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        Jump();
    }
    void Jump()
    {
        if (isJump)
            return;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isJump = true;
            isIdle = false;
            playerRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
     void JumpAnim()
    {
        if (isJump)
            return;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            isJump = true;
            isIdle = false;
            playerRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    void MoveAnimPlayer()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //점프
            tempAnim = currentAnim[2];
            AnimChange(tempAnim);
        }
        if (isJump)
            return;
        MoveAnim();
        IdleAnim();
        AttackAnim();

    }
    void MoveAnim()
    {

       
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isIdle = false;
            // isMove=true;
            tempAnim = currentAnim[1];
            AnimChange(tempAnim);
        }
        
    }
    void IdleAnim()
    {
        if (!Input.anyKey && !isIdle && !isAttack)
        {
            isIdle = true;
            // isMove=false;
            tempAnim = currentAnim[0];
            if (isAlert)
            {
                tempAnim = currentAnim[5];
            }
            AnimChange(tempAnim);
            
        }

    }
    void AttackAnim()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isAttack = true;
            isIdle = false;
            int attackNum=Random.Range(6,9);
            tempAnim = currentAnim[attackNum];
            AnimChange(tempAnim);
        }

    }
    IEnumerator tempAlertAnimRoutine;
    IEnumerator AlertAnimRoutine()
    {
        print("코루틴실행");
        yield return TimeExtensions.WaitForSeconds(5f);
        isAlert = false;
        isIdle=false;
        // IdleAnim();

    }
    internal void AlertAnim()
    {
        isAlert = true;
        isAttack = false;
        tempAnim = currentAnim[5];
        AnimChange(tempAnim);
        if (tempAlertAnimRoutine != null)
        {
            StopCoroutine(tempAlertAnimRoutine);
        }
        tempAlertAnimRoutine = AlertAnimRoutine();
        StartCoroutine(tempAlertAnimRoutine);
    }

    void LandingPlayer()
    {

        isJump = false;
        IdleAnim();
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && !Input.GetKey(KeyCode.LeftAlt))
        {
            isIdle = false;
            tempAnim = currentAnim[1];
            AnimChange(tempAnim);
        }
    }
    internal void ChangeModel(int num)
    {
        foreach (var item in currentAnim)
        {
            item.gameObject.SetActive(false);
        }

        currentAnim = bodyObjs[num].GetComponentsInChildren<PlayerAutoAnimation>(true);
        AnimChange(currentAnim[0]);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            LandingPlayer();
        }
    }
}
