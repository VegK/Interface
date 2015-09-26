namespace Equipment
{
	public class EquipmenCellInfo
	{
		#region Properties
		#region Public
		public int IndexCell { get; set; }
		public int IndexItem { get; set; }
		#endregion
		#region Private

		#endregion
		#endregion

		private EquipmenCellInfo() { }

		#region Methods
		#region Public
		/// <summary>
		/// Получить EquipmenCellInfo на основе EquipmentCellController.
		/// </summary>
		/// <param name="cell">Ячейка экипировки.</param>
		public static EquipmenCellInfo ToCellInfo(EquipmentCellController cell)
		{
			var res = new EquipmenCellInfo();
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