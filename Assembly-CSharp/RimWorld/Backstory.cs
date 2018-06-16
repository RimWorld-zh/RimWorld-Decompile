using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DE RID: 1246
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x000C3D54 File Offset: 0x000C2154
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int i = 0; i < list.Count; i++)
				{
					if (!this.AllowsWorkType(list[i]))
					{
						yield return list[i];
					}
				}
				yield break;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x000C3D80 File Offset: 0x000C2180
		public IEnumerable<WorkGiverDef> DisabledWorkGivers
		{
			get
			{
				List<WorkGiverDef> list = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				for (int i = 0; i < list.Count; i++)
				{
					if (!this.AllowsWorkGiver(list[i]))
					{
						yield return list[i];
					}
				}
				yield break;
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x000C3DAC File Offset: 0x000C21AC
		public bool DisallowsTrait(TraitDef def, int degree)
		{
			bool result;
			if (this.disallowedTraits == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.disallowedTraits.Count; i++)
				{
					if (this.disallowedTraits[i].def == def && this.disallowedTraits[i].degree == degree)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x000C3E28 File Offset: 0x000C2228
		public string TitleFor(Gender g)
		{
			string result;
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				result = this.title;
			}
			else
			{
				result = this.titleFemale;
			}
			return result;
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x000C3E68 File Offset: 0x000C2268
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x000C3E8C File Offset: 0x000C228C
		public string TitleShortFor(Gender g)
		{
			string result;
			if (g == Gender.Female && !this.titleShortFemale.NullOrEmpty())
			{
				result = this.titleShortFemale;
			}
			else if (!this.titleShort.NullOrEmpty())
			{
				result = this.titleShort;
			}
			else
			{
				result = this.TitleFor(g);
			}
			return result;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x000C3EE8 File Offset: 0x000C22E8
		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x000C3F0C File Offset: 0x000C230C
		public BodyTypeDef BodyTypeFor(Gender g)
		{
			BodyTypeDef result;
			if (this.bodyTypeGlobalResolved != null || g == Gender.None)
			{
				result = this.bodyTypeGlobalResolved;
			}
			else if (g == Gender.Female)
			{
				result = this.bodyTypeFemaleResolved;
			}
			else
			{
				result = this.bodyTypeMaleResolved;
			}
			return result;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000C3F58 File Offset: 0x000C2358
		public string FullDescriptionFor(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.baseDesc.AdjustedFor(p));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				if (this.skillGainsResolved.ContainsKey(skillDef))
				{
					stringBuilder.AppendLine(skillDef.skillLabel.CapitalizeFirst() + ":   " + this.skillGainsResolved[skillDef].ToString("+##;-##"));
				}
			}
			stringBuilder.AppendLine();
			foreach (WorkTypeDef workTypeDef in this.DisabledWorkTypes)
			{
				stringBuilder.AppendLine(workTypeDef.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
			}
			foreach (WorkGiverDef workGiverDef in this.DisabledWorkGivers)
			{
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					workGiverDef.workType.gerundLabel.CapitalizeFirst(),
					": ",
					workGiverDef.LabelCap,
					" ",
					"DisabledLower".Translate()
				}));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000C4124 File Offset: 0x000C2524
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x000C414C File Offset: 0x000C254C
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x000C4171 File Offset: 0x000C2571
		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000C419C File Offset: 0x000C259C
		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000C41C8 File Offset: 0x000C25C8
		public void PostLoad()
		{
			this.rawEnglishTitle = this.title;
			this.rawEnglishTitleFemale = this.titleFemale;
			this.rawEnglishTitleShort = this.titleShort;
			this.rawEnglishTitleShortFemale = this.titleShortFemale;
			this.rawEnglishDesc = this.baseDesc;
			this.baseDesc = this.baseDesc.TrimEnd(new char[0]);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000C422C File Offset: 0x000C262C
		public void ResolveReferences()
		{
			int num = Mathf.Abs(GenText.StableStringHash(this.baseDesc) % 100);
			string s = this.title.Replace('-', ' ');
			s = GenText.CapitalizedNoSpaces(s);
			this.identifier = GenText.RemoveNonAlphanumeric(s) + num.ToString();
			foreach (KeyValuePair<string, int> keyValuePair in this.skillGains)
			{
				this.skillGainsResolved.Add(DefDatabase<SkillDef>.GetNamed(keyValuePair.Key, true), keyValuePair.Value);
			}
			this.skillGains = null;
			if (!this.bodyTypeGlobal.NullOrEmpty())
			{
				this.bodyTypeGlobalResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeGlobal, true);
			}
			if (!this.bodyTypeFemale.NullOrEmpty())
			{
				this.bodyTypeFemaleResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeFemale, true);
			}
			if (!this.bodyTypeMale.NullOrEmpty())
			{
				this.bodyTypeMaleResolved = DefDatabase<BodyTypeDef>.GetNamed(this.bodyTypeMale, true);
			}
			if (this.slot == BackstorySlot.Adulthood)
			{
				if (this.bodyTypeGlobalResolved == null)
				{
					if (this.bodyTypeMaleResolved == null)
					{
						Log.Error("Adulthood backstory " + this.title + " is missing male body type. Defaulting...", false);
						this.bodyTypeMaleResolved = BodyTypeDefOf.Male;
					}
					if (this.bodyTypeFemaleResolved == null)
					{
						Log.Error("Adulthood backstory " + this.title + " is missing female body type. Defaulting...", false);
						this.bodyTypeFemaleResolved = BodyTypeDefOf.Female;
					}
				}
			}
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x000C43DC File Offset: 0x000C27DC
		public IEnumerable<string> ConfigErrors(bool ignoreNoSpawnCategories)
		{
			if (this.title.NullOrEmpty())
			{
				yield return "null title, baseDesc is " + this.baseDesc;
			}
			if (this.titleShort.NullOrEmpty())
			{
				yield return "null titleShort, baseDesc is " + this.baseDesc;
			}
			if ((this.workDisables & WorkTags.Violent) != WorkTags.None && this.spawnCategories.Contains("Raider"))
			{
				yield return "cannot do Violent work but can spawn as a raider";
			}
			if (this.spawnCategories.Count == 0 && !ignoreNoSpawnCategories)
			{
				yield return "no spawn categories";
			}
			if (this.spawnCategories.Count == 1 && this.spawnCategories[0] == "Trader")
			{
				yield return "only Trader spawn category";
			}
			if (!this.baseDesc.NullOrEmpty())
			{
				if (char.IsWhiteSpace(this.baseDesc[0]))
				{
					yield return "baseDesc starts with whitepspace";
				}
				if (char.IsWhiteSpace(this.baseDesc[this.baseDesc.Length - 1]))
				{
					yield return "baseDesc ends with whitespace";
				}
			}
			if (Prefs.DevMode)
			{
				foreach (KeyValuePair<SkillDef, int> kvp in this.skillGainsResolved)
				{
					if (kvp.Key.IsDisabled(this.workDisables, this.DisabledWorkTypes))
					{
						yield return "modifies skill " + kvp.Key + " but also disables this skill";
					}
				}
				foreach (KeyValuePair<string, Backstory> kvp2 in BackstoryDatabase.allBackstories)
				{
					if (kvp2.Value != this && kvp2.Value.identifier == this.identifier)
					{
						yield return "backstory identifier used more than once: " + this.identifier;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x000C440D File Offset: 0x000C280D
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x000C441E File Offset: 0x000C281E
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x000C4430 File Offset: 0x000C2830
		public override string ToString()
		{
			string result;
			if (this.title.NullOrEmpty())
			{
				result = "(NullTitleBackstory)";
			}
			else
			{
				result = "(" + this.title + ")";
			}
			return result;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000C4478 File Offset: 0x000C2878
		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		// Token: 0x04000CD0 RID: 3280
		public string identifier = null;

		// Token: 0x04000CD1 RID: 3281
		public BackstorySlot slot;

		// Token: 0x04000CD2 RID: 3282
		public string title;

		// Token: 0x04000CD3 RID: 3283
		public string titleFemale;

		// Token: 0x04000CD4 RID: 3284
		public string titleShort;

		// Token: 0x04000CD5 RID: 3285
		public string titleShortFemale;

		// Token: 0x04000CD6 RID: 3286
		public string baseDesc = null;

		// Token: 0x04000CD7 RID: 3287
		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		// Token: 0x04000CD8 RID: 3288
		[Unsaved]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		// Token: 0x04000CD9 RID: 3289
		public WorkTags workDisables = WorkTags.None;

		// Token: 0x04000CDA RID: 3290
		public WorkTags requiredWorkTags = WorkTags.None;

		// Token: 0x04000CDB RID: 3291
		public List<string> spawnCategories = new List<string>();

		// Token: 0x04000CDC RID: 3292
		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal = null;

		// Token: 0x04000CDD RID: 3293
		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale = null;

		// Token: 0x04000CDE RID: 3294
		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale = null;

		// Token: 0x04000CDF RID: 3295
		[Unsaved]
		private BodyTypeDef bodyTypeGlobalResolved = null;

		// Token: 0x04000CE0 RID: 3296
		[Unsaved]
		private BodyTypeDef bodyTypeFemaleResolved = null;

		// Token: 0x04000CE1 RID: 3297
		[Unsaved]
		private BodyTypeDef bodyTypeMaleResolved = null;

		// Token: 0x04000CE2 RID: 3298
		public List<TraitEntry> forcedTraits = null;

		// Token: 0x04000CE3 RID: 3299
		public List<TraitEntry> disallowedTraits = null;

		// Token: 0x04000CE4 RID: 3300
		public bool shuffleable = true;

		// Token: 0x04000CE5 RID: 3301
		[Unsaved]
		public string rawEnglishTitle;

		// Token: 0x04000CE6 RID: 3302
		[Unsaved]
		public string rawEnglishTitleFemale;

		// Token: 0x04000CE7 RID: 3303
		[Unsaved]
		public string rawEnglishTitleShort;

		// Token: 0x04000CE8 RID: 3304
		[Unsaved]
		public string rawEnglishTitleShortFemale;

		// Token: 0x04000CE9 RID: 3305
		[Unsaved]
		public string rawEnglishDesc;
	}
}
