namespace Equipment
{
	public class EquipmentCellInfo
	{
		#region Properties
		#region Public
		public int IndexCell { get; set; }
		public int IndexItem { get; set; }
		public int Modification { get; set; }
		public int RarityItem { get; set; }
		#endregion
		#region Private

		#endregion
		#endregion

		private EquipmentCellInfo() { }

		#region Methods
		#region Public
		/// <summary>
		/// Получить EquipmenCellInfo на основе EquipmentCellController.
		/// </summary>
		/// <param name="cell">Ячейка экипировки.</param>
		public static EquipmentCellInfo ToCellInfo(EquipmentCellController cell)
		{
			var res = new EquipmentCellInfo();
			res.IndexCell = cell.Index;

			var item = cell.Item;
            if (item != null)
			{
				res.IndexItem = item.BaseItem.Index;
				res.Modification = item.Modification;
				res.RarityItem = (int)item.RarityItem;
			}

			return res;
		}
		/// <summary>
		/// Перенести дополнительные параметры ячейки и предмета в ячейке.
		/// </summary>
		/// <param name="cell">Ячейка.</param>
		public void MoveMoreParams(CellController cell)
		{
			var item = cell.Item;
			if (item != null)
			{
				item.Modification = Modification;
				item.RarityItem = (Rarity)RarityItem;
            }
		}
		#endregion
		#region Private

		#endregion
		#endregion
	}
}