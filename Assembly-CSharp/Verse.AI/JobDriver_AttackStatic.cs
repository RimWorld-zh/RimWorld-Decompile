using RimWorld;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_AttackStatic : JobDriver
	{
		private bool startedIncapacitated;

		private int numAttacksMade;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
