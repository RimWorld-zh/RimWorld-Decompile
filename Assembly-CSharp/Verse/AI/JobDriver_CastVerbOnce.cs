using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3E RID: 2622
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x06003A25 RID: 14885 RVA: 0x001EC6A4 File Offset: 0x001EAAA4
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

		// Token: 0x06003A26 RID: 14886 RVA: 0x001EC714 File Offset: 0x001EAB14
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x001EC72C File Offset: 0x001EAB2C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
