using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E8 RID: 1256
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x04000D16 RID: 3350
		public int uniqueId;

		// Token: 0x04000D17 RID: 3351
		public string label;

		// Token: 0x04000D18 RID: 3352
		private List<DrugPolicyEntry> entriesInt;

		// Token: 0x06001668 RID: 5736 RVA: 0x000C7038 File Offset: 0x000C5438
		public DrugPolicy()
		{
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000C7041 File Offset: 0x000C5441
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x000C7060 File Offset: 0x000C5460
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

		// Token: 0x0600166E RID: 5742 RVA: 0x000C7110 File Offset: 0x000C5510
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

		// Token: 0x0600166F RID: 5743 RVA: 0x000C71CE File Offset: 0x000C55CE
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x000C720C File Offset: 0x000C560C
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}
	}
}
