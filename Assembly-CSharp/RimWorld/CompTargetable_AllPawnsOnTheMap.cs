using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075C RID: 1884
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x00161C2C File Offset: 0x0016002C
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x00161C44 File Offset: 0x00160044
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00161C80 File Offset: 0x00160080
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
