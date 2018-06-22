using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A1 RID: 417
	public class LordToil_MarriageCeremony : LordToil
	{
		// Token: 0x060008A0 RID: 2208 RVA: 0x00051AA2 File Offset: 0x0004FEA2
		public LordToil_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.data = new LordToilData_MarriageCeremony();
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00051ACC File Offset: 0x0004FECC
		public LordToilData_MarriageCeremony Data
		{
			get
			{
				return (LordToilData_MarriageCeremony)this.data;
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00051AEC File Offset: 0x0004FEEC
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = this.CalculateSpectateRect();
			SpectateRectSide allowedSides = SpectateRectSide.All;
			if (this.Data.spectateRect.Width > this.Data.spectateRect.Height)
			{
				allowedSides = SpectateRectSide.Vertical;
			}
			else if (this.Data.spectateRect.Height > this.Data.spectateRect.Width)
			{
				allowedSides = SpectateRectSide.Horizontal;
			}
			this.Data.spectateRectAllowedSides = SpectatorCellFinder.FindSingleBestSide(this.Data.spectateRect, base.Map, allowedSides, 1);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00051B8C File Offset: 0x0004FF8C
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			ThinkTreeDutyHook hook;
			if (this.IsFiance(p))
			{
				hook = DutyDefOf.MarryPawn.hook;
			}
			else
			{
				hook = DutyDefOf.Spectate.hook;
			}
			return hook;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00051BC8 File Offset: 0x0004FFC8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsFiance(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.MarryPawn, this.FianceStandingSpotFor(pawn), -1f);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00051C80 File Offset: 0x00050080
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00051CB0 File Offset: 0x000500B0
		public IntVec3 FianceStandingSpotFor(Pawn pawn)
		{
			Pawn pawn2;
			if (this.firstPawn == pawn)
			{
				pawn2 = this.secondPawn;
			}
			else
			{
				if (this.secondPawn != pawn)
				{
					Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.", false);
					return IntVec3.Invalid;
				}
				pawn2 = this.firstPawn;
			}
			IntVec3 result;
			if (pawn.thingIDNumber < pawn2.thingIDNumber)
			{
				result = this.spot;
			}
			else if (this.GetMarriageSpotAt(this.spot) != null)
			{
				result = this.FindCellForOtherPawnAtMarriageSpot(this.spot);
			}
			else
			{
				result = this.spot + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
			}
			return result;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00051D60 File Offset: 0x00050160
		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Thing x) => x.def == ThingDefOf.MarriageSpot);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00051DA4 File Offset: 0x000501A4
		private IntVec3 FindCellForOtherPawnAtMarriageSpot(IntVec3 cell)
		{
			Thing marriageSpotAt = this.GetMarriageSpotAt(cell);
			CellRect cellRect = marriageSpotAt.OccupiedRect();
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					if (cell.x != i || cell.z != j)
					{
						return new IntVec3(i, 0, j);
					}
				}
			}
			Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.", false);
			return IntVec3.Invalid;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00051E44 File Offset: 0x00050244
		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}

		// Token: 0x040003A3 RID: 931
		private Pawn firstPawn;

		// Token: 0x040003A4 RID: 932
		private Pawn secondPawn;

		// Token: 0x040003A5 RID: 933
		private IntVec3 spot;

		// Token: 0x040003A6 RID: 934
		public static readonly IntVec3 OtherFianceNoMarriageSpotCellOffset = new IntVec3(-1, 0, 0);
	}
}
