using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D6 RID: 2518
	public class Verb_Ignite : Verb
	{
		// Token: 0x06003866 RID: 14438 RVA: 0x001E19B5 File Offset: 0x001DFDB5
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x001E19CC File Offset: 0x001DFDCC
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
