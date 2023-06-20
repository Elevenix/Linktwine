using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField]
    private float minSpeed;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private bool canFollow = true;

    private float _speed;
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _target = FindObjectOfType<Movement>().transform;
        this.transform.up = _target.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canFollow && _target != null)
        {
            this.transform.up = _target.transform.position - this.transform.position;
        }

        this.transform.Translate(Vector3.up * Time.deltaTime * _speed);

        // si l objet part trop loin on le détruit
        if(this.transform.position.x < -100 ||
            this.transform.position.x > 100 ||
            this.transform.position.y < -100 ||
            this.transform.position.y > 100)
        {
            Destroy(this.gameObject);
        }
    }

}
