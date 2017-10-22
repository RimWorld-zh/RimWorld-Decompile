using RimWorld;
using UnityEngine;

namespace Verse
{
	public class ScreenshotModeHandler
	{
		private bool active = false;

		public bool FiltersCurrentEvent
		{
			get
			{
				return (byte)(this.active ? ((Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout) ? 1 : ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag) ? 1 : 0)) : 0) != 0;
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
