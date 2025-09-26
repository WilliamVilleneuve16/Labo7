using UnityEngine;

public static class ArrayExtensions
{
    public static T Random<T>(this T[] elements)
    {
        return elements[UnityEngine.Random.Range(0, elements.Length)];
    }
}