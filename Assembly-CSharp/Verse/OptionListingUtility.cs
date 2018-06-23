using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E9E RID: 3742
	public static class OptionListingUtility
	{
		// Token: 0x04003A78 RID: 14968
		private const float OptionSpacing = 7f;

		// Token: 0x06005869 RID: 22633 RVA: 0x002D50D0 File Offset: 0x002D34D0
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
