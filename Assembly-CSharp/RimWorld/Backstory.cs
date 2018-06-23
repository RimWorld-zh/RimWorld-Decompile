using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004DA RID: 1242
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		// Token: 0x04000CCD RID: 3277
		public string identifier = null;

		// Token: 0x04000CCE RID: 3278
		public BackstorySlot slot;

		// Token: 0x04000CCF RID: 3279
		public string title;

		// Token: 0x04000CD0 RID: 3280
		public string titleFemale;

		// Token: 0x04000CD1 RID: 3281
		public string titleShort;

		// Token: 0x04000CD2 RID: 3282
		public string titleShortFemale;

		// Token: 0x04000CD3 RID: 3283
		public string baseDesc = null;

		// Token: 0x04000CD4 RID: 3284
		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		// Token: 0x04000CD5 RID: 3285
		[Unsaved]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		// Token: 0x04000CD6 RID: 3286
		public WorkTags workDisables = WorkTags.None;

		// Token: 0x04000CD7 RID: 3287
		public WorkTags requiredWorkTags = WorkTags.None;

		// Token: 0x04000CD8 RID: 3288
		public List<string> spawnCategories = new List<string>();

		// Token: 0x04000CD9 RID: 3289
		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal = null;

		// Token: 0x04000CDA RID: 3290
		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale = null;

		// Token: 0x04000CDB RID: 3291
		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale = null;

		// Token: 0x04000CDC RID: 3292
		[Unsaved]
		private BodyTypeDef bodyTypeGlobalResolved = null;

		// Token: 0x04000CDD RID: 3293
		[Unsaved]
		private BodyTypeDef bodyTypeFemaleResolved = null;

		// Token: 0x04000CDE RID: 3294
		[Unsaved]
		private BodyTypeDef bodyTypeMaleResolved = null;

		// Token: 0x04000CDF RID: 3295
		public List<TraitEntry> forcedTraits = null;

		// Token: 0x04000CE0 RID: 3296
		public List<TraitEntry> disallowedTraits = null;

		// Token: 0x04000CE1 RID: 3297
		public bool shuffleable = true;

		// Token: 0x04000CE2 RID: 3298
		[Unsaved]
		public string untranslatedTitle;

		// Token: 0x04000CE3 RID: 3299
		[Unsaved]
		public string untranslatedTitleFemale;

		// Token: 0x04000CE4 RID: 3300
		[Unsaved]
		public string untranslatedTitleShort;

		// Token: 0x04000CE5 RID: 3301
		[Unsaved]
		public string untranslatedTitleShortFemale;

		// Token: 0x04000CE6 RID: 3302
		[Unsaved]
		public string untranslatedDesc;

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001613 RID: 5651 RVA: 0x000C3D60 File Offset: 0x000C2160
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
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x000C3D8C File Offset: 0x000C218C
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

		// Token: 0x06001615 RID: 5653 RVA: 0x000C3DB8 File Offset: 0x000C21B8
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

		// Token: 0x06001616 RID: 5654 RVA: 0x000C3E34 File Offset: 0x000C2234
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

		// Token: 0x06001617 RID: 5655 RVA: 0x000C3E74 File Offset: 0x000C2274
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000C3E98 File Offset: 0x000C2298
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

		// Token: 0x06001619 RID: 5657 RVA: 0x000C3EF4 File Offset: 0x000C22F4
		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000C3F18 File Offset: 0x000C2318
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

		// Token: 0x0600161B RID: 5659 RVA: 0x000C3F64 File Offset: 0x000C2364
		public string FullDescriptionFor(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.baseDesc.AdjustedFor(p, "PAWN"));
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

		// Token: 0x0600161C RID: 5660 RVA: 0x000C4134 File Offset: 0x000C2534
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x000C415C File Offset: 0x000C255C
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x000C4181 File Offset: 0x000C2581
		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x000C41AC File Offset: 0x000C25AC
		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x000C41D8 File Offset: 0x000C25D8
		public void PostLoad()
		{
			this.untranslatedTitle = this.title;
			this.untranslatedTitleFemale = this.titleFemale;
			this.untranslatedTitleShort = this.titleShort;
			this.untranslatedTitleShortFemale = this.titleShortFemale;
			this.untranslatedDesc = this.baseDesc;
			this.baseDesc = this.baseDesc.TrimEnd(new char[0]);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x000C423C File Offset: 0x000C263C
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

		// Token: 0x06001622 RID: 5666 RVA: 0x000C43EC File Offset: 0x000C27EC
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

		// Token: 0x06001623 RID: 5667 RVA: 0x000C441D File Offset: 0x000C281D
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000C442E File Offset: 0x000C282E
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000C4440 File Offset: 0x000C2840
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

		// Token: 0x06001626 RID: 5670 RVA: 0x000C4488 File Offset: 0x000C2888
		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}
	}
}
