using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000621 RID: 1569
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06001FEA RID: 8170 RVA: 0x00112F76 File Offset: 0x00111376
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x00112F8B File Offset: 0x0011138B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x00112FAE File Offset: 0x001113AE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x00112FC0 File Offset: 0x001113C0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x00112FDB File Offset: 0x001113DB
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00112FF0 File Offset: 0x001113F0
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.contents.Any)
			{
				string text = GenThing.ThingsToCommaList(this.contents, true, true, 5).CapitalizeFirst();
				if (this.contents.Count > 5)
				{
					result = "SomeItemStashContents".Translate(new object[]
					{
						text
					});
				}
				else
				{
					result = "ItemStashContents".Translate(new object[]
					{
						text
					});
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400126D RID: 4717
		public ThingOwner contents;
	}
}
