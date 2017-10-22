using Verse;

namespace RimWorld
{
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		public Tale_DoublePawnKilledBy()
		{
		}

		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				base.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
