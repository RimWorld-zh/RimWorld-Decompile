using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_PrisonerEscape : LordToil_Travel
	{
		private int sapperThingID;

		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.dest;
			}
		}

		private LordToilData_Travel Data
		{
			get
			{
				return (LordToilData_Travel)base.data;
			}
		}

		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		protected override float AllArrivedCheckRadius
		{
			get
			{
				return 14f;
			}
		}

		public LordToil_PrisonerEscape(IntVec3 dest, int sapperThingID) : base(dest)
		{
			this.sapperThingID = sapperThingID;
		}

		public override void UpdateAllDuties()
		{
			LordToilData_Travel data = this.Data;
			Pawn leader = this.GetLeader();
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = base.lord.ownedPawns[i];
				if (this.IsSapper(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscapeSapper, data.dest, -1f);
				}
				else if (leader == null || pawn == leader)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, data.dest, -1f);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrisonerEscape, (Thing)leader, 10f);
				}
			}
		}

		public override void LordToilTick()
		{
			base.LordToilTick();
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = base.lord.ownedPawns[i];
				pawn.guilt.Notify_Guilty();
			}
		}

		private Pawn GetLeader()
		{
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < base.lord.ownedPawns.Count)
				{
					if (!base.lord.ownedPawns[num].Downed && this.IsSapper(base.lord.ownedPawns[num]))
					{
						result = base.lord.ownedPawns[num];
						break;
					}
					num++;
					continue;
				}
				int i;
				for (i = 0; i < base.lord.ownedPawns.Count; i++)
				{
					if (!base.lord.ownedPawns[i].Downed)
						goto IL_0095;
				}
				result = null;
				break;
				IL_0095:
				result = base.lord.ownedPawns[i];
				break;
			}
			return result;
		}

		private bool IsSapper(Pawn p)
		{
			return p.thingIDNumber == this.sapperThingID;
		}
	}
}
