using System;
using UnityEngine;

public class CellController : MonoBehaviour
{
	#region Properties
	#region Public
	public int Index;
	public CellType Type;
	public GameObject SelectedCell;

	public ItemController Item
	{
		get
		{
			return _item;
		}
		set
		{
			var change = (_item != value);

			_item = value;

			if (change && OnChangeItem != null)
				OnChangeItem(value);
		}
	}

	/// <summary>
	/// Событие вызываемое при смене предмета в ячейке.
	/// </summary>
	public event ChangeItemHandler OnChangeItem; 
	#endregion
	#region Private
	private ItemController _item;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Проверить возможно ли положить предмет в ячейку.
	/// </summary>
	/// <param name="item">Предмет.</param>
	/// <returns>Можно положить.</returns>
	public virtual bool CheckPutItem(ItemController item)
	{
		var res = true;
		if (Item != null)
			res = !Item.FixedCell;
		return res;
	}
	/// <summary>
	/// Положить предмет в ячейку.
	/// </summary>
	/// <param name="item">Предмет.</param>
	/// <returns>В случаи удачного действия возвращает true.</returns>
	public virtual bool PutItem(ItemController item)
	{
		if (!CheckPutItem(item))
			return false;
		if (item != null)
			item.transform.SetParent(transform, false);
		this.Item = item;
		return true;
	}
	/// <summary>
	/// Показать/скрыть выделение ячейки.
	/// </summary>
	/// <param name="value">Показать/скрыть.</param>
	public void SetSelected(bool value)
	{
		if (SelectedCell == null)
			return;
		SelectedCell.SetActive(value);
	}
	#endregion
	#region Private
	protected virtual void Awake()
	{
		if (SelectedCell == null)
			return;
		SelectedCell.SetActive(false);
	}
	#endregion
	#endregion

	public delegate void ChangeItemHandler(ItemController item);
}