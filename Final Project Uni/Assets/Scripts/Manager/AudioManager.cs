using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region Variables

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource, musicSource;
    [SerializeField] private AudioClip hit;

    [Header("Scene Music")] [SerializeField]
    private AudioClip[] sceneMusic;

    #endregion

    #region UnityMethod

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SceneMusic(0); //test only
    }

    #endregion

    #region Events

    public void HitEffect()
    {
        audioSource.clip = hit;
        audioSource.Play();
    }
    public void SceneMusic(int sceneNumber)
    {
        musicSource.clip = sceneMusic[sceneNumber];
        musicSource.Play();
    }

    #endregion

    #region Ults

    [FoldoutGroup("Event Test")]
    [Button]
    public void playSound()
    {
        HitEffect();
    }

    [FoldoutGroup("Event Test")]
    [Button]
    public void playSceneMusic(int sceneNumber)
    {
        SceneMusic(sceneNumber);
    }

    #endregion
}
