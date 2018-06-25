using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8C RID: 3724
	public class ScreenshotModeHandler
	{
		// Token: 0x04003A1F RID: 14879
		private bool active = false;

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060057F0 RID: 22512 RVA: 0x002D1F34 File Offset: 0x002D0334
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060057F1 RID: 22513 RVA: 0x002D1F50 File Offset: 0x002D0350
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x002D1FD5 File Offset: 0x002D03D5
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
