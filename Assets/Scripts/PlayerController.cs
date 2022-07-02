using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameInput))]
public class PlayerController : MonoBehaviour
{
    private GameInput _input;

    private void Awake()
    {
        _input = GetComponent<GameInput>();
    }
}
