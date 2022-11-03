using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum Behaviour
{
    Idle,
    Patrol,
    Chase,
    Attack
}

public enum MonsterType
{
    Mushroom,
    Zombie
}

public class Monster : CollisionDamage
{
    public MonsterType monsterType = MonsterType.Mushroom;
    public int hp = 10;
    public int attackPower = 1;
    //public int collisionDamage = 2;

    SkinnedMeshRenderer meshRenderer;
    Rigidbody rigid;
    Animator anim;
    BoxCollider enemyCollider;

    [SerializeField]
    Behaviour monState = Behaviour.Idle;
    List<Vector3> patrolPoint = new List<Vector3>();
    bool isDead = false;

    NavMeshAgent navi;

    float checkDistance = 5;

    private DrawGizmos attackPivot;
    public LayerMask targerLayer; // 공격

    Transform target; // 피해
    ParticleSystem damagedParticle;

    float currentTime = 0;

    MushroomSpawner mushroomSpawner;

    UIQuest uiQuest;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        enemyCollider = GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        mushroomSpawner = GameObject.FindObjectOfType<MushroomSpawner>();

        target = GameObject.FindObjectOfType<Player>().transform;
        navi = GetComponent<NavMeshAgent>();

        Transform t = transform.Find("AttackPivot");
        if (t != null) attackPivot = t.GetComponent<DrawGizmos>();
        damagedParticle = GetComponentInChildren<ParticleSystem>();

        patrolPoint.Add(transform.position);
        patrolPoint.Add(transform.position + Vector3.back * 5);

        InitMonster();
        //StartCoroutine(IEPatrol());

        uiQuest = FindObjectOfType<UIQuest>();
    }

    void InitMonster()
    {
        SetCollisionDamage = 3;
    }


    private void Update()
    {
        currentTime += Time.deltaTime;
        
        if(currentTime > 0.25f)
        {
            if (isDead) return;
            currentTime = 0;

            if(monState == Behaviour.Idle || monState == Behaviour.Patrol)
                CheckPlayer();

            if(monState == Behaviour.Chase)
            {
                navi.SetDestination(target.position);
                
                
            }
        }

        if (GameData.isDead)
        { 
            navi.isStopped = true;
            StopPos();
            StopAllCoroutines();
            monState = Behaviour.Patrol;
            StartCoroutine("IEPatrol", 3f);
        }
    }

    IEnumerator IEPatrol()
    {
        if (!isDead)
        {
            navi.isStopped = false;
            int patrolCount = 0;
            while (true)
            {
                int pathNum = patrolCount % 2;
                navi.SetDestination(patrolPoint[pathNum]);
                patrolCount++;

                yield return new WaitForSeconds(5);
            }
        }
        yield return null;
        
    }

    void CheckPlayer()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if(distance <= checkDistance)
        {
            switch (monsterType)
            {
                case MonsterType.Mushroom:
                    AudioMng.Instance.PlayActionEffect("goblin_fairy_attack_01");
                    break;
                case MonsterType.Zombie:
                    AudioMng.Instance.PlayActionEffect("zombie_voice_grunt_22");
                    break;
            }
            monState = Behaviour.Chase;
        }
            
        
    }

    void CheckDamage()
    {
        Collider[] enemy = Physics.OverlapBox(attackPivot.transform.position, attackPivot.size, Quaternion.identity, targerLayer);
        if (enemy != null)
        {
            for (int i = 0; i < enemy.Length; ++i)
                enemy[i].SendMessage("SetDamage", attackPower, SendMessageOptions.DontRequireReceiver);

        }
    }

    public void SetDamage(int damage) 
    {
        if (isDead) return;
        int result = 0 - damage;
        if (result < 0)
        {
            damagedParticle.Play();
            
            switch(monsterType)
            {
                case MonsterType.Mushroom:
                    AudioMng.Instance.PlayActionEffect("damage_hurt_mushroom");
                    break;
                case MonsterType.Zombie:
                    AudioMng.Instance.PlayActionEffect("troll_monster_hurt_pain_short_02");
                    break;
            }
            AudioMng.Instance.PlayActionEffect("damage_impact_mushroom");

            hp += result;
            if (hp <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(HitColorAnimation());
                Reaction();
                anim.SetTrigger("Damage");
            }
        }
    }

    void Die()
    {
        isDead = true;
        rigid.velocity = Vector3.zero;
        StopAllCoroutines();
        SetCollisionDamage = 0;
        anim.SetTrigger("Die2");
        
        switch (monsterType)
        {
            case MonsterType.Mushroom:
                AudioMng.Instance.PlayActionEffect("goblin_fairy_death_07");
                mushroomSpawner.mushroomCount--;
                uiQuest.KillMushroom();
                break;
            case MonsterType.Zombie:
                AudioMng.Instance.PlayActionEffect("ghost_witch_voice_hiss_03");
                uiQuest.KillZombie();
                break;
        }
         
        StartCoroutine(DieAlphaAnimation());
        navi.enabled = false;
        
        Destroy(gameObject, 5f);
    }

    private IEnumerator HitColorAnimation()
    {
        Color color = meshRenderer.material.color;
        color = new Color(1,1,0);
        meshRenderer.material.color = color;
        yield return new WaitForSeconds(0.1f);
        color = new Color(1, 1, 1);
        meshRenderer.material.color = color;
    }

    private IEnumerator DieAlphaAnimation()
    {
        yield return new WaitForSeconds(2);
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime;
            Color color = meshRenderer.material.color;
            color.b = Mathf.Lerp(1, 0, percent);
            color.g = Mathf.Lerp(1, 0, percent);
            meshRenderer.material.color = color;

            yield return null;
        }
    }

    void Reaction()
    {
        navi.isStopped = true;
        rigid.velocity = Vector3.zero;
        Vector3 react = transform.position - target.position;
        react=react.normalized;
        react += Vector3.up;
        rigid.AddForce(react * 2, ForceMode.Impulse);
        Invoke("StopPos", 1f);
    }

    void StopPos()
    {
        if (isDead) return;
        rigid.velocity = Vector3.zero;
        navi.isStopped = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }

}

