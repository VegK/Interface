using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
	public const string ITEMS_PATH = "Items\\";
	public const string ITEM_FILE_EXTENSION = ".item";

	#region Properties
	#region Public
	public static LibraryController Instance
	{
		get
		{
			if (_instance == null)
				throw new Exception(string.Format("На сцене отсутствует объект с компонентом \"{0}\".", typeof(LibraryController)));
			return _instance;
		}
	}
	public GameObject Content;
	public GameObject PrefabCell;
	public GameObject PrefabItem;
	#endregion
	#region Private
	private static LibraryController _instance;
	private List<CellController> _cells;
	private List<Item> _items;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Поменять местами предметы в ячейках.
	/// </summary>
	public void SwapItemsInCell(CellController from, CellController to)
	{
		if (from == null || to == null)
			return;

		if (from.Item != null)
			from.Item.transform.SetParent(to.transform, false);
		if (to.Item != null)
			to.Item.transform.SetParent(from.transform, false);

		var tempItem = to.Item;
		to.Item = from.Item;
		from.Item = tempItem;
	}

	/// <summary>
	/// Создать клона указанного предмета в указанной ячейки.
	/// </summary>
	/// <param name="cell">Ячейка в которой будет создан клон.</param>
	/// <param name="item">Предмет для клонирования.</param>
	public void CreateCloneItem(CellController cell, ItemController item)
	{
		var obj = Instantiate(item.gameObject);
		obj.transform.SetParent(cell.transform, false);

		var ctrl = obj.GetComponent<ItemController>();
		cell.Item = ctrl;
	}
	#endregion
	#region Private
	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		var ctrl = PrefabCell.GetComponent<CellController>();
		if (ctrl == null)
		{
			Debug.Log(string.Format("Отсутствует компонент \"{0}\" у префаба клетки.", typeof(CellController)));
			return;
		}

		LoadItemsFromFiles();
		FillLibrary();

		var size = _items.Count;
        var cellSize = PrefabCell.GetComponent<RectTransform>().sizeDelta;
		var contentSize = Content.GetComponent<RectTransform>().sizeDelta;
		var columns = Mathf.Floor(contentSize.x / cellSize.x);
		var lines = Mathf.Ceil(size / columns);
		var newHeight = cellSize.y * lines;

		// Меняем параметры подложки.
		if (Content.GetComponent<RectTransform>().sizeDelta.y < newHeight)
		{
			contentSize.y = newHeight;
			Content.GetComponent<RectTransform>().sizeDelta = contentSize;
		}
		var pos = Content.transform.localPosition;
		pos.y = -contentSize.y / 2;
		Content.transform.localPosition = pos;

		// TODO: пустая ячейка для тестов
		CreateCell("FreeCell");
	}
	/// <summary>
	/// Загрузить информацию о предметах из файлов.
	/// </summary>
	private void LoadItemsFromFiles()
	{
		_items = new List<Item>();
        var path = Application.dataPath + "\\" + ITEMS_PATH;
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
		_cells = new List<CellController>();

		var path = Application.dataPath + "\\" + ITEMS_PATH + "\\";
		var i = 1;
		foreach (Item itemLib in _items)
		{
			if (!File.Exists(path + itemLib.FileNameImage))
				continue;

			var cell = CreateCell("Cell" + i);
			var item = CreateItem(itemLib);
			item.ProduceClone = true;

			item.transform.SetParent(cell.transform, false);
			cell.Item = item;
			i++;
		}
	}
	/// <summary>
	/// Создать объект ячейки инвентаря.
	/// </summary>
	/// <param name="name">Название ячейки.</param>
	/// <returns>Класс созданной ячейки.</returns>
	private CellController CreateCell(string name)
	{
		var cell = Instantiate(PrefabCell);
		cell.name = name;
		cell.transform.SetParent(Content.transform);

		var cellCtrl = cell.GetComponent<CellController>();
		_cells.Add(cellCtrl);
		return cellCtrl;
	}
	/// <summary>
	/// Создать объект предмета на основании класса предмета.
	/// </summary>
	/// <param name="item">Класс предмета.</param>
	/// <returns>Класс созданного предмета.</returns>
	public ItemController CreateItem(Item item)
	{
		var path = Application.dataPath + "\\" + ITEMS_PATH + "\\";

		var obj = Instantiate(PrefabItem);
		obj.name = item.Name;
		obj.transform.position = Vector2.zero;

		var bytes = File.ReadAllBytes(path + item.FileNameImage);
		var tex2d = new Texture2D(1, 1);
		tex2d.LoadImage(bytes);

		var itemCtrl = obj.GetComponent<ItemController>();
		itemCtrl.ImageItem.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero);

		itemCtrl.Type = (ItemType)item.Type;
		itemCtrl.Title = item.Name;
		itemCtrl.Description = item.Description;
		return itemCtrl;
    }
	#endregion
	#endregion
}