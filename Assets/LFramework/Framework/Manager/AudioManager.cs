/****************************************************
	文件：AudioManager.cs
	作者：XWL
	邮箱:  <2263007881@qq.com>
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LFramework
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioListener _audioListener;
        private Dictionary<string, AudioSource> _sourceDictionary = new Dictionary<string, AudioSource>();

        private void Awake()
        {
            _audioListener = GetComponent<AudioListener>();
        }

        public void CheckAudioListener()
        {
            
            if (_audioListener == null)
            {
                _audioListener = gameObject.AddComponent<AudioListener>();
            }
        }

        public void PlaySound(string soundName)
        {
            CheckAudioListener();

            var clip = Resources.Load<AudioClip>(soundName);

            if (_sourceDictionary.ContainsKey(soundName))
            {
                var audioSource = _sourceDictionary[soundName];

                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                _sourceDictionary.Add(soundName, audioSource);

                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private AudioSource _audioMusicSource;

        public void PlayMusic(string musicName, bool looping = true)
        {
            CheckAudioListener();
            if (!_audioMusicSource)
            {
                _audioMusicSource = gameObject.AddComponent<AudioSource>();
            }

            var clip = Resources.Load<AudioClip>(musicName);
            _audioMusicSource.clip = clip;
            _audioMusicSource.loop = looping;
            _audioMusicSource.Play();
        }

        public void StopMusic()
        {
            _audioMusicSource.Stop();
        }

        public void PauseMusic()
        {
            _audioMusicSource.Pause();
        }

        public void ResumeMusic()
        {
            _audioMusicSource.UnPause();
        }

        public void MusicOff()
        {
            _audioMusicSource.Pause();
            _audioMusicSource.mute = true;
        }

        public void SoundOff()
        {
            var soundSources = GetComponents<AudioSource>();

            foreach (var soundSource in soundSources)
            {
                if (soundSource != _audioMusicSource)
                {
                    soundSource.Pause();
                    soundSource.mute = true;
                }
            }
        }

        public void MusicOn()
        {
            _audioMusicSource.UnPause();
            _audioMusicSource.mute = false;
        }

        public void SoundOn()
        {
            var soundSources = GetComponents<AudioSource>();

            foreach (var soundSource in soundSources)
            {
                if (soundSource != _audioMusicSource)
                {
                    soundSource.UnPause();
                    soundSource.mute = false;
                }
            }
        }

        public void StopSound(string musicName)
        {
            _sourceDictionary[musicName].Stop();
            
        }
    }
}