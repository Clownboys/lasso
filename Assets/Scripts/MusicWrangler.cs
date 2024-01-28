using CollabXR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicWrangler : SingletonBehavior<MusicWrangler>
{
    public enum MusicState { Loading, LobbyIntro, LobbyLoop, WaveIntro, WaveLoop }
    public enum FadeState { Steady, FadingOut, FadingIn };
    public MusicState state = MusicState.Loading;
    public FadeState fadeState;
    public AudioClip lobbyIntro, lobbyLoop, waveIntro, waveLoop;
    public AudioSource music;
    public float maxVolume = 1.0f;
    public float fadeSpeed = 0.1f;
    AudioClip nextClip;
    public void CheckState(GameWrangler.GameState oldState, GameWrangler.GameState newState)
    {
        Debug.Log(oldState.ToString() + " " + newState.ToString());
        if (oldState != newState) // actually a new state
        {
            if (oldState == GameWrangler.GameState.Loading && newState == GameWrangler.GameState.Lobby)
            {
                ChangeState(MusicState.LobbyIntro);
            }
            else if (newState == GameWrangler.GameState.Lobby)
            {
                ChangeState(MusicState.LobbyLoop);
            }
            else if (newState == GameWrangler.GameState.Started)
            {
                ChangeState(MusicState.WaveIntro);
            }
        }
    }

    public void ChangeState(MusicState newState)
    {
        Debug.Log(newState);
        this.state = newState;
        if (this.state == MusicState.LobbyIntro)
        {
            music.loop = false;
            if (music.clip == null)
            {
                music.clip = lobbyIntro;
                fadeState = FadeState.Steady;
                music.volume = maxVolume;
                music.Play();
            }
            else
            {
                nextClip = lobbyIntro;
                fadeState = FadeState.FadingOut;
            }
        }
        else if (this.state == MusicState.LobbyLoop)
        {
            music.clip = lobbyLoop;
            music.loop = true;
            music.Play();
            fadeState = FadeState.Steady;
        }
        else if (this.state == MusicState.WaveIntro)
        {
            music.loop = false;
            if (music.clip == null)
            {
                music.clip = waveIntro;
                fadeState = FadeState.Steady;
                music.volume = maxVolume;
                music.Play();
            }
            else
            {
                nextClip = waveIntro;
                fadeState = FadeState.FadingOut;
            }
        }
        else if (this.state == MusicState.WaveLoop)
        {
            music.clip = waveLoop;
            music.loop = true;
            music.Play();
            fadeState = FadeState.Steady;
        }
    }
    private void Update()
    {
        if (this.state != MusicState.Loading)
        {
            if (fadeState == FadeState.FadingOut)
            {
                music.volume -= Time.deltaTime* fadeSpeed;
                if (music.volume <= 0)
                {
                    music.clip = nextClip;
                    music.Play();
                    fadeState = FadeState.FadingIn;
                }
            }
            else if (fadeState == FadeState.FadingIn)
            {
                music.volume += Time.deltaTime*fadeSpeed;
                if (music.volume >= maxVolume)
                {
                    fadeState = FadeState.Steady;
                }
            }
            Debug.Log(music.time + " " + music.clip.length);
            if (!music.isPlaying && fadeState == FadeState.Steady)
            {
                if (this.state == MusicState.LobbyIntro)
                {
                    ChangeState(MusicState.LobbyLoop);
                }
                else if (this.state == MusicState.WaveIntro)
                {
                    ChangeState(MusicState.WaveLoop);
                }
            }
        }
    }
}
