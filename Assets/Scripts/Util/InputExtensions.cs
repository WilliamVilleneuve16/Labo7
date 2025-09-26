using UnityEngine;

public static class InputExtensions
{
    public static bool IsPressed(this KeyCode[] keys)
    {
        for (var i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i])) return true;
        }
        return false;
    }
    
    public static bool IsPressedDown(this KeyCode[] keys)
    {
        for (var i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i])) return true;
        }
        return false;
    }
}