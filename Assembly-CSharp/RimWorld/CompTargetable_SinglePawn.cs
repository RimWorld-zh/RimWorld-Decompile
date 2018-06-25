using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x00162154 File Offset: 0x00160554
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x0016216C File Offset: 0x0016056C
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x001621A8 File Offset: 0x001605A8
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
