using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3D RID: 2621
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x06003A24 RID: 14884 RVA: 0x001EC378 File Offset: 0x001EA778
		public override string GetReport()
		{
			string text;
			if (base.TargetA.HasThing)
			{
				text = base.TargetThingA.LabelCap;
			}
			else
			{
				text = "AreaLower".Translate();
			}
			return "UsingVerb".Translate(new object[]
			{
				this.job.verbToUse.verbProps.label,
				text
			});
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x001EC3E8 File Offset: 0x001EA7E8
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x001EC400 File Offset: 0x001EA800
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
