using Newtonsoft;
using Newtonsoft.Json;
using QFSW.QC;
using System;
using System.IO;
using UnityEngine;
using System.Reflection;

public class SaveManager : Singleton<SaveManager>
{
    public delegate void SaveCallback();
    public delegate void LoadCallback();
    public event SaveCallback OnSave;
    public event LoadCallback OnLoad;

    [SerializeField] private string version = "v0.0.1";

    public void SaveData<T>(string file, T data)
    {
        if (data is SaveVar e)
            e.Version = version;

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, file + ".json");

        File.WriteAllText(path, json);
    }

    public void LoadData<T>(string file, T obj)
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + file + ".json"))
        {
            string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + file + ".json");
            JsonConvert.PopulateObject(json, obj);
        }
    }

    public void SetData<T>(string field, string data, T obj)
    {
        Type type = obj.GetType();

        FieldInfo fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);

        if (fieldInfo == null)
        {
            Debug.Log(field + " does not exist!");
            return;
        }

        object value = Convert.ChangeType(data, fieldInfo.FieldType);

        fieldInfo.SetValue(obj, value);
    }

    public void ReadData<T>(T obj)
    {
        Type type = obj.GetType();

        FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        for (int i = 0; i < fieldInfo.Length; i++)
        {
            Debug.Log(fieldInfo[i].Name + ": " + fieldInfo[i].GetValue(obj));
        }
    }

    [Command] public void Load() => OnLoad?.Invoke();
    [Command] public void Save() => OnSave?.Invoke();
}