namespace Inventory
{
	public class CellInfo
	{
		#region Properties
		#region Public
		public int IndexCell { get; set; }
		public int IndexItem { get; set; }
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