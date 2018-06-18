using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D6 RID: 2518
	public class Verb_Ignite : Verb
	{
		// Token: 0x06003868 RID: 14440 RVA: 0x001E1A89 File Offset: 0x001DFE89
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x001E1AA0 File Offset: 0x001DFEA0
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
