using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000620 RID: 1568
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06001FE2 RID: 8162 RVA: 0x001121E6 File Offset: 0x001105E6
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001FE3 RID: 8163
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06001FE4 RID: 8164 RVA: 0x001121FD File Offset: 0x001105FD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.ImportantPawnCompPostLoadInit(this);
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00112232 File Offset: 0x00110632
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00112244 File Offset: 0x00110644
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x00112260 File Offset: 0x00110660
		public override void CompTick()
		{
			base.CompTick();
			bool any = this.pawn.Any;
			this.pawn.ThingOwnerTick(true);
			if (any && !base.ParentHasMap)
			{
				if (!this.pawn.Any || this.pawn[0].Destroyed)
				{
					Find.WorldObjects.Remove(this.parent);
				}
				else
				{
					Pawn pawn = this.pawn[0];
					if (pawn.needs.food != null)
					{
						pawn.needs.food.ForceSetLevel(0.8f);
					}
				}
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0011230E File Offset: 0x0011070E
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06001FE9 RID: 8169
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x0400126B RID: 4715
		public ThingOwner<Pawn> pawn;

		// Token: 0x0400126C RID: 4716
		private const float AutoFoodLevel = 0.8f;
	}
}
