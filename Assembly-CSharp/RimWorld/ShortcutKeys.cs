using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000989 RID: 2441
	public class ShortcutKeys
	{
		// Token: 0x060036EF RID: 14063 RVA: 0x001D5F4C File Offset: 0x001D434C
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
