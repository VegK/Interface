﻿using UnityEngine;
using System.Collections;
using System;

public class Parameters : MonoBehaviour
{
	public const string ITEMS_PATH = "Items\\";

	#region Properties
	#region Public
	[Header("Prefabs")]
	public CellController PrefabCell;
	public ItemController PrefabItem;

	[Header("Static objects")]
	public ToolTipController ToolTip;
	public Canvas MainCanvas;

	[Space(10)]
	public Rarity RarityColor;

	public static Parameters Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<Parameters>();
			if (_instance == null)
				throw new Exception(string.Format("На сцене отсутствует объект с компонентом \"{0}\".", typeof(Parameters)));
			return _instance;
		}
	}
	#endregion
	#region Private
	private static Parameters _instance;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;

		ToolTip.gameObject.SetActive(false);
	}
	#endregion
	#endregion

	[Serializable]
	public class Rarity
	{
		public Color Rare = Color.blue;
		public Color Epic = Color.magenta;
		public Color Legendary = new Color(1, 0.5f, 0);
	}
}