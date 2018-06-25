using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x001623B4 File Offset: 0x001607B4
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x001623CC File Offset: 0x001607CC
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x00162408 File Offset: 0x00160808
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
