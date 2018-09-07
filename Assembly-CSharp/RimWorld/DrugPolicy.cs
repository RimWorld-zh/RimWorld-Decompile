using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		public int uniqueId;

		public string label;

		private List<DrugPolicyEntry> entriesInt;

		[CompilerGenerated]
		private static Func<DrugPolicyEntry, float> <>f__am$cache0;

		public DrugPolicy()
		{
		}

		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

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

		public void InitializeIfNeeded()
		{
			if (this.entriesInt != null)
			{
				return;
			}
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

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, new object[0]);
		}

		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}

		[CompilerGenerated]
		private static float <InitializeIfNeeded>m__0(DrugPolicyEntry e)
		{
			return e.drug.GetCompProperties<CompProperties_Drug>().listOrder;
		}
	}
}
