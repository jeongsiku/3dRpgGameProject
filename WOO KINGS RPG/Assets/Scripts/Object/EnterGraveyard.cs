using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGraveyard : MonoBehaviour
{
    public float dropTime = 0;
    Animator animator;

    AudioSource audioSource;
    AudioClip swingClip;
    AudioClip dropClip;

    private void Start()
    {
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(DropCall());
    }

    IEnumerator DropCall()
    {
        yield return new WaitForSeconds(dropTime);
        animator.SetTrigger("Drop");
    }

    void TrapDrop()
    {
        swingClip = Resources.Load<AudioClip>("Sounds/TrapDrops");
        audioSource.clip = swingClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    
    void TrapDropped()
    {
        dropClip = Resources.Load<AudioClip>("Sounds/TrapDropped");
        audioSource.clip = dropClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
}
