using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004EA RID: 1258
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x0600166D RID: 5741 RVA: 0x000C6EF4 File Offset: 0x000C52F4
		public DrugPolicy()
		{
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x000C6EFD File Offset: 0x000C52FD
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x000C6F1C File Offset: 0x000C531C
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

		// Token: 0x06001673 RID: 5747 RVA: 0x000C6FCC File Offset: 0x000C53CC
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

		// Token: 0x06001674 RID: 5748 RVA: 0x000C708A File Offset: 0x000C548A
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000C70C8 File Offset: 0x000C54C8
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
