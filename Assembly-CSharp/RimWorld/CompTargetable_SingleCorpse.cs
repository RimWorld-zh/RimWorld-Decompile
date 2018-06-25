using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075D RID: 1885
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060029A9 RID: 10665 RVA: 0x00161F80 File Offset: 0x00160380
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x00161F98 File Offset: 0x00160398
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

		// Token: 0x060029AB RID: 10667 RVA: 0x00161FE4 File Offset: 0x001603E4
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
