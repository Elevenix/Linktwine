using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Transform sawTransform;

    [SerializeField]
    private float forceSawToPlayer = 200f;
    [SerializeField]
    private GameObject particuleForceSaw;

    private Camera _cam;
    private Rigidbody2D _rb;
    private LineRenderer _line;
    // Start is called before the first frame update
    void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _rb = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // set the position of the player
        _line.SetPosition(0, this.transform.position);
        _line.SetPosition(1, sawTransform.position);
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _rb.position = new Vector3(mousePos.x,mousePos.y,0);

        // the saw got attracted toward the mouse when mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(particuleForceSaw, sawTransform.position, Quaternion.identity);
            Rigidbody2D sawRB = sawTransform.gameObject.GetComponent<Rigidbody2D>();
            sawRB.velocity = Vector2.zero;
            sawRB.AddForce((this.transform.position - sawTransform.position).normalized*forceSawToPlayer);
        }
    }
}
