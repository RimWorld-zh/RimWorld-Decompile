using RimWorld;
using UnityEngine;

namespace Verse
{
	public class ScreenshotModeHandler
	{
		private bool active;

		public bool FiltersCurrentEvent
		{
			get
			{
				if (!this.active)
				{
					return false;
				}
				if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
				{
					if (Event.current.type != 0 && Event.current.type != EventType.MouseUp && Event.current.type != EventType.MouseDrag)
					{
						return false;
					}
					return true;
				}
				return true;
			}
		}

		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}
	}
}
