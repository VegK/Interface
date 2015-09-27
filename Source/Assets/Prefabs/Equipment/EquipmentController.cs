using Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
	public const string FILENAME_SAVE = "equipment.xml";

	#region Properties
	#region Public
	public static EquipmentController Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<EquipmentController>();
			if (_instance == null)
				throw new Exception(string.Format("На сцене отсутствует объект с компонентом \"{0}\".", typeof(EquipmentController)));
			return _instance;
		}
	}
	#endregion
	#region Private
	private static EquipmentController _instance;
	
	private List<EquipmentCellController> _cells;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Сбросить доступность всех ячеек экипировки.
	/// </summary>
	public void ResetAvailableCells()
	{
		foreach (EquipmentCellController cell in _cells)
		{
			cell.SetAvailable(false);
			cell.SetBackground(cell.Item == null);
		}
	}
	/// <summary>
	/// Включить доступность ячейки экипировки определённого типа.
	/// </summary>
	/// <param name="typeItem">Тип предмета.</param>
	/// <param name="nullCell">Выделять только пустые ячейки.</param>
	/// <param name="item">Не выделять ячейки содержащие данный предмет.</param>
    public void EnableAvailableCells(ItemType typeItem, bool nullCell, ItemController item)
	{
		foreach (EquipmentCellController cell in _cells)
		{
			var available = (cell.TypeItem == typeItem);
			if (nullCell)
				available &= (cell.Item == null);
			if (item != null && item == cell.Item)
				available = false;

            cell.SetAvailable(available);
		}
	}
	#endregion
	#region Private
	private void Start()
	{
		_cells = new List<EquipmentCellController>();
		_cells.AddRange(GetComponentsInChildren<EquipmentCellController>());
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
		if (_cells.Count() > 0)
		{
			var status = new EquipmentStatus(_cells);
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

		foreach (EquipmenCellInfo info in status.Cells)
		{
			var cell = _cells.Find(c => c.Index == info.IndexCell);
			if (cell == null)
			{
				var str = "<color=red>Ошибка экипировки</color>: не удалось найти ячейку с индексом <b>{0}</b>.";
				LogController.Instance.AddString(string.Format(str, info.IndexCell));
				continue;
			}

			var baseItem = LibraryController.Instance.GetItem(info.IndexItem);
			if (baseItem == null)
			{
				var str = "<color=red>Ошибка экипировки</color>: не удалось найти предмет с индексом <b>{0}</b>.";
				LogController.Instance.AddString(string.Format(str, info.IndexItem));
				continue;
			}

			var item = LibraryController.Instance.CreateItem(baseItem);
			cell.PutItem(item);
		}
	}
	#endregion
	#endregion
}