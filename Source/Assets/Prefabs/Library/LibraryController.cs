using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LibraryController : BaseInventory
{
	public const string ITEM_FILE_EXTENSION = ".item";

	#region Properties
	#region Public
	public static LibraryController Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<LibraryController>();
			if (_instance == null)
				throw new Exception(string.Format("На сцене отсутствует объект с компонентом \"{0}\".", typeof(LibraryController)));
			return _instance;
		}
	}
	#endregion
	#region Private
	private static LibraryController _instance;
	private List<Item> _items;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Получить базовый предмет по индексу.
	/// </summary>
	/// <param name="index">Индекс базового предмета.</param>
	/// <returns>Базовый предмет, если не удалось найти, то возвращает NULL.</returns>
	public Item GetItem(int index)
	{
		return _items.Find(i => i.Index == index);
	}
	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;
		_items = new List<Item>();
		LoadItemsFromFiles();
	}

	protected override void Start()
	{
		base.Start();
		FillLibrary();
		SetHeightContent(_items.Count);
	}

	/// <summary>
	/// Загрузить информацию о предметах из файлов.
	/// </summary>
	private void LoadItemsFromFiles()
	{
		_items.Clear();

		var path = Application.dataPath + "\\" + Parameters.ITEMS_PATH;
		if (!Directory.Exists(path))
			return;

		try
		{
			var dir = new DirectoryInfo(path);
			var ser = new XmlSerializer(typeof(Item));

			foreach (FileInfo file in dir.GetFiles("*" + ITEM_FILE_EXTENSION, SearchOption.AllDirectories))
				if (file.Extension == ITEM_FILE_EXTENSION)
					using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
					{
						var item = ser.Deserialize(fs) as Item;
						if (item != null)
							_items.Add(item);
						fs.Close();
					}
		}
		catch { }
	}

	/// <summary>
	/// Заполнить ячейки библиотеки предметами.
	/// </summary>
	private void FillLibrary()
	{
		// Очистка контента от объектов ячеек.
		var childrens = Content.GetComponentsInChildren<CellController>();
		foreach (var child in childrens)
			Destroy(child.gameObject);
		Cells.Clear();

		var path = Application.dataPath + "\\" + Parameters.ITEMS_PATH + "\\";
		var i = 1;
		foreach (Item itemLib in _items)
		{
			if (!File.Exists(path + itemLib.FileNameImage))
				continue;

			var cell = CreateCell("Cell" + i, CellType.Standart);
			var item = CreateItem(itemLib);

			if (SetItemInCell(item, cell))
			{
				item.ProduceClone = true;
				i++;
			}
			else
			{
				Destroy(item.gameObject);
			}
		}
	}
	#endregion
	#endregion
}