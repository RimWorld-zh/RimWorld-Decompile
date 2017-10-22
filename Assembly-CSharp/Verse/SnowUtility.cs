namespace Verse
{
	public static class SnowUtility
	{
		public static SnowCategory GetSnowCategory(float snowDepth)
		{
			if (snowDepth < 0.029999999329447746)
			{
				return SnowCategory.None;
			}
			if (snowDepth < 0.25)
			{
				return SnowCategory.Dusting;
			}
			if (snowDepth < 0.5)
			{
				return SnowCategory.Thin;
			}
			if (snowDepth < 0.75)
			{
				return SnowCategory.Medium;
			}
			return SnowCategory.Thick;
		}

		public static string GetDescription(SnowCategory category)
		{
			switch (category)
			{
			case SnowCategory.None:
			{
				return "SnowNone".Translate();
			}
			case SnowCategory.Dusting:
			{
				return "SnowDusting".Translate();
			}
			case SnowCategory.Thin:
			{
				return "SnowThin".Translate();
			}
			case SnowCategory.Medium:
			{
				return "SnowMedium".Translate();
			}
			case SnowCategory.Thick:
			{
				return "SnowThick".Translate();
			}
			default:
			{
				return "Unknown snow";
			}
			}
		}

		public static int MovementTicksAddOn(SnowCategory category)
		{
			switch (category)
			{
			case SnowCategory.None:
			{
				return 0;
			}
			case SnowCategory.Dusting:
			{
				return 0;
			}
			case SnowCategory.Thin:
			{
				return 4;
			}
			case SnowCategory.Medium:
			{
				return 8;
			}
			case SnowCategory.Thick:
			{
				return 12;
			}
			default:
			{
				return 0;
			}
			}
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
