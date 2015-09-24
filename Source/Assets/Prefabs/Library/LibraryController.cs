using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
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

	public int Size = 10;
	#endregion
	#region Private
	private static LibraryController _instance;
	private List<CellController> _cells;
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

		var cellSize = PrefabCell.GetComponent<RectTransform>().sizeDelta;
		var contentSize = Content.GetComponent<RectTransform>().sizeDelta;
		var columns = Mathf.Floor(contentSize.x / cellSize.x);
		var lines = Mathf.Ceil(Size / columns);
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

		// Создаём список пустых ячеек.
		_cells = new List<CellController>();
		for (int i = 0; i < Size; i++)
		{
			var cell = Instantiate(PrefabCell);
			cell.name = "Cell" + (i + 1);
			cell.transform.SetParent(Content.transform);

			ctrl = cell.GetComponent<CellController>();
			_cells.Add(ctrl);
		}

		// Загружаем предметы.
		LoadItem();
    }

	private void LoadItem()
	{
		var item = Instantiate(PrefabItem);
		var ctrl = item.GetComponent<ItemController>();
		item.transform.SetParent(_cells[0].gameObject.transform);
		item.transform.localPosition = Vector2.zero;
		_cells[0].Item = ctrl;
    }
	#endregion
	#endregion
}