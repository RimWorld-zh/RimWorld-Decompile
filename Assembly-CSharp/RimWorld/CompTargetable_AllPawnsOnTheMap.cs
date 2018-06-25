using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075C RID: 1884
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x00161E8C File Offset: 0x0016028C
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x00161EA4 File Offset: 0x001602A4
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x00161EE0 File Offset: 0x001602E0
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
