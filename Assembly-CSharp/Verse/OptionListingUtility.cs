using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EA1 RID: 3745
	public static class OptionListingUtility
	{
		// Token: 0x04003A80 RID: 14976
		private const float OptionSpacing = 7f;

		// Token: 0x0600586D RID: 22637 RVA: 0x002D53E8 File Offset: 0x002D37E8
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
	}
}
