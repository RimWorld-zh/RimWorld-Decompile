using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004EA RID: 1258
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x000C6EA0 File Offset: 0x000C52A0
		public DrugPolicy()
		{
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000C6EA9 File Offset: 0x000C52A9
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x000C6EC8 File Offset: 0x000C52C8
		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

		// Token: 0x170002ED RID: 749
		public DrugPolicyEntry this[int index]
		{
			get
			{
				return this.entriesInt[index];
			}
			set
			{
				this.entriesInt[index] = value;
			}
		}

		// Token: 0x170002EE RID: 750
		public DrugPolicyEntry this[ThingDef drugDef]
		{
			get
			{
				for (int i = 0; i < this.entriesInt.Count; i++)
				{
					if (this.entriesInt[i].drug == drugDef)
					{
						return this.entriesInt[i];
					}
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x000C6F78 File Offset: 0x000C5378
		public void InitializeIfNeeded()
		{
			if (this.entriesInt == null)
			{
				this.entriesInt = new List<DrugPolicyEntry>();
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].category == ThingCategory.Item && allDefsListForReading[i].IsDrug)
					{
						DrugPolicyEntry drugPolicyEntry = new DrugPolicyEntry();
						drugPolicyEntry.drug = allDefsListForReading[i];
						drugPolicyEntry.allowedForAddiction = true;
						this.entriesInt.Add(drugPolicyEntry);
					}
				}
				this.entriesInt.SortBy((DrugPolicyEntry e) => e.drug.GetCompProperties<CompProperties_Drug>().listOrder);
			}
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000C7036 File Offset: 0x000C5436
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000C7074 File Offset: 0x000C5474
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04000D19 RID: 3353
		public int uniqueId;

		// Token: 0x04000D1A RID: 3354
		public string label;

		// Token: 0x04000D1B RID: 3355
		private List<DrugPolicyEntry> entriesInt;
	}
}
