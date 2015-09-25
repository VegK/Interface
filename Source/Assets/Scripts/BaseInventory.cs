using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public abstract class BaseInventory : MonoBehaviour
{
	#region Properties
	#region Public
	public GameObject Content;

	public int Size;
	#endregion
	#region Private
	protected List<CellController> Cells = new List<CellController>();
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Установить высоту объекта контента равную размеру инвентаря.
	/// </summary>
	public void SetHeightContent()
	{
		SetHeightContent(Size);
    }

	/// <summary>
	/// Установить высоту объекта контента.
	/// </summary>
	/// <param name="size">Количество ячеек.</param>
	public void SetHeightContent(int size)
	{
		var cellSize = Parameters.Instance.PrefabCell.GetComponent<RectTransform>().sizeDelta;
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
	}

	/// <summary>
	/// Поменять местами предметы в ячейках.
	/// </summary>
	public static void SwapItemsInCell(CellController from, CellController to)
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
	public static GameObject CreateCloneItem(CellController cell, ItemController item)
	{
		var obj = Instantiate(item.gameObject);
		obj.transform.SetParent(cell.transform, false);

		var ctrl = obj.GetComponent<ItemController>();
		cell.Item = ctrl;

		return obj;
	}

	/// <summary>
	/// Создать объект ячейки инвентаря.
	/// </summary>
	/// <param name="name">Название ячейки.</param>
	/// <returns>Класс созданной ячейки.</returns>
	public virtual CellController CreateCell(string name)
	{
		var cell = Instantiate(Parameters.Instance.PrefabCell);
		cell.name = name;
		cell.transform.SetParent(Content.transform);

		var cellCtrl = cell.GetComponent<CellController>();
		Cells.Add(cellCtrl);
		return cellCtrl;
	}

	/// <summary>
	/// Создать объект предмета на основании класса предмета.
	/// </summary>
	/// <param name="item">Класс предмета.</param>
	/// <returns>Класс созданного предмета.</returns>
	public virtual ItemController CreateItem(Item item)
	{
		var path = Application.dataPath + "\\" + Parameters.ITEMS_PATH + "\\";

		var obj = Instantiate(Parameters.Instance.PrefabItem);
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
	#region Private
	protected virtual void Start()
	{
		CreatingCells();
    }

	private void CreatingCells()
	{
		if (Size == 0)
			return;

		for (int i = 0; i < Size; i++)
		{
			var cell = CreateCell("Cell" + (i + 1));
			Cells.Add(cell);
		}
	}
	#endregion
	#endregion
}