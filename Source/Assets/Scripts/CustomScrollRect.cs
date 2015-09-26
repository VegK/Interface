using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	public class CustomScrollRect : ScrollRect
	{
		public event ScrollPointerHandler OnBeginScroll;
		public event ScrollPointerHandler OnEndScroll;

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			if (OnBeginScroll != null)
				OnBeginScroll(this, eventData);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			if (OnEndScroll != null)
				OnEndScroll(this, eventData);
		}

		public delegate void ScrollPointerHandler(object sender, PointerEventData eventData);
	}
}