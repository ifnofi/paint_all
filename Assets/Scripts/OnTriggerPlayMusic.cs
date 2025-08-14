using System;
using UnityEngine;

public class OnTriggerPlayMusic : MonoBehaviour
{
    private AudioSource getaudioSource;

    private void Start()
    {
        getaudioSource = GetComponent<AudioSource>();
        if (getaudioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            getaudioSource.Play();
            
       
        }
    }
}
