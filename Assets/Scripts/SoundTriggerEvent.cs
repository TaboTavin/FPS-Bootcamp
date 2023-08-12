using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundTriggerEvent : MonoBehaviour
{
    public UnityEvent onPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(onPlayerEnter != null)
            {
                onPlayerEnter.Invoke();
            }
        }
    }

    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }
}
