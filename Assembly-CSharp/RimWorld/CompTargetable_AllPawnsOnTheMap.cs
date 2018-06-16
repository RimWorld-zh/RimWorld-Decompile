using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x00161870 File Offset: 0x0015FC70
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00161888 File Offset: 0x0015FC88
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x001618C4 File Offset: 0x0015FCC4
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
