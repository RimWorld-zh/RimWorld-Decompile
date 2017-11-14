using System.Collections.Generic;

namespace Verse
{
	public static class CoverUtility
	{
		public const float CoverPercent_Corner = 0.75f;

		public static List<CoverInfo> CalculateCoverGiverSet(IntVec3 targetLoc, IntVec3 shooterLoc, Map map)
		{
			List<CoverInfo> list = new List<CoverInfo>();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = targetLoc + GenAdj.AdjacentCells[i];
				CoverInfo item = default(CoverInfo);
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, targetLoc, intVec, map, out item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static float CalculateOverallBlockChance(IntVec3 targetLoc, IntVec3 shooterLoc, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = targetLoc + GenAdj.AdjacentCells[i];
				CoverInfo coverInfo = default(CoverInfo);
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, targetLoc, intVec, map, out coverInfo))
				{
					num = (float)(num + (1.0 - num) * coverInfo.BlockChance);
				}
			}
			return num;
		}

		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, IntVec3 targetLoc, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			Thing cover = adjCell.GetCover(map);
			float num2;
			if (cover != null && !(shooterLoc == targetLoc))
			{
				float angleFlat = (shooterLoc - targetLoc).AngleFlat;
				float angleFlat2 = (adjCell - targetLoc).AngleFlat;
				float num = GenGeo.AngleDifferenceBetween(angleFlat2, angleFlat);
				if (!targetLoc.AdjacentToCardinal(adjCell))
				{
					num = (float)(num * 1.75);
				}
				num2 = cover.def.BaseBlockChance();
				if (num < 15.0)
				{
					num2 = (float)(num2 * 1.0);
					goto IL_010b;
				}
				if (num < 27.0)
				{
					num2 = (float)(num2 * 0.800000011920929);
					goto IL_010b;
				}
				if (num < 40.0)
				{
					num2 = (float)(num2 * 0.60000002384185791);
					goto IL_010b;
				}
				if (num < 52.0)
				{
					num2 = (float)(num2 * 0.40000000596046448);
					goto IL_010b;
				}
				if (num < 65.0)
				{
					num2 = (float)(num2 * 0.20000000298023224);
					goto IL_010b;
				}
				result = CoverInfo.Invalid;
				return false;
			}
			result = CoverInfo.Invalid;
			return false;
			IL_010b:
			float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
			if (lengthHorizontal < 1.8999999761581421)
			{
				num2 = (float)(num2 * 0.33329999446868896);
			}
			else if (lengthHorizontal < 2.9000000953674316)
			{
				num2 = (float)(num2 * 0.66666001081466675);
			}
			result = new CoverInfo(cover, num2);
			return true;
		}

		public static float BaseBlockChance(this ThingDef def)
		{
			if (def.Fillage == FillCategory.Full)
			{
				return 0.75f;
			}
			return def.fillPercent;
		}

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
	}
}
