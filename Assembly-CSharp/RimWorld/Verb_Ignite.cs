using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D4 RID: 2516
	public class Verb_Ignite : Verb
	{
		// Token: 0x06003866 RID: 14438 RVA: 0x001E1D85 File Offset: 0x001E0185
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x001E1D9C File Offset: 0x001E019C
		protected override bool TryCastShot()
		{
			Thing thing = this.currentTarget.Thing;
			Pawn casterPawn = base.CasterPawn;
			FireUtility.TryStartFireIn(thing.OccupiedRect().ClosestCellTo(casterPawn.Position), casterPawn.Map, 0.3f);
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			return true;
		}
	}
}
