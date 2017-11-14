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
			if (this.IsFiance(p))
			{
				return DutyDefOf.MarryPawn.hook;
			}
			return DutyDefOf.Spectate.hook;
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
				goto IL_0042;
			}
			if (this.secondPawn == pawn)
			{
				pawn2 = this.firstPawn;
				goto IL_0042;
			}
			Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.");
			return IntVec3.Invalid;
			IL_0042:
			if (pawn.thingIDNumber < pawn2.thingIDNumber)
			{
				return this.spot;
			}
			if (this.GetMarriageSpotAt(this.spot) != null)
			{
				return this.FindCellForOtherPawnAtMarriageSpot(this.spot);
			}
			return this.spot + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
		}

		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Thing x) => x.def == ThingDefOf.MarriageSpot);
		}

		private IntVec3 FindCellForOtherPawnAtMarriageSpot(IntVec3 cell)
		{
			Thing marriageSpotAt = this.GetMarriageSpotAt(cell);
			CellRect cellRect = marriageSpotAt.OccupiedRect();
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				int num = cellRect.minZ;
				while (num <= cellRect.maxZ)
				{
					if (cell.x == i && cell.z == num)
					{
						num++;
						continue;
					}
					return new IntVec3(i, 0, num);
				}
			}
			Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.");
			return IntVec3.Invalid;
		}

		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}
	}
}
