using System;
using UnityEngine;

public class PhysicsSettings : MonoBehaviour
{
    [SerializeField] private Vector3 gravity = new(0f, -9.81f, 0f);

    private void Awake()
    {
        Physics.gravity = gravity;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Physics.gravity = gravity;
        }
    }
#endif
}