using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;
    public bool reseted = false;

    // Fields
    public SaveState save;
    private const string saveFileName = "data.ss";
    private BinaryFormatter formatter;

    // Actions
    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;

    private void Awake()
    {
        instance = this;
        formatter = new BinaryFormatter();

        // Try and load the previous save state
        Load();
    }

    private void Update()
    {
        if (reseted)
            DeleteSaveFile();
    }

    public void Load()
    {
        try
        {
            string filePath = Application.persistentDataPath + saveFileName;
            Debug.Log("Loading from: " + filePath);

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            save = (SaveState)formatter.Deserialize(file);
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save file not found, let's create a new one!");
            Save();
        }
    }

    public void Save()
    {
        // If theres no previous state found, create a new one!
        if (save == null)
            save = new SaveState();

        // Set the time at which we've tried saving
        save.lastSaveDate = DateTime.Now;

        // Open a file on our system, and write to it
        FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);
    }

    public void DeleteSaveFile()
    {
        string filePath = Application.persistentDataPath + saveFileName;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file deleted: " + filePath);
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }
    }
}