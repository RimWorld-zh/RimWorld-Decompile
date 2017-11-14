using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		public string identifier;

		public BackstorySlot slot;

		private string title;

		private string titleShort;

		public string baseDesc;

		public Dictionary<string, int> skillGains = new Dictionary<string, int>();

		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		public WorkTags workDisables;

		public WorkTags requiredWorkTags;

		public List<string> spawnCategories = new List<string>();

		[LoadAlias("bodyNameGlobal")]
		public BodyType bodyTypeGlobal;

		[LoadAlias("bodyNameFemale")]
		public BodyType bodyTypeFemale;

		[LoadAlias("bodyNameMale")]
		public BodyType bodyTypeMale;

		public List<TraitEntry> forcedTraits;

		public List<TraitEntry> disallowedTraits;

		public bool shuffleable = true;

		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int i = 0;
				while (true)
				{
					if (i < list.Count)
					{
						if (this.AllowsWorkType(list[i]))
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return list[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public IEnumerable<WorkGiverDef> DisabledWorkGivers
		{
			get
			{
				List<WorkGiverDef> list = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				int i = 0;
				while (true)
				{
					if (i < list.Count)
					{
						if (this.AllowsWorkGiver(list[i]))
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return list[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public string Title
		{
			get
			{
				return this.title;
			}
		}

		public string TitleShort
		{
			get
			{
				if (!this.titleShort.NullOrEmpty())
				{
					return this.titleShort;
				}
				return this.title;
			}
		}

		public bool DisallowsTrait(TraitDef def, int degree)
		{
			if (this.disallowedTraits == null)
			{
				return false;
			}
			for (int i = 0; i < this.disallowedTraits.Count; i++)
			{
				if (this.disallowedTraits[i].def == def && this.disallowedTraits[i].degree == degree)
				{
					return true;
				}
			}
			return false;
		}

		public BodyType BodyTypeFor(Gender g)
		{
			if (this.bodyTypeGlobal == BodyType.Undefined)
			{
				switch (g)
				{
				case Gender.None:
					break;
				case Gender.Female:
					return this.bodyTypeFemale;
				default:
					return this.bodyTypeMale;
				}
			}
			return this.bodyTypeGlobal;
		}

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
			foreach (WorkTypeDef disabledWorkType in this.DisabledWorkTypes)
			{
				stringBuilder.AppendLine(disabledWorkType.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
			}
			foreach (WorkGiverDef disabledWorkGiver in this.DisabledWorkGivers)
			{
				stringBuilder.AppendLine(disabledWorkGiver.workType.gerundLabel.CapitalizeFirst() + " -> " + disabledWorkGiver.label + " " + "DisabledLower".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		internal void AddForcedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.forcedTraits == null)
			{
				this.forcedTraits = new List<TraitEntry>();
			}
			this.forcedTraits.Add(new TraitEntry(traitDef, degree));
		}

		internal void AddDisallowedTrait(TraitDef traitDef, int degree = 0)
		{
			if (this.disallowedTraits == null)
			{
				this.disallowedTraits = new List<TraitEntry>();
			}
			this.disallowedTraits.Add(new TraitEntry(traitDef, degree));
		}

		public void PostLoad()
		{
			if (!this.title.Equals(GenText.ToNewsCase(this.title)))
			{
				Log.Warning("Bad capitalization on backstory title: " + this.title);
				this.title = GenText.ToNewsCase(this.title);
			}
			if (this.slot == BackstorySlot.Adulthood && this.bodyTypeGlobal == BodyType.Undefined)
			{
				if (this.bodyTypeMale == BodyType.Undefined)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing male body type. Defaulting...");
					this.bodyTypeMale = BodyType.Male;
				}
				if (this.bodyTypeFemale == BodyType.Undefined)
				{
					Log.Error("Adulthood backstory " + this.title + " is missing female body type. Defaulting...");
					this.bodyTypeFemale = BodyType.Female;
				}
			}
			this.baseDesc = this.baseDesc.TrimEnd();
		}

		public void ResolveReferences()
		{
			int num = Mathf.Abs(GenText.StableStringHash(this.baseDesc) % 100);
			string s = this.title.Replace('-', ' ');
			s = GenText.CapitalizedNoSpaces(s);
			this.identifier = GenText.RemoveNonAlphanumeric(s) + num.ToString();
			foreach (KeyValuePair<string, int> skillGain in this.skillGains)
			{
				this.skillGainsResolved.Add(DefDatabase<SkillDef>.GetNamed(skillGain.Key, true), skillGain.Value);
			}
			this.skillGains = null;
		}

		public IEnumerable<string> ConfigErrors(bool ignoreNoSpawnCategories)
		{
			if (this.title.NullOrEmpty())
			{
				yield return "null title, baseDesc is " + this.baseDesc;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.titleShort.NullOrEmpty())
			{
				yield return "null titleShort, baseDesc is " + this.baseDesc;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if ((this.workDisables & WorkTags.Violent) != 0 && this.spawnCategories.Contains("Raider"))
			{
				yield return "cannot do Violent work but can spawn as a raider";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.spawnCategories.Count == 0 && !ignoreNoSpawnCategories)
			{
				yield return "no spawn categories";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.spawnCategories.Count == 1 && this.spawnCategories[0] == "Trader")
			{
				yield return "only Trader spawn category";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.baseDesc.NullOrEmpty())
			{
				if (char.IsWhiteSpace(this.baseDesc[0]))
				{
					yield return "baseDesc starts with whitepspace";
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (char.IsWhiteSpace(this.baseDesc[this.baseDesc.Length - 1]))
				{
					yield return "baseDesc ends with whitespace";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (Prefs.DevMode)
			{
				foreach (KeyValuePair<SkillDef, int> item in this.skillGainsResolved)
				{
					if (item.Key.IsDisabled(this.workDisables, this.DisabledWorkTypes))
					{
						yield return "modifies skill " + item.Key + " but also disables this skill";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				foreach (KeyValuePair<string, Backstory> allBackstory in BackstoryDatabase.allBackstories)
				{
					if (allBackstory.Value != this && allBackstory.Value.identifier == this.identifier)
					{
						yield return "backstory identifier used more than once: " + this.identifier;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_03ec:
			/*Error near IL_03ed: Unexpected return in MoveNext()*/;
		}

		public void SetTitle(string newTitle)
		{
			this.title = newTitle;
		}

		public void SetTitleShort(string newTitleShort)
		{
			this.titleShort = newTitleShort;
		}

		public override string ToString()
		{
			if (this.title.NullOrEmpty())
			{
				return "(NullTitleBackstory)";
			}
			return "(" + this.title + ")";
		}

		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}
	}
}
