using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set 
{
    public  List<object> set = new();
    public bool Add(Vector3 element)
    {

        if (!set.Contains(element))
        {
            set.Add(element);
            return true;
        }
        else
        {
            return false;
        }
    }
}
