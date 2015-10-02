namespace Inventory
{
	public class CellInfo
	{
		#region Properties
		#region Public
		public int IndexCell { get; set; }
		public int IndexItem { get; set; }
		public int Modification { get; set; }
		#endregion
		#region Private

		#endregion
		#endregion

		private CellInfo() { }

		#region Methods
		#region Public
		/// <summary>
		/// Получить CellInfo на основе CellController.
		/// </summary>
		/// <param name="cell">Ячейка.</param>
		public static CellInfo ToCellInfo(CellController cell)
		{
			var res = new CellInfo();
			res.IndexCell = cell.Index;

			var item = cell.Item;
			if (item != null)
			{
                res.IndexItem = item.BaseItem.Index;
				res.Modification = item.Modification;
			}

			return res;
		}
		/// <summary>
		/// Перенести дополнительные параметры ячейки и предмета в ячейке.
		/// </summary>
		/// <param name="cell">Ячейка.</param>
		public void MoveMoreParams(CellController cell)
		{
			if (cell.Item != null)
				cell.Item.Modification = Modification;
		}
		#endregion
		#region Private

		#endregion
		#endregion
	}
}