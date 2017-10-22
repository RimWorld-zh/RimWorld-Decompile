using System;
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
			List<ListableOption>.Enumerator enumerator = optList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ListableOption current = enumerator.Current;
					num = (float)(num + (current.DrawOption(new Vector2(0f, num), rect.width) + 7.0));
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			GUI.EndGroup();
			return num;
		}
	}
}
