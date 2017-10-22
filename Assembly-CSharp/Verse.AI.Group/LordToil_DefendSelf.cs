using RimWorld;

namespace Verse.AI.Group
{
	public class LordToil_DefendSelf : LordToil
	{
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				base.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, base.lord.ownedPawns[i].Position, -1f);
				base.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
