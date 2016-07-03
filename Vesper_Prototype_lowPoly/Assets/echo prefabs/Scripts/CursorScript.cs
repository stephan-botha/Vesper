using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour {

    private bool cursorHide;

    void Start()
    {
        cursorHide = true;
        UpdateCursor();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorHide = false;
            UpdateCursor();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            cursorHide = true;
            UpdateCursor();
        }
    }

    void UpdateCursor()
    {
        if (cursorHide)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}﻿