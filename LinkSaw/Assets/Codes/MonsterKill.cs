using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKill : MonoBehaviour
{
    [SerializeField]
    private int scoreNumber = 100;

    [SerializeField]
    private bool canEject = false;
    [SerializeField]
    private float forceEject = 1000;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float timeShakeEject = 0.05f;
    [SerializeField]
    private float magnitudeShakeEject = 0.05f;
    [SerializeField]
    private float timeShakeDead = 0.05f;
    [SerializeField]
    private float magnitudeShakeDead = 0.05f;
    [SerializeField]
    private Sound.SoundPlayer soundEject;


    [SerializeField]
    private int pv = 1;

    [SerializeField]
    private GameObject particuleDeath;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // player touched and die
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.GM.endGame();
        }

        // monster touched the saw
        if (collision.gameObject.CompareTag("Saw"))
        {
            // monster lose one life point
            pv--;
            // the monster eject the saw when touched
            if (canEject)
            {
                ejectSaw(collision);
            }
            animator.SetTrigger("eject");

            // when the monster had no more life points
            if (pv <= 0)
            {
                GameManager.GM.addScore(scoreNumber,timeShakeDead, magnitudeShakeDead);
                Instantiate(particuleDeath, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else
            {
                soundEject.StartSounds();
                StartCoroutine(GameManager.GM.Shake(timeShakeEject, magnitudeShakeEject));
            }

        }
    }

    /// <summary>
    /// Eject the saw from the monster (this)
    /// </summary>
    /// <param name="collision"> the object to eject </param>
    private void ejectSaw(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - this.transform.position) * forceEject);
    }
}
