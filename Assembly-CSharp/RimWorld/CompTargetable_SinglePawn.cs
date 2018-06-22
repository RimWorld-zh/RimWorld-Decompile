using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075C RID: 1884
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x00162004 File Offset: 0x00160404
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x0016201C File Offset: 0x0016041C
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x00162058 File Offset: 0x00160458
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
