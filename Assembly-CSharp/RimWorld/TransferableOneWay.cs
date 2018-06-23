using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000823 RID: 2083
	public class TransferableOneWay : Transferable
	{
		// Token: 0x04001907 RID: 6407
		public List<Thing> things = new List<Thing>();

		// Token: 0x04001908 RID: 6408
		private int countToTransfer;

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x0018F48C File Offset: 0x0018D88C
		public override Thing AnyThing
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.things[0];
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002ED1 RID: 11985 RVA: 0x0018F4C0 File Offset: 0x0018D8C0
		public override ThingDef ThingDef
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.AnyThing.def;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x0018F4F4 File Offset: 0x0018D8F4
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x0018F51C File Offset: 0x0018D91C
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x0018F53C File Offset: 0x0018D93C
		public override bool Interactive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x0018F554 File Offset: 0x0018D954
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x0018F56C File Offset: 0x0018D96C
		public override string TipDescription
		{
			get
			{
				return (!this.HasAnyThing) ? "" : this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002ED7 RID: 11991 RVA: 0x0018F5A4 File Offset: 0x0018D9A4
		// (set) Token: 0x06002ED8 RID: 11992 RVA: 0x0018F5BF File Offset: 0x0018D9BF
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x0018F5D8 File Offset: 0x0018D9D8
		public int MaxCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x0018F624 File Offset: 0x0018DA24
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x0018F63C File Offset: 0x0018DA3C
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x0018F658 File Offset: 0x0018DA58
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x0018F67C File Offset: 0x0018DA7C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.", false);
				}
			}
		}
	}
}
