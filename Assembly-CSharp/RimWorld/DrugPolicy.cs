using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E6 RID: 1254
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x04000D16 RID: 3350
		public int uniqueId;

		// Token: 0x04000D17 RID: 3351
		public string label;

		// Token: 0x04000D18 RID: 3352
		private List<DrugPolicyEntry> entriesInt;

		// Token: 0x06001664 RID: 5732 RVA: 0x000C6EE8 File Offset: 0x000C52E8
		public DrugPolicy()
		{
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x000C6EF1 File Offset: 0x000C52F1
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001666 RID: 5734 RVA: 0x000C6F10 File Offset: 0x000C5310
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

		// Token: 0x0600166A RID: 5738 RVA: 0x000C6FC0 File Offset: 0x000C53C0
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

		// Token: 0x0600166B RID: 5739 RVA: 0x000C707E File Offset: 0x000C547E
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, new object[0]);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x000C70BC File Offset: 0x000C54BC
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}
	}
}
