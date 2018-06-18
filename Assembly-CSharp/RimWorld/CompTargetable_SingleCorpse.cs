using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060029AC RID: 10668 RVA: 0x00161C58 File Offset: 0x00160058
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x00161C70 File Offset: 0x00160070
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

		// Token: 0x060029AE RID: 10670 RVA: 0x00161CBC File Offset: 0x001600BC
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
