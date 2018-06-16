using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB2 RID: 4018
	public static class CoverUtility
	{
		// Token: 0x06006112 RID: 24850 RVA: 0x0030F960 File Offset: 0x0030DD60
		public static List<CoverInfo> CalculateCoverGiverSet(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			List<CoverInfo> list = new List<CoverInfo>();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(map))
				{
					CoverInfo item;
					if (CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x0030F9DC File Offset: 0x0030DDDC
		public static float CalculateOverallBlockChance(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(map))
				{
					CoverInfo coverInfo;
					if (CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out coverInfo))
					{
						num += (1f - num) * coverInfo.BlockChance;
					}
				}
			}
			return num;
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x0030FA60 File Offset: 0x0030DE60
		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, LocalTargetInfo target, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			IntVec3 cell = target.Cell;
			Thing cover = adjCell.GetCover(map);
			bool result2;
			if (cover == null || cover == target.Thing || shooterLoc == cell)
			{
				result = CoverInfo.Invalid;
				result2 = false;
			}
			else
			{
				float angleFlat = (shooterLoc - cell).AngleFlat;
				float angleFlat2 = (adjCell - cell).AngleFlat;
				float num = GenGeo.AngleDifferenceBetween(angleFlat2, angleFlat);
				if (!cell.AdjacentToCardinal(adjCell))
				{
					num *= 1.75f;
				}
				float num2 = cover.def.BaseBlockChance();
				if (num < 15f)
				{
					num2 *= 1f;
				}
				else if (num < 27f)
				{
					num2 *= 0.8f;
				}
				else if (num < 40f)
				{
					num2 *= 0.6f;
				}
				else if (num < 52f)
				{
					num2 *= 0.4f;
				}
				else
				{
					if (num >= 65f)
					{
						result = CoverInfo.Invalid;
						return false;
					}
					num2 *= 0.2f;
				}
				float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
				if (lengthHorizontal < 1.9f)
				{
					num2 *= 0.3333f;
				}
				else if (lengthHorizontal < 2.9f)
				{
					num2 *= 0.66666f;
				}
				result = new CoverInfo(cover, num2);
				result2 = true;
			}
			return result2;
		}

		// Token: 0x06006115 RID: 24853 RVA: 0x0030FBF4 File Offset: 0x0030DFF4
		public static float BaseBlockChance(this ThingDef def)
		{
			float result;
			if (def.Fillage == FillCategory.Full)
			{
				result = 0.75f;
			}
			else
			{
				result = def.fillPercent;
			}
			return result;
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x0030FC28 File Offset: 0x0030E028
		public static float TotalSurroundingCoverScore(IntVec3 c, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Thing cover = c2.GetCover(map);
					if (cover != null)
					{
						num += cover.def.BaseBlockChance();
					}
				}
			}
			return num;
		}

		// Token: 0x04003F7C RID: 16252
		public const float CoverPercent_Corner = 0.75f;
	}
}
