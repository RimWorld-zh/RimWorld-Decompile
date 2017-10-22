using RimWorld;

namespace Verse.AI.Group
{
	public class LordToil_ExitMap : LordToil
	{
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		protected LordToilData_ExitMap Data
		{
			get
			{
				return (LordToilData_ExitMap)base.data;
			}
		}

		public LordToil_ExitMap(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			base.data = new LordToilData_ExitMap();
			this.Data.locomotion = locomotion;
			this.Data.canDig = canDig;
		}

		public override void UpdateAllDuties()
		{
			LordToilData_ExitMap data = this.Data;
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapBest);
				pawnDuty.locomotion = data.locomotion;
				pawnDuty.canDig = data.canDig;
				base.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}
	}
}
