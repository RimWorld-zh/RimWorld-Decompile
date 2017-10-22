using RimWorld;

namespace Verse.AI.Group
{
	public class LordToil_DefendPoint : LordToil
	{
		private bool allowSatisfyLongNeeds = true;

		protected LordToilData_DefendPoint Data
		{
			get
			{
				return (LordToilData_DefendPoint)base.data;
			}
		}

		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.defendPoint;
			}
		}

		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return this.allowSatisfyLongNeeds;
			}
		}

		public LordToil_DefendPoint(bool canSatisfyLongNeeds = true)
		{
			this.allowSatisfyLongNeeds = canSatisfyLongNeeds;
			base.data = new LordToilData_DefendPoint();
		}

		public LordToil_DefendPoint(IntVec3 defendPoint, float defendRadius = 28f) : this(true)
		{
			this.Data.defendPoint = defendPoint;
			this.Data.defendRadius = defendRadius;
		}

		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = this.Data;
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				base.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
				base.lord.ownedPawns[i].mindState.duty.focusSecond = data.defendPoint;
				base.lord.ownedPawns[i].mindState.duty.radius = data.defendRadius;
			}
		}

		public void SetDefendPoint(IntVec3 defendPoint)
		{
			this.Data.defendPoint = defendPoint;
		}
	}
}
