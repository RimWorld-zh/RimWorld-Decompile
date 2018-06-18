using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x00161E2C File Offset: 0x0016022C
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x00161E44 File Offset: 0x00160244
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x00161E80 File Offset: 0x00160280
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
