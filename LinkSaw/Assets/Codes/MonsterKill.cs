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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.GM.endGame();
        }

        if (collision.gameObject.CompareTag("Saw"))
        {
            pv--;
            if (canEject)
            {
                ejectSaw(collision);
            }
            animator.SetTrigger("eject");

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

    private void ejectSaw(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - this.transform.position) * forceEject);
    }
}
