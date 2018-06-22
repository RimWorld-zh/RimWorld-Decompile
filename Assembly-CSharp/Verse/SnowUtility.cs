using System;

namespace Verse
{
	// Token: 0x02000CA4 RID: 3236
	public static class SnowUtility
	{
		// Token: 0x06004749 RID: 18249 RVA: 0x00259CA0 File Offset: 0x002580A0
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

		// Token: 0x0600474A RID: 18250 RVA: 0x00259D00 File Offset: 0x00258100
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

		// Token: 0x0600474B RID: 18251 RVA: 0x00259D8C File Offset: 0x0025818C
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

		// Token: 0x0600474C RID: 18252 RVA: 0x00259DE8 File Offset: 0x002581E8
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
