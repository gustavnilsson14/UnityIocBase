using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class ReflectionUtil
{
    public static bool CopyObjectValues(object original, object target) {
        if (!GetStoredObject(out StoredObject storedObject, original))
            return false;
        if (!ApplyStoredObject(storedObject, target))
            return false;
        return true;
    }
    public static bool GetStoredObject(out StoredObject storedObject, object original, Dictionary<string, string> id = null)
    {
        storedObject = null;
        if (original == null)
            return false;
        storedObject = new StoredObject(original, id);
        return true;
    }
    public static List<T> GetAllOfType<T>(object original)
    {
        List<T> result = new List<T>();
        foreach (FieldInfo fieldInfo in GetFieldInfos(original))
        {
            if (fieldInfo.FieldType != typeof(T))
                continue;
            result.Add((T)fieldInfo.GetValue(original));
        }
        return result;
    }
    public static bool ApplyStoredObject(StoredObject storedObject, object target) {
        if (storedObject.objectType != target.GetType()) 
            return false;
        storedObject.ApplyTo(target);
        return true;
    }
    public static Dictionary<string, object> GetDict(object target) {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        foreach (FieldInfo fieldInfo in GetFieldInfos(target))
        {

        }
        return dict;
    }
    /*
    public static void ApplyDict(Dictionary<string, object> dict, object target)
    {
        foreach (string key in dict.Keys)
        {
            FieldInfo fieldInfo = GetBackingFieldInfo(target, key);
            if (fieldInfo == null)
                continue;
            if (!dict.TryGetValue(key, out object value))
                continue;
            ApplyObject(fieldInfo, target, value);
        }
    }
    */
    public static bool GetFieldInfo(out FieldInfo fieldInfo, object target, string key) {

        fieldInfo = target.GetType().GetField(key, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (fieldInfo != null)
            return true;
        fieldInfo = target.GetType().GetField($"<{key}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        return fieldInfo != null;
    }

    private static void ApplyObject(FieldInfo fieldInfo, object target, object value)
    {
        if (int.TryParse(value as string, out int intValue))
        {
            fieldInfo.SetValue(target, intValue);
            return;
        }
        if (value is JObject)
        {
            fieldInfo.SetValue(target, GetComplexValue(value as JToken));
            return;
        }
    }

    private static object GetComplexValue(JToken o)
    {
        switch ((string)o["type"])
        {
            case "range":
                float stepSize = 0.1f;
                if (o["stepSize"] != null)
                    stepSize = (float)o["stepSize"];
                float result = RandomUtil.RandomFloat((float)o["min"], (float)o["max"]);
                int numSteps = Mathf.FloorToInt(result / stepSize);
                return (float)numSteps * stepSize;
        }
        return "";
    }

    public static bool IsSubClassOrClass<T>(Type t) {
        if (t.IsSubclassOf(typeof(T)) || t == typeof(T))
            return true;
        return false;
    }

    public static Dictionary<string,string> StoredObjectToDict(StoredObject storedObject)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        storedObject.values.ForEach(x => result.Add(x.fieldName,x.valueString));
        return result;
    }

    public static List<Type> GetAllImplementationsOfInterface<T>()
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(myType => myType.GetInterfaces().Contains(typeof(T))).ToList();
    }
    public static FieldInfo[] GetFieldInfos(object original, BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default)
    {
        Type objectType = original.GetType();
        return objectType.GetFields(flags);
    }
}
[System.Serializable]
public class StoredObject
{
    public Dictionary<string, string> id;
    public Type objectType;
    public string objectTypeName;
    public List<StoredObjectValue> values;
    public StoredObject(object original, Dictionary<string, string> id) {
        this.objectType = original.GetType();
        this.objectTypeName = original.GetType().ToString();
        this.id = id;
        values = new List<StoredObjectValue>();
        foreach (FieldInfo fieldInfo in ReflectionUtil.GetFieldInfos(objectType))
        {
            object value = fieldInfo.GetValue(original);
            if (!IsValidValueForCopy(value))
                continue;
            values.Add(new StoredObjectValue(fieldInfo.Name, value));
        }
    }
    public void ApplyTo(object target)
    {
        foreach (StoredObjectValue storedObjectValue in values)
        {
            FieldInfo fieldInfo = objectType.GetField(storedObjectValue.fieldName);
            if (fieldInfo == null)
                continue;
            fieldInfo.SetValue(target, storedObjectValue.value);
        }
    }
    public bool IsValidValueForCopy(object value)
    {
        if (value is string)
            return true;
        if (value is bool)
            return true;
        if (value is float)
            return true;
        if (value is int)
            return true;
        if (value is StoredObject)
            return true;
        if (value is List<StoredObject>)
            return true;
        if (value is Enum)
            return true;
        return false;
    }
}
[System.Serializable]
public class StoredObjectValue
{
    public string fieldName;
    public object value;
    public string valueString;
    public StoredObjectValue(string fieldName, object value)
    {
        this.fieldName = fieldName;
        this.value = value;
        this.valueString = value.ToString();
    }
}