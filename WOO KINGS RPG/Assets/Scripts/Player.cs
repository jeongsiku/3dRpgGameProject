using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp = 10;
    public int currentHp = 0;
    float hpRegentic = 0;

    public int attackPower = 10;
    public int defensePower = 10;

    int maxATKPoint = 100;
    int maxDEFPoint = 100;
    int maxHPPoint = 100;

    SkinnedMeshRenderer meshRenderer;
    Animator anim;
    Rigidbody rigid;

    public LayerMask targerLayer; // АјАн
    private DrawGizmos attackPivot;
    ParticleSystem attackParticle;
    ParticleSystem getItemParticle;

    bool isDamaged = false;

    GameManager gameManager;
    UIManager uiManager;
    MapManager mapManager;

    public void Init()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        Transform t = transform.Find("AttackPivot");
        if (t != null) attackPivot = t.GetComponent<DrawGizmos>();

        t = transform.Find("AttackPivot/AttackParticle");
        if(t != null) attackParticle = t.GetComponent<ParticleSystem>();

        t = transform.Find("GetItemParticle");
        if (t != null) getItemParticle = t.GetComponent<ParticleSystem>();


        meshRenderer.sharedMaterial.color = new Color(1, 1, 1);

        gameManager = GameObject.FindObjectOfType<GameManager>();

        uiManager = GameObject.FindObjectOfType<UIManager>();
        if(uiManager != null) uiManager.Init();

        mapManager = GameObject.FindObjectOfType<MapManager>();

        DontDestroyOnLoad(gameObject);
        InitCharInfo();
    }

    void InitCharInfo()
    {
        currentHp = maxHp;
        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        uiManager.SetPlayerMaxHp(maxHp);
        uiManager.SetPlayerHp(currentHp);

        uiManager.SetCurrentPlayerATKPoint(attackPower);
        uiManager.SetMaxPlayerATKPoint(maxATKPoint);
        uiManager.SetCurrentPlayerDEFPoint(defensePower);
        uiManager.SetMaxPlayerDEFPoint(maxDEFPoint);
        uiManager.SetCurrentPlayerHPPoint(maxHp);
        uiManager.SetMaxPlayerHPPoint(maxHPPoint);

        uiManager.RefreshGold();
    }

    public void SetItemStat(int addAtk = 0, int delAtk = 0, int addDef = 0, int delDef = 0, int addHp = 0, int delHp = 0 )
    {
        attackPower += addAtk;
        attackPower -= delAtk;
        defensePower += addDef;
        defensePower -= delDef;
        maxHp += addHp;
        maxHp -= delHp;
        if(currentHp > maxHp)
            currentHp = maxHp;
        UpdateStatUI();
    }

    

    void CheckDamage()
    {
        attackParticle.Play();
        Collider[] enemy = Physics.OverlapBox(attackPivot.transform.position, attackPivot.size, Quaternion.identity, targerLayer);
        if (enemy != null)
        {
            for( int i = 0; i< enemy.Length; ++ i )
                enemy[i].SendMessage("SetDamage", attackPower, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SetDamage(int damage)
    {
        if (isDamaged) return;
        if (GameData.isDead) return;

        int result = 0 - damage;
        if (result < 0)
        {
            currentHp += result;
            if (currentHp <= 0)
            {
                
                Die();
            }
            else
            {
                
                Hit();
            }
        }
    }

    void Die()
    {
        StartCoroutine(HitColorAnimation(1));
        GameData.isDead = true;
        rigid.velocity = Vector3.zero;

        uiManager.SetPlayerHp(0);
        rigid.isKinematic = true;
        Collider coll = GetComponent<Collider>();
        if (coll != null) coll.enabled = false;


        anim.SetTrigger("Die2");
        StartCoroutine(DieColorAnimation());
        Invoke("CallRespawn", 5f);
    }

    void CallRespawn()
    {
        StartCoroutine(IERespawnPlayer());
    }

    void Hit()
    {
        isDamaged = true;
        StartCoroutine(HitColorAnimation(3));
        anim.SetTrigger("Damage");
        AudioMng.Instance.PlayActionEffect("voice_male_c_effort_short_jump_04");
        uiManager.SetPlayerHp(currentHp);
        Invoke("IsDamagedOut", 1f);
    }

    void IsDamagedOut()
    {
        isDamaged = false;
    }

    private IEnumerator HitColorAnimation(int count)
    {
        Color color = meshRenderer.sharedMaterial.color;
        int cnt = 0;
        while(cnt < count)
        {
            color = new Color(1, 0, 0);
            meshRenderer.sharedMaterial.color = color;
            yield return new WaitForSeconds(0.1f);
            
            color = new Color(1, 1, 1);
            meshRenderer.sharedMaterial.color = color;
            yield return new WaitForSeconds(0.1f);

            cnt++;
            
        }
    }

    private IEnumerator DieColorAnimation()
    {
        yield return new WaitForSeconds(2);
        float percent = 0;
        while (percent < 2)
        {
            percent += Time.deltaTime;
            Color color = meshRenderer.sharedMaterial.color;
            color.b = Mathf.Lerp(1, 0, percent);
            color.g = Mathf.Lerp(1, 0, percent);
            meshRenderer.sharedMaterial.color = color;

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            CollisionDamage collisionDamage = collision.gameObject.GetComponent<CollisionDamage>();
            
            if (GameData.isDead) return;
            if (isDamaged) return;
            if (collisionDamage.GetCollisionDamage == 0) return;

            currentHp -= collisionDamage.GetCollisionDamage;
            
            if(currentHp <= 0)
            {
                Die();
            }
            else
            {
                Hit();
                Reaction(collision);
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        FieldItem item = other.gameObject.GetComponent<FieldItem>();
        switch (other.gameObject.tag)
        {
            
            case "LowLine":
                StartCoroutine(IERespawnPlayer());
                break;

            case "Heart":
                currentHp += item.HealPoint;
                if(currentHp > maxHp)
                {
                    currentHp = maxHp;
                }
                uiManager.SetPlayerHp(currentHp);
                AudioMng.Instance.PlayActionEffect("getheart");
                getItemParticle.Play();
                Destroy(item.gameObject);
                break;
            case "HpUp":
                maxHp += item.MaxHeartUp;
                currentHp += item.MaxHeartUp;
                if(currentHp > maxHp) currentHp = maxHp;
                uiManager.SetPlayerMaxHp(maxHp);
                uiManager.SetPlayerHp(currentHp);
                AudioMng.Instance.PlayActionEffect("maxHeartUp");
                getItemParticle.Play();
                Destroy(item.gameObject);
                break;
            case "Coin":
                GameData.gold += item.Coin;
                uiManager.RefreshGold();
                AudioMng.Instance.PlayActionEffect("retro_collect_pickup_coin_25");
                getItemParticle.Play();
                StartCoroutine(IERespawnCoin(item));
                break;
        }
    }

    
    IEnumerator IERespawnCoin(FieldItem item)
    {
        item.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        item.gameObject.SetActive(true);
    }

    IEnumerator IERespawnPlayer()
    {
        //gameManager.FadeOut();
        yield return new WaitForSeconds(1.2f);
        switch(GameData.currentQuest)
        {
            case 1:
                transform.position = mapManager.StartPoint1;
                break;
            case 2:
                transform.position = mapManager.StartPoint2;
                break;
            case 3:
                transform.position = mapManager.StartPoint3;
                break;
            case 4:
                transform.position = mapManager.StartPoint4;
                break;
        }
        
        
        Color color = meshRenderer.sharedMaterial.color;
        color = new Color(1, 1, 1);
        meshRenderer.sharedMaterial.color = color;
        anim.SetTrigger("Reset");
        
        transform.rotation = Quaternion.identity;
        //gameManager.FadeIn();
    }


    void Reaction(Collision collision)
    {
        Vector3 react = transform.position - collision.transform.position;
        react = react.normalized;
        react += Vector3.up;
        rigid.AddForce(react * 3f, ForceMode.Impulse);
    }

    private void LateUpdate()
    {
        if (GameData.isDead) return;
        hpRegentic += Time.deltaTime;

        if(hpRegentic > 5f)
        {
            hpRegentic = 0;
            if (currentHp < maxHp)
            {
                currentHp++;
                UpdateStatUI();
            }
                
        }
    }
}
