using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class SelectionDrawerUtility
	{
		private const float SelJumpDuration = 0.07f;

		private const float SelJumpDistance = 0.2f;

		public static readonly Texture2D SelectedTexGUI = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketGUI", true);

		public static void CalculateSelectionBracketPositionsUI<T>(Vector2[] bracketLocs, T obj, Rect rect, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num = default(float);
			float num2 = (float)(selectTimes.TryGetValue(obj, out num) ? Mathf.Max(0f, (float)(1.0 - (Time.realtimeSinceStartup - num) / 0.070000000298023224)) : 1.0);
			float num3 = (float)(num2 * 0.20000000298023224 * jumpDistanceFactor);
			float num4 = (float)(0.5 * (rect.width - textureSize.x) + num3);
			float num5 = (float)(0.5 * (rect.height - textureSize.y) + num3);
			ref Vector2 val = ref bracketLocs[0];
			Vector2 center = rect.center;
			float x = center.x - num4;
			Vector2 center2 = rect.center;
			val = new Vector2(x, center2.y - num5);
			ref Vector2 val2 = ref bracketLocs[1];
			Vector2 center3 = rect.center;
			float x2 = center3.x + num4;
			Vector2 center4 = rect.center;
			val2 = new Vector2(x2, center4.y - num5);
			ref Vector2 val3 = ref bracketLocs[2];
			Vector2 center5 = rect.center;
			float x3 = center5.x + num4;
			Vector2 center6 = rect.center;
			val3 = new Vector2(x3, center6.y + num5);
			ref Vector2 val4 = ref bracketLocs[3];
			Vector2 center7 = rect.center;
			float x4 = center7.x - num4;
			Vector2 center8 = rect.center;
			val4 = new Vector2(x4, center8.y + num5);
		}

		public static void CalculateSelectionBracketPositionsWorld<T>(Vector3[] bracketLocs, T obj, Vector3 worldPos, Vector2 worldSize, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num = default(float);
			float num2 = (float)(selectTimes.TryGetValue(obj, out num) ? Mathf.Max(0f, (float)(1.0 - (Time.realtimeSinceStartup - num) / 0.070000000298023224)) : 1.0);
			float num3 = (float)(num2 * 0.20000000298023224 * jumpDistanceFactor);
			float num4 = (float)(0.5 * (worldSize.x - textureSize.x) + num3);
			float num5 = (float)(0.5 * (worldSize.y - textureSize.y) + num3);
			float y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
			bracketLocs[0] = new Vector3(worldPos.x - num4, y, worldPos.z - num5);
			bracketLocs[1] = new Vector3(worldPos.x + num4, y, worldPos.z - num5);
			bracketLocs[2] = new Vector3(worldPos.x + num4, y, worldPos.z + num5);
			bracketLocs[3] = new Vector3(worldPos.x - num4, y, worldPos.z + num5);
		}
	}
}
