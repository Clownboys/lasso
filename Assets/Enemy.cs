using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip popupSound;
    public AudioClip spinSound;
    public AudioClip grabbedSound;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.Play("EnemyPopup");
    }

    public void PopupEvent()
    {
        sound.clip = popupSound;
        sound.Play();
    }

    public void SpinEvent()
    {
        sound.clip = spinSound;
        sound.Play();
    }

    public void Whip()
    {
        anim.Play("EnemySpin");
    }
}
