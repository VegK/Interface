using UnityEngine;
using System.Linq;
using Equipment;
using System.Collections.Generic;

public class EquipmentController : MonoBehaviour
{
	public const string FILENAME_SAVE = "equipment.xml";

	#region Properties
	#region Public

	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Start()
	{
		Load();
    }

	private void OnApplicationQuit()
	{
		Save();
    }
	/// <summary>
	/// Сохранить экипировку.
	/// </summary>
	private void Save()
	{
		var equips = GetComponentsInChildren<EquipmentCellController>();
		if (equips.Count() > 0)
		{
			var status = new EquipmentStatus(equips.ToList());
			status.Save();
		}
	}
	/// <summary>
	/// Загрузить экипировку.
	/// </summary>
	private void Load()
	{
		EquipmentStatus status;
		if (!EquipmentStatus.Load(out status))
			return;

		var equips = new List<EquipmentCellController>();
		equips.AddRange(GetComponentsInChildren<EquipmentCellController>());

		foreach (EquipmenCellInfo info in status.Cells)
		{
			var cell = equips.Find(c => c.Index == info.IndexCell);
			if (cell == null)
			{
				// TODO: ошибка cell
				continue;
			}

			var baseItem = LibraryController.Instance.GetItem(info.IndexItem);
			if (baseItem == null)
			{
				// TODO: ошибка item
				continue;
			}

			var item = LibraryController.Instance.CreateItem(baseItem);
			cell.SetItem(item);
		}
	}
	#endregion
	#endregion
}