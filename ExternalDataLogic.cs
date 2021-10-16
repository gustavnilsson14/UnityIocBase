using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ExternalDataLogic : InterfaceLogicBase
{
    public static ExternalDataLogic I;
    public static JObject data;

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    private void LoadData()
    {
        JsonSerializer serializer = new JsonSerializer();

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        MyFileHandler.ReadFile(GameFile.CONTENT, out string data, true);
        ExternalDataLogic.data = JObject.Parse(data);
    }

    public static JToken GetExternalData(string path)
    {
        return ExternalDataLogic.data.SelectToken(path);
    }

    public static JToken GetExternalDataList(string type) {
        return ExternalDataLogic.data.SelectToken($"{type}");
    }

    public static void ApplyDataToGameobject(BehaviourBase target, JToken jToken) {
        target.name = (string)jToken["name"];
        Dictionary<string, object> variables = jToken["variables"].ToObject<Dictionary<string, object>>();
        foreach (string key in variables.Keys)
        {
            object value = variables[key];
            if (value is JObject)
                value = GetComplexValue(value as JObject);
            ApplyPrimitiveValue(target, key, value);
        }
        if (jToken["children"] == null)
            return;
        List<JToken> children = jToken["children"].ToObject<List<JToken>>();
        foreach (JToken childToken in children)
        {
            ApplyDataToChildType(target, childToken);
        }
    }

    private static void ApplyDataToChildType(BehaviourBase target, JToken childToken)
    {
        Dictionary<string, object> childTypes = (childToken["types"] as JToken).ToObject<Dictionary<string, object>>();
        List<BehaviourBase> children = target.GetComponentsInChildren(Type.GetType((string)childToken["component"])).Cast<BehaviourBase>().ToList();
        foreach (string key in childTypes.Keys)
        {
            ApplyDataToGameobject(children.Find(x => x.gameObject.name == key), (JToken)childTypes[key]);
        }
    }

    private static object GetComplexValue(JObject o)
    {
        object result = "";
        switch ((string)o["type"])
        {
            case "range":
                float stepSize = 0.1f;
                if (o["stepSize"] != null)
                    stepSize = (float)o["stepSize"];
                result = RandomUtil.RandomFloat((float)o["min"], (float)o["max"]);
                int numSteps = Mathf.FloorToInt((float)result / stepSize);
                return (float)numSteps * stepSize;
            case "intrange":
                int intStepSize = 1;
                if (o["stepSize"] != null)
                    intStepSize = (int)o["stepSize"];
                result = RandomUtil.RandomInt((int)o["min"], (int)o["max"]);
                numSteps = Mathf.FloorToInt((int)result / intStepSize);
                return (int)numSteps * intStepSize;
        }
        return result;
    }

    private static void ApplyPrimitiveValue(BehaviourBase target, string key, object value)
    {
        if (!ReflectionUtil.GetFieldInfo(out FieldInfo fieldInfo, target, key)) {
            return;
        }
        fieldInfo.SetValue(target, value);
    }
}

public interface IExternalDataInstance {
    string path { get; set; }
}