using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensions : MonoBehaviour
{
    public static void DestroyOtherInstances<T>(T obj) where T : MonoBehaviour
    {
        T[] others = FindObjectsOfType<T>();
        for (int i = 0; i < others.Length; i++)
        {
            GameObject other = others[i].gameObject;
            if (other != obj.gameObject) Destroy(other);
        }
    }

    public static bool CheckIfAlreadyExists<T>(T obj) where T : MonoBehaviour
    {
        T[] others = FindObjectsOfType<T>();
        for (int i = 0; i < others.Length; i++)
        {
            if (others[i] != obj) return true;
        }

        return false;
    }

}
