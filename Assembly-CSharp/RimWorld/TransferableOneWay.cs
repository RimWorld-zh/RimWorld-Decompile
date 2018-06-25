using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000825 RID: 2085
	public class TransferableOneWay : Transferable
	{
		// Token: 0x0400190B RID: 6411
		public List<Thing> things = new List<Thing>();

		// Token: 0x0400190C RID: 6412
		private int countToTransfer;

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x0018F840 File Offset: 0x0018DC40
		public override Thing AnyThing
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.things[0];
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x0018F874 File Offset: 0x0018DC74
		public override ThingDef ThingDef
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.AnyThing.def;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x0018F8A8 File Offset: 0x0018DCA8
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x0018F8D0 File Offset: 0x0018DCD0
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002ED7 RID: 11991 RVA: 0x0018F8F0 File Offset: 0x0018DCF0
		public override bool Interactive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x0018F908 File Offset: 0x0018DD08
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x0018F920 File Offset: 0x0018DD20
		public override string TipDescription
		{
			get
			{
				return (!this.HasAnyThing) ? "" : this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002EDA RID: 11994 RVA: 0x0018F958 File Offset: 0x0018DD58
		// (set) Token: 0x06002EDB RID: 11995 RVA: 0x0018F973 File Offset: 0x0018DD73
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
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x0018F98C File Offset: 0x0018DD8C
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

		// Token: 0x06002EDD RID: 11997 RVA: 0x0018F9D8 File Offset: 0x0018DDD8
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x0018F9F0 File Offset: 0x0018DDF0
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x0018FA0C File Offset: 0x0018DE0C
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x0018FA30 File Offset: 0x0018DE30
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
