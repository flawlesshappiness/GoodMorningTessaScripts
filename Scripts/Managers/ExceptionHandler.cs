using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionHandler {

	private static MenuException menuException;
	private static bool isSetup = false;

    /// <summary>
    /// Sets up the exception handling
    /// </summary>
    /// <param name="menu">The exception menu</param>
	public static void SetupExceptionHandling(MenuException menu)
	{
		if(isSetup) return;
		isSetup = true;

		ExceptionHandler.menuException = menu;

		Application.logMessageReceived += delegate(string condition, string stackTrace, LogType type) {
			HandleException(condition, stackTrace, type);
		};
	}

    /// <summary>
    /// Called when an exception is thrown, logs it, and shows the exception menu
    /// </summary>
    /// <param name="condition">The condition</param>
    /// <param name="stackTrace">The stacktrace</param>
    /// <param name="type">The LogType</param>
	static void HandleException(string condition, string stackTrace, LogType type)
	{
		LogManager.Write("");
		LogManager.Write(string.Concat(type));
		LogManager.Write(string.Concat("CONDITION: ", condition));
		LogManager.Write("STACKTRACE:");
		LogManager.Write(stackTrace);

		if(type == LogType.Exception)
		{
			LogManager.WriteToFile();
			menuException.SetText("ERROR: Exception occured", stackTrace);
			menuException.Show();
		}
	}
}
