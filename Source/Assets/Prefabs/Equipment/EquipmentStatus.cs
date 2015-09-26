using Inventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
				using (FileStream stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
				using (TextWriter writer = new StreamWriter(stream))
				{
					ser.Serialize(writer, listItems);

					var hash = stream.GetSHA1Hash();
					PlayerPrefs.SetString("equipment", hash);
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

					var hashFile = stream.GetSHA1Hash();
					var hashSave = PlayerPrefs.GetString("equipment");

					if (!String.IsNullOrEmpty(hashSave) && hashFile == hashSave)
					{
						if (list != null)
							status = list;
					}
					else
					{
						LogController.Instance.AddString("<color=red>Ошибка экипировки</color>: файл экипировки повреждём - загрузка невозможна.");
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
