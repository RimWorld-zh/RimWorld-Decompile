using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098D RID: 2445
	public class ShortcutKeys
	{
		// Token: 0x060036F6 RID: 14070 RVA: 0x001D5D50 File Offset: 0x001D4150
		public void ShortcutKeysOnGUI()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (KeyBindingDefOf.NextColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectNextColonist();
					Event.current.Use();
				}
				if (KeyBindingDefOf.PreviousColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectPreviousColonist();
					Event.current.Use();
				}
			}
		}
	}
}
