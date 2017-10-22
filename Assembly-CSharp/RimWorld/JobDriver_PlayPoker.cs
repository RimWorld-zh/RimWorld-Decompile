using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect((Func<EffecterDef>)(() => EffecterDefOf.PlayPoker), (Func<LocalTargetInfo>)(() => base.TargetA.Thing.OccupiedRect().ClosestCellTo(base.pawn.Position)));
		}
	}
}
