using System;
using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour
{
	#region Properties
	#region Public
	public static LogController Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<LogController>();
			if (_instance == null)
				throw new Exception(string.Format("На сцене отсутствует объект с компонентом \"{0}\".", typeof(LogController)));
			return _instance;
		}
	}

	public Text Text;
	public Scrollbar ScrollBar;
	#endregion
	#region Private
	private static LogController _instance;

	private bool _newString;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Добавить строку в лог.
	/// </summary>
	/// <param name="value">Строка.</param>
	public void AddString(string value)
	{
		Text.text = value + "\r\n" + Text.text;
		_newString = true;
	}

	public void OnValueChaned(float data)
	{
		if (_newString)
		{
			ScrollBar.value = 1;
			_newString = false;
		}
	}
	#endregion
	#region Private
	private void Start()
	{
		Text.text = string.Empty;
	}
	#endregion
	#endregion
}