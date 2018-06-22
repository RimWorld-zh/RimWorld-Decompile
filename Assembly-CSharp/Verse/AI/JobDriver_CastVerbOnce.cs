using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A3B RID: 2619
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x06003A20 RID: 14880 RVA: 0x001EC24C File Offset: 0x001EA64C
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

		// Token: 0x06003A21 RID: 14881 RVA: 0x001EC2BC File Offset: 0x001EA6BC
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x001EC2D4 File Offset: 0x001EA6D4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
