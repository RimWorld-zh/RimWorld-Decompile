using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8D RID: 3725
	public class ScreenshotModeHandler
	{
		// Token: 0x04003A27 RID: 14887
		private bool active = false;

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060057F0 RID: 22512 RVA: 0x002D2120 File Offset: 0x002D0520
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060057F1 RID: 22513 RVA: 0x002D213C File Offset: 0x002D053C
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x002D21C1 File Offset: 0x002D05C1
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
