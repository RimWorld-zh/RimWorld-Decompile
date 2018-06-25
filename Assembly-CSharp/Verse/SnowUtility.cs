using System;

namespace Verse
{
	public static class SnowUtility
	{
		public static SnowCategory GetSnowCategory(float snowDepth)
		{
			SnowCategory result;
			if (snowDepth < 0.03f)
			{
				result = SnowCategory.None;
			}
			else if (snowDepth < 0.25f)
			{
				result = SnowCategory.Dusting;
			}
			else if (snowDepth < 0.5f)
			{
				result = SnowCategory.Thin;
			}
			else if (snowDepth < 0.75f)
			{
				result = SnowCategory.Medium;
			}
			else
			{
				result = SnowCategory.Thick;
			}
			return result;
		}

		public static string GetDescription(SnowCategory category)
		{
			string result;
			switch (category)
			{
			case SnowCategory.None:
				result = "SnowNone".Translate();
				break;
			case SnowCategory.Dusting:
				result = "SnowDusting".Translate();
				break;
			case SnowCategory.Thin:
				result = "SnowThin".Translate();
				break;
			case SnowCategory.Medium:
				result = "SnowMedium".Translate();
				break;
			case SnowCategory.Thick:
				result = "SnowThick".Translate();
				break;
			default:
				result = "Unknown snow";
				break;
			}
			return result;
		}

		public static int MovementTicksAddOn(SnowCategory category)
		{
			int result;
			switch (category)
			{
			case SnowCategory.None:
				result = 0;
				break;
			case SnowCategory.Dusting:
				result = 0;
				break;
			case SnowCategory.Thin:
				result = 4;
				break;
			case SnowCategory.Medium:
				result = 8;
				break;
			case SnowCategory.Thick:
				result = 12;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.snowGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}
	}
}
