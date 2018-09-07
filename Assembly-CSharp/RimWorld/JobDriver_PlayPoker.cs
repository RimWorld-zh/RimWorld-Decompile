using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlayPoker : JobDriver_SitFacingBuilding
	{
		[CompilerGenerated]
		private static Func<EffecterDef> <>f__am$cache0;

		public JobDriver_PlayPoker()
		{
		}

		protected override void ModifyPlayToil(Toil toil)
		{
			base.ModifyPlayToil(toil);
			toil.WithEffect(() => EffecterDefOf.PlayPoker, () => base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
		}

		[CompilerGenerated]
		private static EffecterDef <ModifyPlayToil>m__0()
		{
			return EffecterDefOf.PlayPoker;
		}

		[CompilerGenerated]
		private LocalTargetInfo <ModifyPlayToil>m__1()
		{
			return base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position);
		}
	}
}
