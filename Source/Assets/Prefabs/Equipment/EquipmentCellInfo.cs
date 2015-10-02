namespace Equipment
{
	public class EquipmentCellInfo
	{
		#region Properties
		#region Public
		public int IndexCell { get; set; }
		public int IndexItem { get; set; }
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
			if (cell.Item != null)
			{
				res.IndexItem = cell.Item.BaseItem.Index;
			}
			else
			{
				res.IndexItem = 0;
			}
			return res;
		}
		#endregion
		#region Private

		#endregion
		#endregion
	}
}