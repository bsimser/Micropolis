using MicropolisCore;
using UnityEngine;

public class MovePlayerCamera : MonoBehaviour
{
    public float speed = 1.0f;

    // Use this for initialization
    private void Start()
    {
        int x = Micropolis.WORLD_W / 2;
        int y = Micropolis.WORLD_H / 2;
        transform.position = new Vector3(x, -y, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        // get the new transform basd on horizontal and vertical input
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        // save the current position in case we go outside the bounds of the map
        var currentPosition = transform.position;
        // update the player transform based on our speed
        transform.position += move * speed * Time.deltaTime;
        // if the player goes outside the bounds of the map put them back to where they were
        if (transform.position.x <= 0 || transform.position.y >= 0 || transform.position.x >= 120 ||
            transform.position.y <= -100)
        {
            transform.position = currentPosition;
        }
    }
}