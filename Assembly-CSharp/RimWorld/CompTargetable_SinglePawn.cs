using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x00161D98 File Offset: 0x00160198
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x00161DB0 File Offset: 0x001601B0
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x00161DEC File Offset: 0x001601EC
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
