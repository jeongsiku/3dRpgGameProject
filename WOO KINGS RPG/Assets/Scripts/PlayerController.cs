using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    UIPlayerAction uiPlayerAction;

    Transform player;
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    float playerSpeed = 3; // 캐릭터이동속도
    float jumpPower = 5;
    public float camRotSpeed = 50;

    public Camera mainCamera;
    public GameObject cameraPoint;

    Animator anim;
    Rigidbody rigid;

    bool jDown = false;
    bool aDown = false;
    bool fDown = false;
    bool escDown = false;
    bool iDown = false;
    bool cDown = false;

    bool isJump = false;
    bool isAttack = false;
    
    public GameObject scanObject = null;


    public void Init()
    {
        gameManager = FindObjectOfType<GameManager>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        
        uiPlayerAction = GetComponentInChildren<UIPlayerAction>();
        if (uiPlayerAction != null) uiPlayerAction.Init();

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        aDown = Input.GetMouseButtonDown(0);
        fDown = Input.GetButtonDown("Interaction");
        escDown = Input.GetButtonDown("Cancel");
        iDown = Input.GetButtonDown("Inventory");
        cDown = Input.GetButtonDown("CharPanel");
    }

    void Move()
    {
        if(!GameData.isDead && !GameData.isUI && !GameData.isFreeze)
        {
            moveVec = Vector3.forward * vAxis + Vector3.right * hAxis;
            transform.Translate(moveVec.normalized * Time.deltaTime * playerSpeed, Space.Self);
            anim.SetBool("Run", moveVec != Vector3.zero);
            if ( hAxis > 0)
            {
                anim.SetBool("Left", false);
                anim.SetBool("Right", true);
            }
            else if (hAxis < 0)
            {
                anim.SetBool("Left", true);
                anim.SetBool("Right", false);
            }
            else
            {
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
            }
            
            if(moveVec!= Vector3.zero)
                CheckHarvesting();
        }
    }

    void Turn()
    {
        if (!GameData.isDead && !GameData.isUI && !GameData.isFreeze)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * camRotSpeed * Input.GetAxis("Mouse X"));
        }
    }

    void Jump()
    {
        if (jDown && !isJump && !GameData.isDead && !GameData.isUI && !GameData.isFreeze)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            isJump = true;
            AudioMng.Instance.PlayActionEffect("voice_male_c_effort_short_jump_04");

            CheckHarvesting();
        }
    }

    void Attack()
    {
        if(!isAttack && aDown && !GameData.isDead && !GameData.isUI && !GameData.isFreeze)
        {
            isAttack = true;
            anim.SetTrigger("Atk2");
            Invoke("AttackSound", 0.2f);
            Invoke("OutIsAttack", 0.6f);

            CheckHarvesting();
        }

        
    }

    void OutIsAttack()
    {
        isAttack = false;
    }

    void AttackSound()
    {
        AudioMng.Instance.PlayActionEffect("whoosh_weapon_knife_swing_01");
    }

    void Interaction()
    {
        if(fDown && !GameData.isDead && !GameData.isUI && !GameData.isFreeze && scanObject != null)
        {
            gameManager.Interaction(scanObject);
        }
    }

    void OpenInventory()
    {
        if(iDown)
        {
            gameManager.OpenInventory();
        }
    }

    void OpenCharPanel()
    {
        if(cDown)
        {
            gameManager.OpenCharPanel();
        }
    }

    void Cancel()
    {
        if(escDown && GameData.isMenu)
        {
            gameManager.OpenMenu(false);
            return;
        }

        if (escDown && !GameData.isUI && !GameData.isMenu)
        {
            gameManager.OpenMenu(true);
            return;
        }

        if (escDown && !GameData.isFreeze)
        {
            gameManager.DeleteScanObject();
            if (GameData.isUI) GameData.isUI = false;
            gameManager.CloseQuestPanel();
            gameManager.CloseShopUI();
            GameData.isInteractView = false;

            CheckHarvesting();
            return;
        }
        
        
    }

    void ScanObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 0.5f, out hit))
        {
            ScanableObject obj = hit.transform.gameObject.GetComponent<ScanableObject>();
            if(obj != null)
            {
                scanObject = hit.transform.gameObject;
                if (scanObject.tag == "ItemBox")
                    scanObject?.GetComponent<Renderer>().sharedMaterial.SetFloat("_show", 1);
            }
            else
            {
                if(scanObject != null)
                {
                    if (scanObject.tag == "ItemBox")
                        scanObject?.GetComponent<Renderer>().sharedMaterial.SetFloat("_show", 0);
                    scanObject = null;
                }
            }
        }
        else
        {
            scanObject = null;
        }
    }

    void CheckHarvesting()
    {
        if(GameData.isHarvesting)
        {
            gameManager.StopHarvesting();
        }
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        ScanObject();
        Interaction();
        OpenInventory();
        OpenCharPanel();
        Cancel();
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    uISystemMsg.Print("테스트중");
        //}
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
}
