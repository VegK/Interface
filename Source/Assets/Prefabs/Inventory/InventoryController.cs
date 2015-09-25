namespace Inventory
{
	public class InventoryController : BaseInventory
	{
		public const string FILENAME_SAVE = "inventory.xml";

		#region Properties
		#region Public

		#endregion
		#region Private

		#endregion
		#endregion

		#region Methods
		#region Public

		#endregion
		#region Private
		protected override void Start()
		{
			base.Start();
			LoadInventory();
        }

		private void OnApplicationQuit()
		{
			var status = new InventoryStatus(Cells);
			status.Save();
		}
		/// <summary>
		/// Загрузить инвентарь.
		/// </summary>
		private void LoadInventory()
		{
			InventoryStatus status;
			if (!InventoryStatus.Load(out status))
				return;

			foreach (CellInfo info in status.Cells)
			{
				var cell = Cells.Find(c => c.Index == info.IndexCell);
				if (cell == null)
				{
					// TODO: ошибка cell
					continue;
				}

				var baseItem = LibraryController.Instance.GetItem(info.IndexItem);
				if (baseItem == null)
				{
					// TODO: ошибка item
					continue;
				}

				var item = CreateItem(baseItem);
				SetItemInCell(item, cell);
			}
		}
		#endregion
		#endregion
	}
}