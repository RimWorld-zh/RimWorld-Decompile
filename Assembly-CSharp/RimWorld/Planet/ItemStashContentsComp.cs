using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000625 RID: 1573
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06001FF3 RID: 8179 RVA: 0x00112F22 File Offset: 0x00111322
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00112F37 File Offset: 0x00111337
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x00112F5A File Offset: 0x0011135A
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00112F6C File Offset: 0x0011136C
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x00112F87 File Offset: 0x00111387
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x00112F9C File Offset: 0x0011139C
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.contents.Any)
			{
				ItemStashContentsComp.tmpContents.Clear();
				ItemStashContentsComp.tmpContents.AddRange(this.contents);
				ItemStashContentsComp.tmpContents.SortByDescending((Thing x) => x.MarketValue * (float)x.stackCount);
				ItemStashContentsComp.tmpContentsStr.Clear();
				for (int i = 0; i < Mathf.Min(5, ItemStashContentsComp.tmpContents.Count); i++)
				{
					ItemStashContentsComp.tmpContentsStr.Add(ItemStashContentsComp.tmpContents[i].LabelShort.CapitalizeFirst());
				}
				string text = ItemStashContentsComp.tmpContentsStr.ToCommaList(true);
				int count = ItemStashContentsComp.tmpContents.Count;
				ItemStashContentsComp.tmpContents.Clear();
				ItemStashContentsComp.tmpContentsStr.Clear();
				if (count > 5)
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

		// Token: 0x04001270 RID: 4720
		public ThingOwner contents;

		// Token: 0x04001271 RID: 4721
		private static List<Thing> tmpContents = new List<Thing>();

		// Token: 0x04001272 RID: 4722
		private static List<string> tmpContentsStr = new List<string>();
	}
}
