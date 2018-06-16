using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098D RID: 2445
	public class ShortcutKeys
	{
		// Token: 0x060036F4 RID: 14068 RVA: 0x001D5C88 File Offset: 0x001D4088
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
