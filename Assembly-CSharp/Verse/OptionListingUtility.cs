using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class OptionListingUtility
	{
		private const float OptionSpacing = 7f;

		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption item in optList)
			{
				num = (float)(num + (item.DrawOption(new Vector2(0f, num), rect.width) + 7.0));
			}
			GUI.EndGroup();
			return num;
		}
	}
}
