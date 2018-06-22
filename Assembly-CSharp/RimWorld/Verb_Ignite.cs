using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D2 RID: 2514
	public class Verb_Ignite : Verb
	{
		// Token: 0x06003862 RID: 14434 RVA: 0x001E1C61 File Offset: 0x001E0061
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x001E1C78 File Offset: 0x001E0078
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
