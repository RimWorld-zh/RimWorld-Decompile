using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E8 RID: 1256
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x04000D19 RID: 3353
		public int uniqueId;

		// Token: 0x04000D1A RID: 3354
		public string label;

		// Token: 0x04000D1B RID: 3355
		private List<DrugPolicyEntry> entriesInt;

		// Token: 0x06001667 RID: 5735 RVA: 0x000C7238 File Offset: 0x000C5638
		public DrugPolicy()
		{
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000C7241 File Offset: 0x000C5641
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001669 RID: 5737 RVA: 0x000C7260 File Offset: 0x000C5660
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

		// Token: 0x0600166D RID: 5741 RVA: 0x000C7310 File Offset: 0x000C5710
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

		// Token: 0x0600166E RID: 5742 RVA: 0x000C73CE File Offset: 0x000C57CE
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, new object[0]);
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x000C740C File Offset: 0x000C580C
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}
	}
}
