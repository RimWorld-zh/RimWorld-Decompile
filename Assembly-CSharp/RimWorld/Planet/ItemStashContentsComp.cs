using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000623 RID: 1571
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x04001271 RID: 4721
		public ThingOwner contents;

		// Token: 0x06001FED RID: 8173 RVA: 0x0011332E File Offset: 0x0011172E
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x00113343 File Offset: 0x00111743
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00113366 File Offset: 0x00111766
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00113378 File Offset: 0x00111778
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00113393 File Offset: 0x00111793
		public override void PostPostRemove()
		{
			base.PostPostRemove();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x001133A8 File Offset: 0x001117A8
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
