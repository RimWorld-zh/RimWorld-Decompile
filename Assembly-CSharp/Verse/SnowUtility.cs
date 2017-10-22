namespace Verse
{
	public static class SnowUtility
	{
		public static SnowCategory GetSnowCategory(float snowDepth)
		{
			return (SnowCategory)((!(snowDepth < 0.029999999329447746)) ? ((snowDepth < 0.25) ? 1 : ((!(snowDepth < 0.5)) ? ((!(snowDepth < 0.75)) ? 4 : 3) : 2)) : 0);
		}

		public static string GetDescription(SnowCategory category)
		{
			string result;
			switch (category)
			{
			case SnowCategory.None:
			{
				result = "SnowNone".Translate();
				break;
			}
			case SnowCategory.Dusting:
			{
				result = "SnowDusting".Translate();
				break;
			}
			case SnowCategory.Thin:
			{
				result = "SnowThin".Translate();
				break;
			}
			case SnowCategory.Medium:
			{
				result = "SnowMedium".Translate();
				break;
			}
			case SnowCategory.Thick:
			{
				result = "SnowThick".Translate();
				break;
			}
			default:
			{
				result = "Unknown snow";
				break;
			}
			}
			return result;
		}

		public static int MovementTicksAddOn(SnowCategory category)
		{
			int result;
			switch (category)
			{
			case SnowCategory.None:
			{
				result = 0;
				break;
			}
			case SnowCategory.Dusting:
			{
				result = 0;
				break;
			}
			case SnowCategory.Thin:
			{
				result = 4;
				break;
			}
			case SnowCategory.Medium:
			{
				result = 8;
				break;
			}
			case SnowCategory.Thick:
			{
				result = 12;
				break;
			}
			default:
			{
				result = 0;
				break;
			}
			}
			return result;
		}

		public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int num2 = 0; num2 < num; num2++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[num2];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num3 = (float)(1.0 - lengthHorizontal / radius);
					map.snowGrid.AddDepth(intVec, num3 * depth);
				}
			}
		}
	}
}
