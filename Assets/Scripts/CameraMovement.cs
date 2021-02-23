using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public PlayerControlls playerControlls;
    private Vector2 movementVector;
    private int maxSpeed = 5000;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        playerControlls = new PlayerControlls();
        playerControlls.Enable();

        playerControlls.KeyboardMouse.Move.performed += context => movementVector = context.ReadValue<Vector2>();

        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(movementVector.x * maxSpeed * Time.deltaTime, 0f, movementVector.y * maxSpeed * Time.deltaTime);
    }
}
