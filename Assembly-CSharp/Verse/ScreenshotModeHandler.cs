using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8A RID: 3722
	public class ScreenshotModeHandler
	{
		// Token: 0x04003A1F RID: 14879
		private bool active = false;

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060057EC RID: 22508 RVA: 0x002D1E08 File Offset: 0x002D0208
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x060057ED RID: 22509 RVA: 0x002D1E24 File Offset: 0x002D0224
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x002D1EA9 File Offset: 0x002D02A9
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
