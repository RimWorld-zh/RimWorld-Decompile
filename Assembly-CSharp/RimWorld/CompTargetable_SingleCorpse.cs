using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075B RID: 1883
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x00161E30 File Offset: 0x00160230
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00161E48 File Offset: 0x00160248
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

		// Token: 0x060029A7 RID: 10663 RVA: 0x00161E94 File Offset: 0x00160294
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
