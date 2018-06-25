using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098B RID: 2443
	public class ShortcutKeys
	{
		// Token: 0x060036F3 RID: 14067 RVA: 0x001D608C File Offset: 0x001D448C
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
