using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ListExtensions {
   public static List<U> TryCastAll<T, U>(this List<T> list)
   {
        return list
            .Where(selected => selected is U)
            .ToList().Cast<U>().ToList();
    }
}

class Base { }
class Test<T, U>
    where U : struct
    where T : Base, new()
{ }