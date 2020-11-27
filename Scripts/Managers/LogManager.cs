using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LogManager : MonoBehaviour {

	private static List<string> listLog = new List<string>();
	private const bool DEBUG_ENABLED = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Writes a string to the log
    /// </summary>
    /// <param name="s">The string</param>
	public static void Write(string s)
	{
		if(DataObject.WEBBUILD) return;
		if(DEBUG_ENABLED) print(s);
		listLog.Add(s);
		WriteToFile();
	}

    /// <summary>
    /// Writes the log to a .txt file
    /// </summary>
	public static void WriteToFile()
	{
		var path = GetPath();
		File.WriteAllLines(path, listLog.ToArray());
	}

    /// <summary>
    /// Returns the filepath of the log.txt file
    /// </summary>
    /// <returns>The filepath</returns>
	static string GetPath()
	{
		return Application.dataPath + "/log.txt";
	}
}
