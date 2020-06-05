using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string MyEnemy; //this component is shared, so we need to specify who were're trying to attack and damage
    public string MyTeam;
    public float HealthDecrement = 0.015f;
    public int AttackRange = 3;
    private AudioSource attackSound;

    void Awake()
    {
        attackSound = gameObject.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(MyEnemy); //look for all enemy units

        foreach (GameObject g in units)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < AttackRange && g != null) //if they're nearby, reduce their health and run their damage "animation"
            {
                g.GetComponent<Health>().HealthLevel -= HealthDecrement;
                g.GetComponent<Health>().RunDamageAnimation();

                if (!attackSound.isPlaying)
                    TrySound();
            }
        }

    }

    private void TrySound()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(MyTeam); //look for all friendly units

        foreach (GameObject g in units)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < AttackRange && g.GetComponent<AudioSource>()) //if a friendly (with audio source) is within attack range it doesn't need to play it's own attack sound
            {
                if (g.GetComponent<AudioSource>().isPlaying) //stop if it is
                    return;
            }
        }//if we reach the end, play the sound

        attackSound.Play();
    }
}
