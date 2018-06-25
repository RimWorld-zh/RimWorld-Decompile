using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000623 RID: 1571
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x0400126D RID: 4717
		public ThingOwner contents;

		// Token: 0x06001FEE RID: 8174 RVA: 0x001130C6 File Offset: 0x001114C6
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x001130DB File Offset: 0x001114DB
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x001130FE File Offset: 0x001114FE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00113110 File Offset: 0x00111510
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0011312B File Offset: 0x0011152B
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00113140 File Offset: 0x00111540
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
	}
}
