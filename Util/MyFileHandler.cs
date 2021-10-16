using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyFileHandler
{
    private static string fileExtension = ".stars";

    public static bool ReadFile(GameFile gameFile, out string data, bool autoCreateIfMissing = false, string defaultData = "")
    {
        Debug.Log("LOADING: " + GetGameFilePath(gameFile));
        data = "";
        if (!File.Exists(MyFileHandler.GetGameFilePath(gameFile)))
            return MyFileHandler.FileMissing(gameFile, autoCreateIfMissing, defaultData);
        StreamReader reader = new StreamReader(MyFileHandler.GetGameFilePath(gameFile));
        data = reader.ReadToEnd();
        reader.Close();
        return true;
    }

    public static bool FileExists(GameFile gameFile) {
        if (!File.Exists(MyFileHandler.GetGameFilePath(gameFile)))
            return false;
        return true;
    }

    private static bool FileMissing(GameFile gameFile, bool autoCreateIfMissing, string defaultData) {
        if (autoCreateIfMissing)
            MyFileHandler.WriteFile(gameFile, defaultData);
        return false;
    }

    public static void WriteFile(GameFile gameFile, string serializedData)
    {
        StreamWriter writer = new StreamWriter(MyFileHandler.GetGameFilePath(gameFile), false);
        writer.Write(serializedData);
        writer.Flush();
        writer.Close();
    }

    public static string GetGameFilePath(GameFile gameFile)
    {
        return Application.persistentDataPath + MyFileHandler.GameFileToString(gameFile);
    }

    public static string GameFileToString(GameFile gameFile) {
        return "/" + gameFile.ToString() + MyFileHandler.fileExtension;
    }
}

public enum GameFile
{
    SAVE, CONFIG,
    CONTENT
}

[System.Serializable]
public class SaveListContainer
{
    public object dataList;

    public SaveListContainer(object dataList)
    {
        //Cause json is object oriented
        this.dataList = dataList;
    }
}