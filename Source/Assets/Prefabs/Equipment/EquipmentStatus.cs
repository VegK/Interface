using Inventory;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Equipment
{
	public class EquipmentStatus
	{
		#region Properties
		#region Public
		public List<EquipmenCellInfo> Cells;
		#endregion
		#region Private
		private List<EquipmentCellController> _cells;
		#endregion
		#endregion

		private EquipmentStatus()
		{
			Cells = new List<EquipmenCellInfo>();
		}
		public EquipmentStatus(List<EquipmentCellController> cells)
		{
			_cells = cells;
		}

		#region Methods
		#region Public
		/// <summary>
		/// Сохранить состояние экипировки в файл.
		/// </summary>
		/// <returns>Сохранение прошло успешно.</returns>
		public bool Save()
		{
			var listItems = new EquipmentStatus();
			foreach (EquipmentCellController cell in _cells)
				if (cell.Item != null)
					listItems.Cells.Add(EquipmenCellInfo.ToCellInfo(cell));

			var file = Application.dataPath + "\\" + EquipmentController.FILENAME_SAVE;
			var ser = new XmlSerializer(typeof(EquipmentStatus));

			try
			{
				using (FileStream stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				using (TextWriter writer = new StreamWriter(stream))
				{
					ser.Serialize(writer, listItems);
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
		/// Загрузить состояние экипировки из файла.
		/// </summary>
		/// <param name="status">Загруженный статус экипировки.</param>
		/// <returns>Загрузка прошла успешно.</returns>
		public static bool Load(out EquipmentStatus status)
		{
			status = new EquipmentStatus();
			var file = Application.dataPath + "\\" + EquipmentController.FILENAME_SAVE;
			if (!File.Exists(file))
				return false;

			var ser = new XmlSerializer(typeof(EquipmentStatus));
			try
			{
				using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					var list = ser.Deserialize(stream) as EquipmentStatus;
					if (list != null)
						status = list;
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
