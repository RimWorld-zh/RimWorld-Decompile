using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000824 RID: 2084
	public class TransferableImmutable : Transferable
	{
		// Token: 0x04001908 RID: 6408
		public List<Thing> things = new List<Thing>();

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x0018F518 File Offset: 0x0018D918
		public override Thing AnyThing
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.things[0];
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x0018F54C File Offset: 0x0018D94C
		public override ThingDef ThingDef
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.AnyThing.def;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x0018F580 File Offset: 0x0018D980
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x0018F5A8 File Offset: 0x0018D9A8
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x0018F5C8 File Offset: 0x0018D9C8
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x0018F5E0 File Offset: 0x0018D9E0
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x0018F5F8 File Offset: 0x0018D9F8
		public override string TipDescription
		{
			get
			{
				return (!this.HasAnyThing) ? "" : this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x0018F630 File Offset: 0x0018DA30
		// (set) Token: 0x06002EC8 RID: 11976 RVA: 0x0018F646 File Offset: 0x0018DA46
		public override int CountToTransfer
		{
			get
			{
				return 0;
			}
			protected set
			{
				if (value != 0)
				{
					throw new InvalidOperationException("immutable transferable");
				}
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002EC9 RID: 11977 RVA: 0x0018F65C File Offset: 0x0018DA5C
		public string LabelWithTotalStackCount
		{
			get
			{
				string text = this.Label;
				int totalStackCount = this.TotalStackCount;
				if (totalStackCount != 1)
				{
					text = text + " x" + totalStackCount.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x0018F69C File Offset: 0x0018DA9C
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst();
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002ECB RID: 11979 RVA: 0x0018F6BC File Offset: 0x0018DABC
		public int TotalStackCount
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

		// Token: 0x06002ECC RID: 11980 RVA: 0x0018F708 File Offset: 0x0018DB08
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x0018F720 File Offset: 0x0018DB20
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x0018F738 File Offset: 0x0018DB38
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x0018F754 File Offset: 0x0018DB54
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, new object[0]);
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
