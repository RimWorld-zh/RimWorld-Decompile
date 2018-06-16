using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x00161BC4 File Offset: 0x0015FFC4
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x00161BDC File Offset: 0x0015FFDC
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetBuildings = false,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = ((TargetInfo x) => x.Thing is Corpse && base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x00161C28 File Offset: 0x00160028
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
