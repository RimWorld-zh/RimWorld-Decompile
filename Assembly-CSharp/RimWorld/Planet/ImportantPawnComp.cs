using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000622 RID: 1570
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x0400126B RID: 4715
		public ThingOwner<Pawn> pawn;

		// Token: 0x0400126C RID: 4716
		private const float AutoFoodLevel = 0.8f;

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00112336 File Offset: 0x00110736
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001FE7 RID: 8167
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0011234D File Offset: 0x0011074D
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

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00112382 File Offset: 0x00110782
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00112394 File Offset: 0x00110794
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x001123B0 File Offset: 0x001107B0
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

		// Token: 0x06001FEC RID: 8172 RVA: 0x0011245E File Offset: 0x0011085E
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06001FED RID: 8173
		protected abstract void RemovePawnOnWorldObjectRemoved();
	}
}
