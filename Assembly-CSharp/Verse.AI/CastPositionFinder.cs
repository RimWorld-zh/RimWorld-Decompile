#define ENABLE_PROFILER
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse.AI
{
	public static class CastPositionFinder
	{
		private static CastPositionRequest req;

		private static IntVec3 casterLoc;

		private static IntVec3 targetLoc;

		private static Verb verb;

		private static float rangeFromTarget;

		private static float rangeFromTargetSquared;

		private static float optimalRangeSquared;

		private static float rangeFromCasterToCellSquared;

		private static float rangeFromTargetToCellSquared;

		private static int inRadiusMark;

		private static ByteGrid avoidGrid;

		private static float maxRangeFromCasterSquared;

		private static float maxRangeFromTargetSquared;

		private static float maxRangeFromLocusSquared;

		private static IntVec3 bestSpot = IntVec3.Invalid;

		private static float bestSpotPref = 0.001f;

		private const float BaseAIPreference = 0.3f;

		private const float MinimumPreferredRange = 5f;

		private const float OptimalRangeFactor = 0.8f;

		private const float OptimalRangeFactorImportance = 0.3f;

		public static bool TryFindCastPosition(CastPositionRequest newReq, out IntVec3 dest)
		{
			CastPositionFinder.req = newReq;
			CastPositionFinder.casterLoc = CastPositionFinder.req.caster.Position;
			CastPositionFinder.targetLoc = CastPositionFinder.req.target.Position;
			CastPositionFinder.verb = CastPositionFinder.req.verb;
			CastPositionFinder.avoidGrid = newReq.caster.GetAvoidGrid();
			bool result;
			if (CastPositionFinder.verb == null)
			{
				Log.Error(CastPositionFinder.req.caster + " tried to find casting position without a verb.");
				dest = IntVec3.Invalid;
				result = false;
			}
			else
			{
				if (CastPositionFinder.req.maxRegionsRadius > 0)
				{
					Region region = CastPositionFinder.casterLoc.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable);
					if (region == null)
					{
						Log.Error("TryFindCastPosition requiring region traversal but root region is null.");
						dest = IntVec3.Invalid;
						result = false;
						goto IL_05c2;
					}
					CastPositionFinder.inRadiusMark = Rand.Int;
					RegionTraverser.MarkRegionsBFS(region, null, newReq.maxRegionsRadius, CastPositionFinder.inRadiusMark, RegionType.Set_Passable);
					if (CastPositionFinder.req.maxRangeFromLocus > 0.0099999997764825821)
					{
						Region region2 = CastPositionFinder.req.locus.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable);
						if (region2 == null)
						{
							Log.Error("locus " + CastPositionFinder.req.locus + " has no region");
							dest = IntVec3.Invalid;
							result = false;
							goto IL_05c2;
						}
						if (region2.mark != CastPositionFinder.inRadiusMark)
						{
							Log.Error(CastPositionFinder.req.caster + " can't possibly get to locus " + CastPositionFinder.req.locus + " as it's not in a maxRegionsRadius of " + CastPositionFinder.req.maxRegionsRadius + ". Overriding maxRegionsRadius.");
							CastPositionFinder.req.maxRegionsRadius = 0;
						}
					}
				}
				CellRect cellRect = CellRect.WholeMap(CastPositionFinder.req.caster.Map);
				if (CastPositionFinder.req.maxRangeFromCaster > 0.0099999997764825821)
				{
					int num = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromCaster);
					CellRect otherRect = new CellRect(CastPositionFinder.casterLoc.x - num, CastPositionFinder.casterLoc.z - num, num * 2 + 1, num * 2 + 1);
					cellRect.ClipInsideRect(otherRect);
				}
				int num2 = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromTarget);
				CellRect otherRect2 = new CellRect(CastPositionFinder.targetLoc.x - num2, CastPositionFinder.targetLoc.z - num2, num2 * 2 + 1, num2 * 2 + 1);
				cellRect.ClipInsideRect(otherRect2);
				if (CastPositionFinder.req.maxRangeFromLocus > 0.0099999997764825821)
				{
					int num3 = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromLocus);
					CellRect otherRect3 = new CellRect(CastPositionFinder.targetLoc.x - num3, CastPositionFinder.targetLoc.z - num3, num3 * 2 + 1, num3 * 2 + 1);
					cellRect.ClipInsideRect(otherRect3);
				}
				CastPositionFinder.bestSpot = IntVec3.Invalid;
				CastPositionFinder.bestSpotPref = 0.001f;
				CastPositionFinder.maxRangeFromCasterSquared = CastPositionFinder.req.maxRangeFromCaster * CastPositionFinder.req.maxRangeFromCaster;
				CastPositionFinder.maxRangeFromTargetSquared = CastPositionFinder.req.maxRangeFromTarget * CastPositionFinder.req.maxRangeFromTarget;
				CastPositionFinder.maxRangeFromLocusSquared = CastPositionFinder.req.maxRangeFromLocus * CastPositionFinder.req.maxRangeFromLocus;
				CastPositionFinder.rangeFromTarget = (CastPositionFinder.req.caster.Position - CastPositionFinder.req.target.Position).LengthHorizontal;
				CastPositionFinder.rangeFromTargetSquared = (float)(CastPositionFinder.req.caster.Position - CastPositionFinder.req.target.Position).LengthHorizontalSquared;
				CastPositionFinder.optimalRangeSquared = (float)(CastPositionFinder.verb.verbProps.range * 0.800000011920929 * (CastPositionFinder.verb.verbProps.range * 0.800000011920929));
				CastPositionFinder.EvaluateCell(CastPositionFinder.req.caster.Position);
				if ((double)CastPositionFinder.bestSpotPref >= 1.0)
				{
					dest = CastPositionFinder.req.caster.Position;
					result = true;
				}
				else
				{
					float slope = (float)(-1.0 / CellLine.Between(CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Position).Slope);
					CellLine cellLine = new CellLine(CastPositionFinder.req.target.Position, slope);
					bool flag = cellLine.CellIsAbove(CastPositionFinder.req.caster.Position);
					Profiler.BeginSample("TryFindCastPosition scan near side");
					CellRect.CellRectIterator iterator = cellRect.GetIterator();
					while (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						if (cellLine.CellIsAbove(current) == flag && cellRect.Contains(current))
						{
							CastPositionFinder.EvaluateCell(current);
						}
						iterator.MoveNext();
					}
					Profiler.EndSample();
					if (CastPositionFinder.bestSpot.IsValid && CastPositionFinder.bestSpotPref > 0.33000001311302185)
					{
						dest = CastPositionFinder.bestSpot;
						result = true;
					}
					else
					{
						Profiler.BeginSample("TryFindCastPosition scan far side");
						CellRect.CellRectIterator iterator2 = cellRect.GetIterator();
						while (!iterator2.Done())
						{
							IntVec3 current2 = iterator2.Current;
							if (cellLine.CellIsAbove(current2) != flag && cellRect.Contains(current2))
							{
								CastPositionFinder.EvaluateCell(current2);
							}
							iterator2.MoveNext();
						}
						Profiler.EndSample();
						if (CastPositionFinder.bestSpot.IsValid)
						{
							dest = CastPositionFinder.bestSpot;
							result = true;
						}
						else
						{
							dest = CastPositionFinder.casterLoc;
							result = false;
						}
					}
				}
			}
			goto IL_05c2;
			IL_05c2:
			return result;
		}

		private static void EvaluateCell(IntVec3 c)
		{
			if (CastPositionFinder.maxRangeFromTargetSquared > 0.0099999997764825821 && CastPositionFinder.maxRangeFromTargetSquared < 250000.0 && (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared > CastPositionFinder.maxRangeFromTargetSquared)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0f, "range target", 50);
				}
			}
			else if ((double)CastPositionFinder.maxRangeFromLocusSquared > 0.01 && (float)(c - CastPositionFinder.req.locus).LengthHorizontalSquared > CastPositionFinder.maxRangeFromLocusSquared)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.1f, "range home", 50);
				}
			}
			else
			{
				if (CastPositionFinder.maxRangeFromCasterSquared > 0.0099999997764825821)
				{
					CastPositionFinder.rangeFromCasterToCellSquared = (float)(c - CastPositionFinder.req.caster.Position).LengthHorizontalSquared;
					if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.maxRangeFromCasterSquared)
					{
						if (DebugViewSettings.drawCastPositionSearch)
						{
							CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.2f, "range caster", 50);
						}
						return;
					}
				}
				if (c.Walkable(CastPositionFinder.req.caster.Map))
				{
					if (CastPositionFinder.req.maxRegionsRadius > 0 && c.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable).mark != CastPositionFinder.inRadiusMark)
					{
						if (DebugViewSettings.drawCastPositionSearch)
						{
							CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.64f, "reg radius", 50);
						}
					}
					else if (!CastPositionFinder.req.caster.Map.reachability.CanReach(CastPositionFinder.req.caster.Position, c, PathEndMode.OnCell, TraverseParms.For(CastPositionFinder.req.caster, Danger.Some, TraverseMode.ByPawn, false)))
					{
						if (DebugViewSettings.drawCastPositionSearch)
						{
							CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.4f, "can't reach", 50);
						}
					}
					else
					{
						float num = CastPositionFinder.CastPositionPreference(c);
						if (CastPositionFinder.avoidGrid != null)
						{
							byte b = CastPositionFinder.avoidGrid[c];
							num *= Mathf.Max(0.1f, (float)((37.5 - (float)(int)b) / 37.5));
						}
						if (DebugViewSettings.drawCastPositionSearch)
						{
							CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, (float)(num / 4.0), num.ToString("F3"), 50);
						}
						if (!(num < CastPositionFinder.bestSpotPref))
						{
							if (!CastPositionFinder.verb.CanHitTargetFrom(c, CastPositionFinder.req.target))
							{
								if (DebugViewSettings.drawCastPositionSearch)
								{
									CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.6f, "can't hit", 50);
								}
							}
							else if (!CastPositionFinder.req.caster.Map.pawnDestinationReservationManager.CanReserve(c, CastPositionFinder.req.caster))
							{
								if (DebugViewSettings.drawCastPositionSearch)
								{
									CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, (float)(num * 0.89999997615814209), "resvd", 50);
								}
							}
							else if (PawnUtility.KnownDangerAt(c, CastPositionFinder.req.caster))
							{
								if (DebugViewSettings.drawCastPositionSearch)
								{
									CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.9f, "danger", 50);
								}
							}
							else
							{
								CastPositionFinder.bestSpot = c;
								CastPositionFinder.bestSpotPref = num;
							}
						}
					}
				}
			}
		}

		private static float CastPositionPreference(IntVec3 c)
		{
			bool flag = true;
			List<Thing> list = CastPositionFinder.req.caster.Map.thingGrid.ThingsListAtFast(c);
			int num = 0;
			float result;
			while (true)
			{
				if (num < list.Count)
				{
					Thing thing = list[num];
					Fire fire = thing as Fire;
					if (fire != null && fire.parent == null)
					{
						result = -1f;
						break;
					}
					if (thing.def.passability == Traversability.PassThroughOnly)
					{
						flag = false;
					}
					num++;
					continue;
				}
				float num2 = 0.3f;
				if (CastPositionFinder.req.caster.kindDef.aiAvoidCover)
				{
					num2 = (float)(num2 + (8.0 - CoverUtility.TotalSurroundingCoverScore(c, CastPositionFinder.req.caster.Map)));
				}
				if (CastPositionFinder.req.wantCoverFromTarget)
				{
					num2 += CoverUtility.CalculateOverallBlockChance(c, CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Map);
				}
				float num3 = (CastPositionFinder.req.caster.Position - c).LengthHorizontal;
				if (CastPositionFinder.rangeFromTarget > 100.0)
				{
					num3 = (float)(num3 - (CastPositionFinder.rangeFromTarget - 100.0));
					if (num3 < 0.0)
					{
						num3 = 0f;
					}
				}
				num2 *= Mathf.Pow(0.967f, num3);
				float num4 = 1f;
				CastPositionFinder.rangeFromTargetToCellSquared = (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared;
				float num5 = Mathf.Abs(CastPositionFinder.rangeFromTargetToCellSquared - CastPositionFinder.optimalRangeSquared) / CastPositionFinder.optimalRangeSquared;
				num5 = (float)(1.0 - num5);
				num5 = (float)(0.699999988079071 + 0.30000001192092896 * num5);
				num4 *= num5;
				if (CastPositionFinder.rangeFromTargetToCellSquared < 25.0)
				{
					num4 = (float)(num4 * 0.5);
				}
				num2 *= num4;
				if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.rangeFromTargetSquared)
				{
					num2 = (float)(num2 * 0.40000000596046448);
				}
				if (!flag)
				{
					num2 = (float)(num2 * 0.20000000298023224);
				}
				result = num2;
				break;
			}
			return result;
		}
	}
}
