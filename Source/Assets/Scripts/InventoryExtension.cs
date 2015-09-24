using UnityEngine;
using System.Collections;

public class InventoryExtension : MonoBehaviour
{
	#region Properties
	#region Public

	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public
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
	#endregion
	#region Private

	#endregion
	#endregion
}