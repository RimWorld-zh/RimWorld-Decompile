using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000624 RID: 1572
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x00112192 File Offset: 0x00110592
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001FEC RID: 8172
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06001FED RID: 8173 RVA: 0x001121A9 File Offset: 0x001105A9
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

		// Token: 0x06001FEE RID: 8174 RVA: 0x001121DE File Offset: 0x001105DE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x001121F0 File Offset: 0x001105F0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0011220C File Offset: 0x0011060C
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

		// Token: 0x06001FF1 RID: 8177 RVA: 0x001122BA File Offset: 0x001106BA
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06001FF2 RID: 8178
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x0400126E RID: 4718
		public ThingOwner<Pawn> pawn;

		// Token: 0x0400126F RID: 4719
		private const float AutoFoodLevel = 0.8f;
	}
}
