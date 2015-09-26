using System;
using UnityEngine;

public class CellController : MonoBehaviour
{
	#region Properties
	#region Public
	public int Index;
	public CellType Type;

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
	public virtual bool CheckSetItem(ItemController item)
	{
		return true;
	}
	/// <summary>
	/// Положить предмет в ячейку.
	/// </summary>
	/// <param name="item">Предмет.</param>
	/// <returns>В случаи удачного действия возвращает true.</returns>
	public virtual bool SetItem(ItemController item)
	{
		if (!CheckSetItem(item))
			return false;
		if (item != null)
			item.transform.SetParent(transform, false);
		this.Item = item;
		return true;
	}
	#endregion
	#region Private
	protected virtual void Start()
	{

	}
	#endregion
	#endregion

	public delegate void ChangeItemHandler(ItemController item);
}