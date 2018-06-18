using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9F RID: 3743
	public static class OptionListingUtility
	{
		// Token: 0x06005849 RID: 22601 RVA: 0x002D34C0 File Offset: 0x002D18C0
		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption listableOption in optList)
			{
				num += listableOption.DrawOption(new Vector2(0f, num), rect.width) + 7f;
			}
			GUI.EndGroup();
			return num;
		}

		// Token: 0x04003A68 RID: 14952
		private const float OptionSpacing = 7f;
	}
}
