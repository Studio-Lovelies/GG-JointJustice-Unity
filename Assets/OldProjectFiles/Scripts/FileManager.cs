using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour 
{
	/// <summary>
	/// The root save path for ALL data
	/// </summary>
	/// <value>The sav path.</value>
	public static string savPath 
	{
		get 
		{
			#if UNITY_EDITOR
			return "Assets/";
			#else
			return Application.persistentDataPath + "/";
			#endif
		}
	}	

	/// <summary>
	/// The save path for all data generated at runtime.
	/// </summary>
	/// <value>The data path.</value>
	public static string dataPath
	{
		get
		{
			return savPath + "data/";
		}
	}

	public static string fileExtension = ".txt";

	/// <summary>
	/// Separates the filename from the containing directory and returns the directory path.
	/// </summary>
	/// <returns>The directory from path.</returns>
	/// <param name="filePath">File path.</param>
	public static string GetDirectoryFromPath(string filePath)
	{
		string directoryPath = "";
		string[] parts = filePath.Split('/');
		foreach(string part in parts)
		{
			if (!part.Contains("."))
				directoryPath += part + "/";
		}
		return directoryPath;
	}

	/// <summary>
	/// Will return true if the directory already exists or was created. Returns false if the directory could not be made.
	/// </summary>
	/// <returns><c>true</c>, if create directory from path was tryed, <c>false</c> otherwise.</returns>
	/// <param name="path">Path.</param>
	public static bool TryCreateDirectoryFromPath(string path)
	{
		string directoryPath = path;

		if (Directory.Exists(path) || File.Exists(path)) return true;
		if (path.Contains("."))
		{
			directoryPath = GetDirectoryFromPath (path);
			if (Directory.Exists(directoryPath)) return true;
		}

		if (directoryPath != "" && !directoryPath.Contains("."))
		{
			print (directoryPath);
			try
			{
				Directory.CreateDirectory(directoryPath);
				return true;
			}
			catch (System.Exception e)
			{
				Debug.LogError ("Could not create Directory!\nERROR DETAILS: " + e.ToString ());
				return false;
			}
		}
		else
		{
			Debug.LogError("Directory was invalid - " + directoryPath + "\npath="+path + "\ndirectoryPath="+directoryPath);
			return false;
		}
	}

	/// <summary>
	/// Takes a file path and injects the default path if needed, and appends the default extension if needed.
	/// </summary>
	/// <returns>The correct file path.</returns>
	/// <param name="filePath">File path.</param>
	public static string AttemptCorrectFilePath(string filePath)
	{
		//make sure we add the default save path if desired.
		filePath = filePath.Replace("[]", dataPath);
		//add the default extension if no extension is present.
		if (!filePath.Contains(".")) filePath += fileExtension;

		return filePath;
	}

	public static void SaveFile(string filePath, string line)
	{
		SaveFile(filePath, new List<string>(){line});
	}

	/// <summary>
	/// Save a file with the specified lines	
	/// </summary>
	public static void SaveFile(string filePath, List<string> lines)
	{
		filePath = AttemptCorrectFilePath(filePath);

		//If the directory does not exist, try to create it. Prevent continuation if path was not valid.
		if (!TryCreateDirectoryFromPath(filePath))
		{
			Debug.LogError ("FAILED TO SAVE FILE [" + filePath + "] Please see console/log for details.");
			return;
		}

		StreamWriter sw = new StreamWriter(filePath);
		int i = 0;
		for (i = 0; i < lines.Count; i++)
		{
			sw.WriteLine(lines[i]);
		}

		sw.Close();

		print("Saved " + i.ToString() + " lines to file [" +filePath+"]");
	}

	/// <summary>
	/// Converts an array of strings into a list of the same values.
	/// </summary>
	/// <returns>The to list.</returns>
	/// <param name="array">Array.</param>
	/// <param name="removeBlankLines">If set to <c>true</c> remove blank lines.</param>
	static List<string> ArrayToList(string[] array, bool removeBlankLines = true)
	{
		List<string> list = new List<string>();
		for(int i = 0; i < array.Length; i++)
		{
			string s = array[i];
			if (s.Length > 0 || !removeBlankLines)
			{
				list.Add(s);
			}
		}
		return list;
	}

	/// <summary>
	/// Reads the data from the file at this path and returns a list of lines.
	/// </summary>
	/// <returns>The of file.</returns>
	/// <param name="filePath">File path.</param>
	public static List<string> LoadFile(string filePath, bool removeBlankLines = true)
	{
		filePath = AttemptCorrectFilePath(filePath);

		if (File.Exists(filePath))
		{
			List<string> lines = ArrayToList(File.ReadAllLines(filePath), removeBlankLines);
			return lines;
		}
		else
		{
			string errorMessage = "ERR! File "+filePath+" does not exist!";
			Debug.LogError(errorMessage);
			return new List<string>(){errorMessage};
		}
	}

	/// <summary>
	/// Takes a class and saves every public variable in that class regardless of whether it is serializable or not. 
	/// Allows for saving of non serializable variables such as colors, vectors, quaternions, sprites, textures, audio clips, etc. 
	/// Saves anything by converting it into a string that JSON can read at a later time.
	/// </summary>
	/// <param name="filePath">File path.</param>
	/// <param name="serializableClassToSave">Serializable class to save.</param>
	public static void SaveJSON(string filePath, object classToSave)
	{
		string jsonString = JsonUtility.ToJson(classToSave);
		SaveFile(filePath, jsonString);
	}

	/// <summary>
	/// Load a class from a JSON file by converting every string to its proper value. 
	/// Loads non serializable objects such as colors, vectors, quaternions, sprites, textures, audio clips, etc.
	/// </summary>
	/// <returns>The JSO.</returns>
	/// <param name="filePath">File path.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T LoadJSON<T>(string filePath)
	{
		string jsonString = LoadFile(filePath)[0];
		return JsonUtility.FromJson<T>(jsonString);
	}
}