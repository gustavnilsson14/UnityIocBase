using System;
using UnityEditor;

public class EnumUtil
{
    public static T GetEnumByRatio<T>(float ratio) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
        if (ratio == 0) return (T)Enum.ToObject(typeof(T), 0);
        float length = Enum.GetNames(typeof(T)).Length - 1;
        int index = (int)MathF.Ceiling(length * ratio);
        return (T)Enum.ToObject(typeof(T), index);
    }
    public static T NextInEnum<T>(T src) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
}