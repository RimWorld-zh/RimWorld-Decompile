using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060029A7 RID: 10663 RVA: 0x00161904 File Offset: 0x0015FD04
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x0016191C File Offset: 0x0015FD1C
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x00161958 File Offset: 0x0015FD58
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
