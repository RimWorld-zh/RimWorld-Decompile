using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000822 RID: 2082
	public class TransferableImmutable : Transferable
	{
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x0018F164 File Offset: 0x0018D564
		public override Thing AnyThing
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.things[0];
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x0018F198 File Offset: 0x0018D598
		public override ThingDef ThingDef
		{
			get
			{
				return (!this.HasAnyThing) ? null : this.AnyThing.def;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x0018F1CC File Offset: 0x0018D5CC
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x0018F1F4 File Offset: 0x0018D5F4
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x0018F214 File Offset: 0x0018D614
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x0018F22C File Offset: 0x0018D62C
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x0018F244 File Offset: 0x0018D644
		public override string TipDescription
		{
			get
			{
				return (!this.HasAnyThing) ? "" : this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x0018F27C File Offset: 0x0018D67C
		// (set) Token: 0x06002EC5 RID: 11973 RVA: 0x0018F292 File Offset: 0x0018D692
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
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x0018F2A8 File Offset: 0x0018D6A8
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
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x0018F2E8 File Offset: 0x0018D6E8
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst();
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x0018F308 File Offset: 0x0018D708
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

		// Token: 0x06002EC9 RID: 11977 RVA: 0x0018F354 File Offset: 0x0018D754
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x0018F36C File Offset: 0x0018D76C
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x0018F384 File Offset: 0x0018D784
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x0018F3A0 File Offset: 0x0018D7A0
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

		// Token: 0x04001904 RID: 6404
		public List<Thing> things = new List<Thing>();
	}
}
