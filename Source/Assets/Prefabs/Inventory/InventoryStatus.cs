using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Inventory
{
	public class InventoryStatus
	{
		#region Properties
		#region Public
		public List<CellInfo> Cells;
		#endregion
		#region Private
		private List<CellController> _cells;
		#endregion
		#endregion

		private InventoryStatus()
		{
			Cells = new List<CellInfo>();
		}
		public InventoryStatus(List<CellController> cells)
		{
			_cells = cells;
		}

		#region Methods
		#region Public
		/// <summary>
		/// Сохранить состояние инвентаря в файл.
		/// </summary>
		/// <returns>Сохранение прошло успешно.</returns>
		public bool Save()
		{
			var listItems = new InventoryStatus();
			foreach (CellController cell in _cells)
				if (cell.Item != null)
					listItems.Cells.Add(CellInfo.ToCellInfo(cell));

			var file = Application.dataPath + "\\" + InventoryController.FILENAME_SAVE;
			var ser = new XmlSerializer(typeof(InventoryStatus));

			try
			{
				using (FileStream stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
				using (TextWriter writer = new StreamWriter(stream))
				{
					ser.Serialize(writer, listItems);

					var hash = stream.GetSHA1Hash();
					PlayerPrefs.SetString("inventory", hash);
					PlayerPrefs.Save();

					writer.Close();
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Загрузить состояние инвентаря из файла.
		/// </summary>
		/// <param name="status">Загруженный статус инвентаря.</param>
		/// <returns>Загрузка прошла успешно.</returns>
		public static bool Load(out InventoryStatus status)
		{
			status = new InventoryStatus();
			var file = Application.dataPath + "\\" + InventoryController.FILENAME_SAVE;
			if (!File.Exists(file))
				return false;

			var ser = new XmlSerializer(typeof(InventoryStatus));
			try
			{
				using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					var list = ser.Deserialize(stream) as InventoryStatus;
					var hashFile = stream.GetSHA1Hash();
					var hashSave = PlayerPrefs.GetString("inventory");

					if (!String.IsNullOrEmpty(hashSave) && hashFile == hashSave)
					{
						if (list != null)
							status = list;
					}
					else
					{
						LogController.Instance.AddString("<color=red>Ошибка инвентаря</color>: файл инвентаря повреждён - загрузка невозможна.");
					}
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
		#endregion
		#region Private

		#endregion
		#endregion
	}
}