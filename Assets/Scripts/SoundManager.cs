using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ImmortalSingletonBehaviour<SoundManager> {

    public AudioSource sfx;

    public AudioClip resourceCollected;
    public AudioClip disasterEncounter;
    public AudioClip disasterAverted;
    public AudioClip disasterHit;
    public AudioClip prayerStarted;
    public AudioClip prayerCompleted;
    public AudioClip gameOver;
    public AudioClip buttonClick;


    public void ResourceCollected() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(resourceCollected, 1f);
    }

    public void DisasterEncounter() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(disasterEncounter, 1f);
    }

    public void DisasterAverted() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(disasterAverted, 1f);
    }

    public void DisasterHit() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(disasterHit, 1f);
    }

    public void PrayerStarted() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(prayerStarted, 1f);
    }

    public void PrayerCompleted() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(prayerCompleted, 1f);
    }

    public void GameOver() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(gameOver, 1f);
    }

    public void ButtonClick() {
        if (!GameManager.Instance.gameOver) sfx.PlayOneShot(buttonClick, 1f);
    }

}
