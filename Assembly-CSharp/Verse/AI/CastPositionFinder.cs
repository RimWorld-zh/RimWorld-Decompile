using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse.AI
{
	// Token: 0x02000AE3 RID: 2787
	public static class CastPositionFinder
	{
		// Token: 0x040026F3 RID: 9971
		private static CastPositionRequest req;

		// Token: 0x040026F4 RID: 9972
		private static IntVec3 casterLoc;

		// Token: 0x040026F5 RID: 9973
		private static IntVec3 targetLoc;

		// Token: 0x040026F6 RID: 9974
		private static Verb verb;

		// Token: 0x040026F7 RID: 9975
		private static float rangeFromTarget;

		// Token: 0x040026F8 RID: 9976
		private static float rangeFromTargetSquared;

		// Token: 0x040026F9 RID: 9977
		private static float optimalRangeSquared;

		// Token: 0x040026FA RID: 9978
		private static float rangeFromCasterToCellSquared;

		// Token: 0x040026FB RID: 9979
		private static float rangeFromTargetToCellSquared;

		// Token: 0x040026FC RID: 9980
		private static int inRadiusMark;

		// Token: 0x040026FD RID: 9981
		private static ByteGrid avoidGrid;

		// Token: 0x040026FE RID: 9982
		private static float maxRangeFromCasterSquared;

		// Token: 0x040026FF RID: 9983
		private static float maxRangeFromTargetSquared;

		// Token: 0x04002700 RID: 9984
		private static float maxRangeFromLocusSquared;

		// Token: 0x04002701 RID: 9985
		private static IntVec3 bestSpot = IntVec3.Invalid;

		// Token: 0x04002702 RID: 9986
		private static float bestSpotPref = 0.001f;

		// Token: 0x04002703 RID: 9987
		private const float BaseAIPreference = 0.3f;

		// Token: 0x04002704 RID: 9988
		private const float MinimumPreferredRange = 5f;

		// Token: 0x04002705 RID: 9989
		private const float OptimalRangeFactor = 0.8f;

		// Token: 0x04002706 RID: 9990
		private const float OptimalRangeFactorImportance = 0.3f;

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00208864 File Offset: 0x00206C64
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
				Log.Error(CastPositionFinder.req.caster + " tried to find casting position without a verb.", false);
				dest = IntVec3.Invalid;
				result = false;
			}
			else
			{
				if (CastPositionFinder.req.maxRegions > 0)
				{
					Region region = CastPositionFinder.casterLoc.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable);
					if (region == null)
					{
						Log.Error("TryFindCastPosition requiring region traversal but root region is null.", false);
						dest = IntVec3.Invalid;
						return false;
					}
					CastPositionFinder.inRadiusMark = Rand.Int;
					RegionTraverser.MarkRegionsBFS(region, null, newReq.maxRegions, CastPositionFinder.inRadiusMark, RegionType.Set_Passable);
					if (CastPositionFinder.req.maxRangeFromLocus > 0.01f)
					{
						Region locusReg = CastPositionFinder.req.locus.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable);
						if (locusReg == null)
						{
							Log.Error("locus " + CastPositionFinder.req.locus + " has no region", false);
							dest = IntVec3.Invalid;
							return false;
						}
						if (locusReg.mark != CastPositionFinder.inRadiusMark)
						{
							CastPositionFinder.inRadiusMark = Rand.Int;
							RegionTraverser.BreadthFirstTraverse(region, null, delegate(Region r)
							{
								r.mark = CastPositionFinder.inRadiusMark;
								CastPositionFinder.req.maxRegions = CastPositionFinder.req.maxRegions + 1;
								return r == locusReg;
							}, 999999, RegionType.Set_Passable);
						}
					}
				}
				CellRect cellRect = CellRect.WholeMap(CastPositionFinder.req.caster.Map);
				if (CastPositionFinder.req.maxRangeFromCaster > 0.01f)
				{
					int num = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromCaster);
					CellRect otherRect = new CellRect(CastPositionFinder.casterLoc.x - num, CastPositionFinder.casterLoc.z - num, num * 2 + 1, num * 2 + 1);
					cellRect.ClipInsideRect(otherRect);
				}
				int num2 = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromTarget);
				CellRect otherRect2 = new CellRect(CastPositionFinder.targetLoc.x - num2, CastPositionFinder.targetLoc.z - num2, num2 * 2 + 1, num2 * 2 + 1);
				cellRect.ClipInsideRect(otherRect2);
				if (CastPositionFinder.req.maxRangeFromLocus > 0.01f)
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
				CastPositionFinder.optimalRangeSquared = CastPositionFinder.verb.verbProps.range * 0.8f * (CastPositionFinder.verb.verbProps.range * 0.8f);
				CastPositionFinder.EvaluateCell(CastPositionFinder.req.caster.Position);
				if ((double)CastPositionFinder.bestSpotPref >= 1.0)
				{
					dest = CastPositionFinder.req.caster.Position;
					result = true;
				}
				else
				{
					float slope = -1f / CellLine.Between(CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Position).Slope;
					CellLine cellLine = new CellLine(CastPositionFinder.req.target.Position, slope);
					bool flag = cellLine.CellIsAbove(CastPositionFinder.req.caster.Position);
					Profiler.BeginSample("TryFindCastPosition scan near side");
					CellRect.CellRectIterator iterator = cellRect.GetIterator();
					while (!iterator.Done())
					{
						IntVec3 c = iterator.Current;
						if (cellLine.CellIsAbove(c) == flag && cellRect.Contains(c))
						{
							CastPositionFinder.EvaluateCell(c);
						}
						iterator.MoveNext();
					}
					Profiler.EndSample();
					if (CastPositionFinder.bestSpot.IsValid && CastPositionFinder.bestSpotPref > 0.33f)
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
							IntVec3 c2 = iterator2.Current;
							if (cellLine.CellIsAbove(c2) != flag && cellRect.Contains(c2))
							{
								CastPositionFinder.EvaluateCell(c2);
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
			return result;
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x00208E0C File Offset: 0x0020720C
		private static void EvaluateCell(IntVec3 c)
		{
			if (CastPositionFinder.maxRangeFromTargetSquared > 0.01f && CastPositionFinder.maxRangeFromTargetSquared < 250000f)
			{
				if ((float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared > CastPositionFinder.maxRangeFromTargetSquared)
				{
					if (DebugViewSettings.drawCastPositionSearch)
					{
						CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0f, "range target", 50);
					}
					return;
				}
			}
			if ((double)CastPositionFinder.maxRangeFromLocusSquared > 0.01)
			{
				if ((float)(c - CastPositionFinder.req.locus).LengthHorizontalSquared > CastPositionFinder.maxRangeFromLocusSquared)
				{
					if (DebugViewSettings.drawCastPositionSearch)
					{
						CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.1f, "range home", 50);
					}
					return;
				}
			}
			if (CastPositionFinder.maxRangeFromCasterSquared > 0.01f)
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
				if (CastPositionFinder.req.maxRegions > 0 && c.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable).mark != CastPositionFinder.inRadiusMark)
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
						num *= Mathf.Max(0.1f, (37.5f - (float)b) / 37.5f);
					}
					if (DebugViewSettings.drawCastPositionSearch)
					{
						CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, num / 4f, num.ToString("F3"), 50);
					}
					if (num >= CastPositionFinder.bestSpotPref)
					{
						if (!CastPositionFinder.verb.CanHitTargetFrom(c, CastPositionFinder.req.target))
						{
							if (DebugViewSettings.drawCastPositionSearch)
							{
								CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.6f, "can't hit", 50);
							}
						}
						else if (!CastPositionFinder.req.caster.Map.pawnDestinationReservationManager.CanReserve(c, CastPositionFinder.req.caster, false))
						{
							if (DebugViewSettings.drawCastPositionSearch)
							{
								CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, num * 0.9f, "resvd", 50);
							}
						}
						else if (PawnUtility.KnownDangerAt(c, CastPositionFinder.req.caster.Map, CastPositionFinder.req.caster))
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

		// Token: 0x06003DBB RID: 15803 RVA: 0x00209228 File Offset: 0x00207628
		private static float CastPositionPreference(IntVec3 c)
		{
			bool flag = true;
			List<Thing> list = CastPositionFinder.req.caster.Map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				Fire fire = thing as Fire;
				if (fire != null && fire.parent == null)
				{
					return -1f;
				}
				if (thing.def.passability == Traversability.PassThroughOnly)
				{
					flag = false;
				}
			}
			float num = 0.3f;
			if (CastPositionFinder.req.caster.kindDef.aiAvoidCover)
			{
				num += 8f - CoverUtility.TotalSurroundingCoverScore(c, CastPositionFinder.req.caster.Map);
			}
			if (CastPositionFinder.req.wantCoverFromTarget)
			{
				num += CoverUtility.CalculateOverallBlockChance(c, CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Map);
			}
			float num2 = (CastPositionFinder.req.caster.Position - c).LengthHorizontal;
			if (CastPositionFinder.rangeFromTarget > 100f)
			{
				num2 -= CastPositionFinder.rangeFromTarget - 100f;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
			}
			num *= Mathf.Pow(0.967f, num2);
			float num3 = 1f;
			CastPositionFinder.rangeFromTargetToCellSquared = (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared;
			float num4 = Mathf.Abs(CastPositionFinder.rangeFromTargetToCellSquared - CastPositionFinder.optimalRangeSquared) / CastPositionFinder.optimalRangeSquared;
			num4 = 1f - num4;
			num4 = 0.7f + 0.3f * num4;
			num3 *= num4;
			if (CastPositionFinder.rangeFromTargetToCellSquared < 25f)
			{
				num3 *= 0.5f;
			}
			num *= num3;
			if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.rangeFromTargetSquared)
			{
				num *= 0.4f;
			}
			if (!flag)
			{
				num *= 0.2f;
			}
			return num;
		}
	}
}
