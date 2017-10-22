using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_Sleep : LordToil
	{
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				base.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
