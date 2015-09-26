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
	
	#endregion
	#region Private
	private void Start()
	{

	}
	#endregion
	#endregion

	public delegate void ChangeItemHandler(ItemController item);
}