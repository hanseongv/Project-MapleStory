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
    [SerializeField] float attackCoolTime = 0.6f;
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
    [SerializeField]bool isJump;
    [SerializeField]bool isMove;
    [SerializeField]bool isIdle;
    [SerializeField]bool isDown;
    [SerializeField]internal bool isAttack;
    [SerializeField]bool isAlert;
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
        if (isDown||isAttack)
            return;
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
        AttackAnim();
        if (isJump)
            return;
        DownAnim();
        MoveAnim();
        IdleAnim();


    }
    void DownAnim()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isDown = true;
            // isIdle = false;
            // isMove=true;
            tempAnim = currentAnim[3];
            AnimChange(tempAnim);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isDown = false;
            isIdle = false;
            isMove = false;
            // isIdle=false;
            // // isMove=true;
            // tempAnim = currentAnim[3];
            // AnimChange(tempAnim);
        }
    }

    void MoveAnim()
    {

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            isMove = false;
            isIdle = false;
            // if(!Input.GetKey(KeyCode.DownArrow))
            isDown = false;
        }
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && !isMove)
        {
            isMove = true;
            isDown = false;
            // isIdle = false;
            tempAnim = currentAnim[1];
            AnimChange(tempAnim);
        }



    }
    void IdleAnim()
    {
        // if (!Input.anyKey && !isIdle && !isAttack)
        if (!isDown && !isIdle && !isAttack && !isJump && !isMove)
        {
            isIdle = true;
            // isMove=false;
            tempAnim = currentAnim[0];
            if (isAlert)
            {
                tempAnim = currentAnim[6];
            }
            AnimChange(tempAnim);

        }

    }
    void AttackAnim()
    {
        if (Input.GetKey(KeyCode.LeftControl)&&!isAttack)
        {
            

            isAttack = true;
            isIdle = false;
            int attackNum = Random.Range(7, 10);
            tempAnim = currentAnim[attackNum];
            if (Input.GetKey(KeyCode.DownArrow))
                tempAnim = currentAnim[4];
            AnimChange(tempAnim);

            if (tempAttackCoolTimeAnimRoutine != null)
            {
                StopCoroutine(tempAttackCoolTimeAnimRoutine);
            }
            tempAttackCoolTimeAnimRoutine = AttackCoolTimeAnimRoutine();
            StartCoroutine(tempAttackCoolTimeAnimRoutine);
        }
    }
    IEnumerator tempAttackCoolTimeAnimRoutine;
    IEnumerator AttackCoolTimeAnimRoutine()
    {
        float coolTime = 0f;
        print("1");
        while (coolTime<attackCoolTime)
        {
            coolTime += Time.deltaTime;
            yield return null;
        }
        print("2");
        isAttack = false;
        isMove=false;
        
    }
    IEnumerator tempAlertAnimRoutine;
    IEnumerator AlertAnimRoutine()
    {
        print("코루틴실행");
        yield return TimeExtensions.WaitForSeconds(5f);
        isAlert = false;
        isIdle = false;
        // IdleAnim();

    }
    internal void AlertAnim()
    {
        isAlert = true;
        // isAttack = false;


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
        if (isAttack)
            return;

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
