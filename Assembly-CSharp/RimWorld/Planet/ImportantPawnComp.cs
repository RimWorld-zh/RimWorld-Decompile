using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000622 RID: 1570
	public abstract class ImportantPawnComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x0400126F RID: 4719
		public ThingOwner<Pawn> pawn;

		// Token: 0x04001270 RID: 4720
		private const float AutoFoodLevel = 0.8f;

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0011259E File Offset: 0x0011099E
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep);
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001FE6 RID: 8166
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06001FE7 RID: 8167 RVA: 0x001125B5 File Offset: 0x001109B5
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

		// Token: 0x06001FE8 RID: 8168 RVA: 0x001125EA File Offset: 0x001109EA
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x001125FC File Offset: 0x001109FC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00112618 File Offset: 0x00110A18
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

		// Token: 0x06001FEB RID: 8171 RVA: 0x001126C6 File Offset: 0x00110AC6
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06001FEC RID: 8172
		protected abstract void RemovePawnOnWorldObjectRemoved();
	}
}
