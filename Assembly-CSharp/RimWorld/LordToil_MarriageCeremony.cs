using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_MarriageCeremony : LordToil
	{
		private Pawn firstPawn;

		private Pawn secondPawn;

		private IntVec3 spot;

		public static readonly IntVec3 OtherFianceNoMarriageSpotCellOffset = new IntVec3(-1, 0, 0);

		public LordToilData_MarriageCeremony Data
		{
			get
			{
				return (LordToilData_MarriageCeremony)base.data;
			}
		}

		public LordToil_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			base.data = new LordToilData_MarriageCeremony();
		}

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

		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return (!this.IsFiance(p)) ? DutyDefOf.Spectate.hook : DutyDefOf.MarryPawn.hook;
		}

		public override void UpdateAllDuties()
		{
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = base.lord.ownedPawns[i];
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

		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		public IntVec3 FianceStandingSpotFor(Pawn pawn)
		{
			Pawn pawn2 = null;
			if (this.firstPawn == pawn)
			{
				pawn2 = this.secondPawn;
				goto IL_0049;
			}
			if (this.secondPawn == pawn)
			{
				pawn2 = this.firstPawn;
				goto IL_0049;
			}
			Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.");
			IntVec3 result = IntVec3.Invalid;
			goto IL_00a0;
			IL_0049:
			result = ((pawn.thingIDNumber >= pawn2.thingIDNumber) ? ((this.GetMarriageSpotAt(this.spot) == null) ? (this.spot + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset) : this.FindCellForOtherPawnAtMarriageSpot(this.spot)) : this.spot);
			goto IL_00a0;
			IL_00a0:
			return result;
		}

		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.def == ThingDefOf.MarriageSpot));
		}

		private IntVec3 FindCellForOtherPawnAtMarriageSpot(IntVec3 cell)
		{
			Thing marriageSpotAt = this.GetMarriageSpotAt(cell);
			CellRect cellRect = marriageSpotAt.OccupiedRect();
			int num = cellRect.minX;
			IntVec3 result;
			while (true)
			{
				int num2;
				if (num <= cellRect.maxX)
				{
					num2 = cellRect.minZ;
					while (num2 <= cellRect.maxZ)
					{
						if (cell.x == num && cell.z == num2)
						{
							num2++;
							continue;
						}
						goto IL_0046;
					}
					num++;
					continue;
				}
				Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.");
				result = IntVec3.Invalid;
				break;
				IL_0046:
				result = new IntVec3(num, 0, num2);
				break;
			}
			return result;
		}

		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}
	}
}
