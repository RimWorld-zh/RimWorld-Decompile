using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		private static List<Thing> tmpThings = new List<Thing>();

		public MentalStateWorker_SadisticRageTantrum()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!base.StateCanOccur(pawn))
			{
				result = false;
			}
			else
			{
				MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
				TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x), 0, 40);
				bool flag = MentalStateWorker_SadisticRageTantrum.tmpThings.Any<Thing>();
				MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
				result = flag;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MentalStateWorker_SadisticRageTantrum()
		{
		}

		[CompilerGenerated]
		private sealed class <StateCanOccur>c__AnonStorey0
		{
			internal Pawn pawn;

			public <StateCanOccur>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
			}
		}
	}
}
