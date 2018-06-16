using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000826 RID: 2086
	public class TransferableImmutable : Transferable
	{
		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x0018EEF0 File Offset: 0x0018D2F0
		public override Thing AnyThing
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.things[0];
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x0018EF24 File Offset: 0x0018D324
		public override ThingDef ThingDef
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.AnyThing.def;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x0018EF58 File Offset: 0x0018D358
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x0018EF80 File Offset: 0x0018D380
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x0018EFA0 File Offset: 0x0018D3A0
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x0018EFB8 File Offset: 0x0018D3B8
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x0018EFD0 File Offset: 0x0018D3D0
		public override string TipDescription
		{
			get
			{
				return (!this.HasAnyThing) ? "" : this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002EC9 RID: 11977 RVA: 0x0018F008 File Offset: 0x0018D408
		// (set) Token: 0x06002ECA RID: 11978 RVA: 0x0018F01E File Offset: 0x0018D41E
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

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002ECB RID: 11979 RVA: 0x0018F034 File Offset: 0x0018D434
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

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002ECC RID: 11980 RVA: 0x0018F074 File Offset: 0x0018D474
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst();
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002ECD RID: 11981 RVA: 0x0018F094 File Offset: 0x0018D494
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

		// Token: 0x06002ECE RID: 11982 RVA: 0x0018F0E0 File Offset: 0x0018D4E0
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x0018F0F8 File Offset: 0x0018D4F8
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x0018F110 File Offset: 0x0018D510
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x0018F12C File Offset: 0x0018D52C
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

		// Token: 0x04001906 RID: 6406
		public List<Thing> things = new List<Thing>();
	}
}
