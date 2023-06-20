using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public GameObject Saw;

    private Camera _cam;
    private Rigidbody2D _rb;
    private LineRenderer _line;

    // Start is called before the first frame update
    void Start()
    {
        // get the camera in the scene
        _cam = FindObjectOfType<Camera>();

        // get the component rigidbody2D on the player
        _rb = GetComponent<Rigidbody2D>();

        // get the lineRenderer component
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // take the mouse position
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        // move the player in the mouse position (z = 0, because mousePos.z = -10)
        _rb.position = new Vector3(mousePos.x, mousePos.y, 0);

        _line.SetPosition(0, this.transform.position);
        _line.SetPosition(1, Saw.transform.position);
    }
}
