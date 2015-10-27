namespace Inventory
{
	public class CellInfo
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