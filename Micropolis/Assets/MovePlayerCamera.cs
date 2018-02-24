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
        transform.position = new Vector3(x, y, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * speed * Time.deltaTime;
    }
}