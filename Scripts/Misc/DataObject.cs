using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class DataObject {

	public static DataObject save;

	//LEVELS

	//VOLUME
	public float volumeMaster = 1f;
	public float volumeMusic = 1f;
	public float volumeFx = 1f;

	//SETTINGS
	public const bool WEBBUILD = true;

	private DataObject ()
	{
		
	}
    
    /// <summary>
    /// Returns a new DataObject
    /// </summary>
    /// <returns>The DataObject</returns>
	public static DataObject New()
	{
		return new DataObject();
	}

    /// <summary>
    /// Returns
    /// </summary>
    /// <returns></returns>
	private static string GetPath()
	{
		return Application.persistentDataPath + "/save.dat";
	}

	/// <summary>
	/// Saves this data object to file.
	/// </summary>
	public void Save()
	{
		if(WEBBUILD) return; //Don't save if not allowed
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(GetPath());
		bf.Serialize(file, this);
		file.Close();
	}

	/// <summary>
	/// Loads data from save file.
	/// </summary>
	public static DataObject Load()
	{
		if(WEBBUILD) return new DataObject();
			
		if(File.Exists(GetPath()))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(GetPath(), FileMode.Open);
			DataObject data = (DataObject)bf.Deserialize(file);
			file.Close();
			//
			return data;
		}
		else
		{
			return DataObject.New();
		}
	}
}