using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8C RID: 3724
	public class ScreenshotModeHandler
	{
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060057CE RID: 22478 RVA: 0x002D01F8 File Offset: 0x002CE5F8
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060057CF RID: 22479 RVA: 0x002D0214 File Offset: 0x002CE614
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x002D0299 File Offset: 0x002CE699
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x04003A11 RID: 14865
		private bool active = false;
	}
}
