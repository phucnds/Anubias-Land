using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetMouseScreenPoint()
    {
        return Input.mousePosition;
    }

    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector3 inputMoveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = 1f;
        }

        return inputMoveDir;
    }

    public float GetCameraRotateAmount()
    {
        float rotateAmount = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount += 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount -= 1f;
        }

        return rotateAmount;
    }

    public float GetCameraZoomAmout()
    {
        float zoomAmout = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmout = -1f;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmout = 1f;
        }

        return zoomAmout;
    }

}
