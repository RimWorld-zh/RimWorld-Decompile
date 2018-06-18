using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3F RID: 2623
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x06003A26 RID: 14886 RVA: 0x001EC00C File Offset: 0x001EA40C
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

		// Token: 0x06003A27 RID: 14887 RVA: 0x001EC07C File Offset: 0x001EA47C
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x001EC094 File Offset: 0x001EA494
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
