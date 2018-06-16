using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000058 RID: 88
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0001C866 File Offset: 0x0001AC66
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
		}
	}
}
