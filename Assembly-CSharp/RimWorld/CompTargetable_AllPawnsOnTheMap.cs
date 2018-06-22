using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075A RID: 1882
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x00161ADC File Offset: 0x0015FEDC
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x00161AF4 File Offset: 0x0015FEF4
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x00161B30 File Offset: 0x0015FF30
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			if (this.parent.MapHeld == null)
			{
				yield break;
			}
			TargetingParameters tp = this.GetTargetingParameters();
			foreach (Pawn p in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (tp.CanTarget(p))
				{
					yield return p;
				}
			}
			yield break;
		}
	}
}
