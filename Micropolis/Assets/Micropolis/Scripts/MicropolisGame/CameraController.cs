using System;
using Cinemachine;
using MicropolisCore;
using MicropolisEngine;
using MicropolisGame;
using UnityEngine;

/// <summary>
/// Controls the camera (both real and virtual) in the world allowing
/// the user to pan and zoom around the TileMap.
/// </summary>
public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;

    public float zoomSpeed = 20f;

    private MicropolisUnityEngine _engine;
    private Vector2 _lastMousePos;
    private CinemachineVirtualCamera _virtualCamera;
    private Camera _mainCamera;

    private void Start()
    {
        // get a reference to our game manager which holds the Micropolis engine
        _engine = FindObjectOfType<GameManager>().Engine;

        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _mainCamera = Camera.main;

        // setup the zoom for the camera and engine
        InitZoom();

        // start the player in the middle of the world
        var x = Micropolis.WORLD_W / 2;
        var y = Micropolis.WORLD_H / 2;
        transform.position = new Vector3(x, -y, 0);
    }

    private void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandleZoom()
    {
        var zoom = _engine.zoom;

        // TODO use input mapping instead of hard coded key values here
        if (Input.GetKey(KeyCode.X) && zoom <= MicropolisUnityEngine.MIN_ZOOM ||
            Input.GetAxis("Mouse ScrollWheel") < 0 && zoom <= MicropolisUnityEngine.MIN_ZOOM)
        {
            // zoom out
            zoom += 1f * Time.deltaTime * zoomSpeed;
        }
        else if (Input.GetKey(KeyCode.Z) && zoom >= MicropolisUnityEngine.MAX_ZOOM ||
                 Input.GetAxis("Mouse ScrollWheel") > 0 && zoom >= MicropolisUnityEngine.MAX_ZOOM)
        {
            // zoom in
            zoom -= 1f * Time.deltaTime * zoomSpeed;
        }
        
        // if the zoom value changed enough then update the (virtual) camera size
        if (Math.Abs(zoom - _engine.zoom) > float.Epsilon)
        {
            _virtualCamera.m_Lens.OrthographicSize = zoom;
            _engine.zoom = zoom;
        }
    }

    private void InitZoom()
    {
        _engine.zoom = _virtualCamera.m_Lens.OrthographicSize;
    }

    private void HandlePan()
    {
        // save the current position in case we go outside the bounds of the map
        var currentPosition = transform.position;

        // pan the map with the mouse or keyboard
        HandleMousePan();
        HandleKeyboardPan();

        // if the player goes outside the bounds of the map reset their position
        if (transform.position.x <= 0 || transform.position.y >= 0 ||
            transform.position.x >= Micropolis.WORLD_W ||
            transform.position.y <= -Micropolis.WORLD_H)
        {
            transform.position = currentPosition;
        }
    }

    private void HandleKeyboardPan()
    {
        // get the new translation based on horizontal and vertical input
        var translation = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        // update the player transform based on our panning speed and current frame
        transform.position += translation * panSpeed * Time.deltaTime;
    }

    private void HandleMousePan()
    {
        // get the current mouse position for comparison
        Vector2 currentMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            // mouse was clicked in this frame so not dragging yet
        }
        else if (Input.GetMouseButton(0))
        {
            // mouse button still down so let's move
            transform.Translate(_lastMousePos - currentMousePos);
            currentMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        // update the last position based on the new current position
        _lastMousePos = currentMousePos;
    }
}