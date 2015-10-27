using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class BaseInventory : MonoBehaviour
{
	#region Properties
	#region Public
	public RectTransform Content;

	public int Size;
	#endregion
	#region Private
	protected List<CellController> Cells;
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
		var contentSize = Content.sizeDelta;
		var columns = Mathf.Floor(contentSize.x / cellSize.x);
		var lines = Mathf.Ceil(size / columns);
		var newHeight = cellSize.y * lines;

		// Если новая высота контанта больше текущей, то меняем параметры подложки.
		if (newHeight > Content.sizeDelta.y)
		{
			contentSize.y = newHeight;
			Content.sizeDelta = contentSize;

			var pos = Content.transform.localPosition;
			pos.y = -contentSize.y / 2;
			Content.transform.localPosition = pos;
		}
	}
	/// <summary>
	/// Поменять местами предметы в ячейках.
	/// </summary>
	public static void SwapItemsInCell(CellController from, CellController to)
	{
		if (from == null || to == null)
			return;

		// Проверяем возможно ли перемещение предмета из ячейке назначения в ячейку "от куда".
		if (!from.CheckPutItem(to.Item))
			return;

		var tempItem = to.Item;
		var swap = SetItemInCell(from.Item, to);
		if (swap)
			SetItemInCell(tempItem, from);
	}
	/// <summary>
	/// Создать клона указанного предмета в указанной ячейки.
	/// </summary>
	/// <param name="cell">Ячейка в которой будет создан клон.</param>
	/// <param name="item">Предмет для клонирования.</param>
	public static ItemController CreateCloneItem(CellController cell, ItemController item)
	{
		ItemController clone = null;
		if (cell.CheckPutItem(item))
		{
			clone = Instantiate(item);
			clone.BaseItem = item.BaseItem;
			// TODO: Случайно задаём редкость предмета.
			clone.RarityItem = (Rarity)UnityEngine.Random.Range(0, 4);
			cell.PutItem(clone);
		}
		return clone;
	}
	/// <summary>
	/// Уничтожить объект в ячейке.
	/// </summary>
	/// <param name="cell">Ячейка с предметом.</param>
	public static void RecycleItem(CellController cell)
	{
		if (cell.Item != null)
		{
			Destroy(cell.Item.gameObject);
			LogController.Instance.AddString(String.Format("Предмет \"{0}\" удалён.", cell.Item.BaseItem.Name));
		}
		cell.Item = null;
	}
	/// <summary>
	/// Попытаться модифицировать предмет на 1.
	/// </summary>
	/// <param name="item">Предмет для модификации.</param>
	/// <returns>Результат модификации.</returns>
	public static bool ModificationItem(ItemController item)
	{
		var str = string.Empty;
		var res = false;

		// TODO: В 50% случаев предмет модифицируется.
		var rnd = UnityEngine.Random.Range(0, 2);
		if (rnd == 0)
		{
			item.Modification++;
			res = true;

			str = "<color=green>Успешная</color> модификация предмета \"{0}\".";
		}
		else
			str = "<color=red>Не удалось</color> модифицировать предмет \"{0}\".";

		LogController.Instance.AddString(String.Format(str, item.name));

		return res;
	}
	/// <summary>
	/// Создать объект ячейки инвентаря.
	/// </summary>
	/// <param name="name">Название ячейки.</param>
	/// <returns>Класс созданной ячейки.</returns>
	public virtual CellController CreateCell(string name, CellType type)
	{
		var cell = Instantiate(Parameters.Instance.PrefabCell);
		cell.name = name;
		cell.transform.SetParent(Content.transform);

		var index = 1;
		if (Cells.Count > 0)
			index = Cells.Max(c => c.Index) + 1;
		cell.Index = index;
		cell.Type = type;
		Cells.Add(cell);
		return cell;
	}
	/// <summary>
	/// Создать объект предмета на основании класса предмета.
	/// </summary>
	/// <param name="baseItem">Базовый предмет.</param>
	/// <returns>Класс созданного предмета.</returns>
	public virtual ItemController CreateItem(Item baseItem)
	{
		var path = Application.dataPath + "\\" + Parameters.ITEMS_PATH + "\\";

		var item = Instantiate(Parameters.Instance.PrefabItem);
		item.name = baseItem.Name;
		item.transform.position = Vector2.zero;

		var bytes = File.ReadAllBytes(path + baseItem.FileNameImage);
		var tex2d = new Texture2D(1, 1);
		tex2d.LoadImage(bytes);

		var rect = new Rect(0, 0, tex2d.width, tex2d.height);
		item.ImageItem.sprite = Sprite.Create(tex2d, rect, Vector2.zero);
		item.BaseItem = baseItem;
		return item;
	}
	/// <summary>
	/// Положить предмет в ячейку.
	/// </summary>
	/// <param name="item">Предмет.</param>
	/// <param name="cell">Ячейка.</param>
	public static bool SetItemInCell(ItemController item, CellController cell)
	{
		if (cell == null)
			throw new NullReferenceException("Ячейка не может быть NULL.");
		return cell.PutItem(item);
	}
	#endregion
	#region Private
	protected virtual void Start()
	{
		Cells = new List<CellController>();
		CreatingCells();
		SetHeightContent();
	}
	/// <summary>
	/// Создать ячейки инвентаря.
	/// </summary>
	private void CreatingCells()
	{
		if (Size == 0)
			return;

		for (int i = 0; i < Size; i++)
			CreateCell("Cell" + (i + 1), CellType.Standart);
	}
	#endregion
	#endregion
}