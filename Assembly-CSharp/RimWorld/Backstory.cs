using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[CaseInsensitiveXMLParsing]
	public class Backstory
	{
		public string identifier;

		public BackstorySlot slot;

		public string title;

		public string titleFemale;

		public string titleShort;

		public string titleShortFemale;

		public string baseDesc;

		private Dictionary<string, int> skillGains = new Dictionary<string, int>();

		[Unsaved]
		public Dictionary<SkillDef, int> skillGainsResolved = new Dictionary<SkillDef, int>();

		public WorkTags workDisables;

		public WorkTags requiredWorkTags;

		public List<string> spawnCategories = new List<string>();

		[LoadAlias("bodyNameGlobal")]
		private string bodyTypeGlobal;

		[LoadAlias("bodyNameFemale")]
		private string bodyTypeFemale;

		[LoadAlias("bodyNameMale")]
		private string bodyTypeMale;

		[Unsaved]
		private BodyTypeDef bodyTypeGlobalResolved;

		[Unsaved]
		private BodyTypeDef bodyTypeFemaleResolved;

		[Unsaved]
		private BodyTypeDef bodyTypeMaleResolved;

		public List<TraitEntry> forcedTraits;

		public List<TraitEntry> disallowedTraits;

		public bool shuffleable = true;

		[Unsaved]
		public string untranslatedTitle;

		[Unsaved]
		public string untranslatedTitleFemale;

		[Unsaved]
		public string untranslatedTitleShort;

		[Unsaved]
		public string untranslatedTitleShortFemale;

		[Unsaved]
		public string untranslatedDesc;

		[Unsaved]
		public bool titleTranslated;

		[Unsaved]
		public bool titleFemaleTranslated;

		[Unsaved]
		public bool titleShortTranslated;

		[Unsaved]
		public bool titleShortFemaleTranslated;

		[Unsaved]
		public bool descTranslated;

		public Backstory()
		{
		}

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

		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		public string TitleShortFor(Gender g)
		{
			if (g == Gender.Female && !this.titleShortFemale.NullOrEmpty())
			{
				return this.titleShortFemale;
			}
			if (!this.titleShort.NullOrEmpty())
			{
				return this.titleShort;
			}
			return this.TitleFor(g);
		}

		public string TitleShortCapFor(Gender g)
		{
			return this.TitleShortFor(g).CapitalizeFirst();
		}

		public BodyTypeDef BodyTypeFor(Gender g)
		{
			if (this.bodyTypeGlobalResolved != null || g == Gender.None)
			{
				return this.bodyTypeGlobalResolved;
			}
			if (g == Gender.Female)
			{
				return this.bodyTypeFemaleResolved;
			}
			return this.bodyTypeMaleResolved;
		}

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
			string desc = stringBuilder.ToString().TrimEndNewlines();
			return Find.ActiveLanguageWorker.PostProcessedBackstoryDescription(desc);
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
			this.untranslatedTitle = this.title;
			this.untranslatedTitleFemale = this.titleFemale;
			this.untranslatedTitleShort = this.titleShort;
			this.untranslatedTitleShortFemale = this.titleShortFemale;
			this.untranslatedDesc = this.baseDesc;
			this.baseDesc = this.baseDesc.TrimEnd(new char[0]);
			this.baseDesc = this.baseDesc.Replace("\r", string.Empty);
		}

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
			if (this.slot == BackstorySlot.Adulthood && this.bodyTypeGlobalResolved == null)
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

		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<WorkTypeDef>, IEnumerator, IDisposable, IEnumerator<WorkTypeDef>
		{
			internal List<WorkTypeDef> <list>__0;

			internal int <i>__1;

			internal Backstory $this;

			internal WorkTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					i = 0;
					break;
				case 1u:
					IL_84:
					i++;
					break;
				default:
					return false;
				}
				if (i >= list.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!base.AllowsWorkType(list[i]))
					{
						this.$current = list[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_84;
				}
				return false;
			}

			WorkTypeDef IEnumerator<WorkTypeDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.WorkTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorkTypeDef> IEnumerable<WorkTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Backstory.<>c__Iterator0 <>c__Iterator = new Backstory.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<WorkGiverDef>, IEnumerator, IDisposable, IEnumerator<WorkGiverDef>
		{
			internal List<WorkGiverDef> <list>__0;

			internal int <i>__1;

			internal Backstory $this;

			internal WorkGiverDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					list = DefDatabase<WorkGiverDef>.AllDefsListForReading;
					i = 0;
					break;
				case 1u:
					IL_84:
					i++;
					break;
				default:
					return false;
				}
				if (i >= list.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (!base.AllowsWorkGiver(list[i]))
					{
						this.$current = list[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_84;
				}
				return false;
			}

			WorkGiverDef IEnumerator<WorkGiverDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.WorkGiverDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorkGiverDef> IEnumerable<WorkGiverDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Backstory.<>c__Iterator1 <>c__Iterator = new Backstory.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator2 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal bool ignoreNoSpawnCategories;

			internal Dictionary<SkillDef, int>.Enumerator $locvar0;

			internal KeyValuePair<SkillDef, int> <kvp>__1;

			internal Dictionary<string, Backstory>.Enumerator $locvar1;

			internal KeyValuePair<string, Backstory> <kvp>__2;

			internal Backstory $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (this.title.NullOrEmpty())
					{
						this.$current = "null title, baseDesc is " + this.baseDesc;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_CB;
				case 3u:
					goto IL_116;
				case 4u:
					goto IL_155;
				case 5u:
					goto IL_1AA;
				case 6u:
					goto IL_1F9;
				case 7u:
					goto IL_244;
				case 8u:
					goto IL_267;
				case 9u:
					goto IL_328;
				default:
					return false;
				}
				if (this.titleShort.NullOrEmpty())
				{
					this.$current = "null titleShort, baseDesc is " + this.baseDesc;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_CB:
				if ((this.workDisables & WorkTags.Violent) != WorkTags.None && this.spawnCategories.Contains("Raider"))
				{
					this.$current = "cannot do Violent work but can spawn as a raider";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_116:
				if (this.spawnCategories.Count == 0 && !ignoreNoSpawnCategories)
				{
					this.$current = "no spawn categories";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_155:
				if (this.spawnCategories.Count == 1 && this.spawnCategories[0] == "Trader")
				{
					this.$current = "only Trader spawn category";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1AA:
				if (this.baseDesc.NullOrEmpty())
				{
					goto IL_244;
				}
				if (char.IsWhiteSpace(this.baseDesc[0]))
				{
					this.$current = "baseDesc starts with whitepspace";
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_1F9:
				if (char.IsWhiteSpace(this.baseDesc[this.baseDesc.Length - 1]))
				{
					this.$current = "baseDesc ends with whitespace";
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_244:
				if (!Prefs.DevMode)
				{
					goto IL_3E3;
				}
				enumerator = this.skillGainsResolved.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_267:
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						kvp = enumerator.Current;
						if (kvp.Key.IsDisabled(this.workDisables, base.DisabledWorkTypes))
						{
							this.$current = "modifies skill " + kvp.Key + " but also disables this skill";
							if (!this.$disposing)
							{
								this.$PC = 8;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				enumerator2 = BackstoryDatabase.allBackstories.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_328:
					switch (num)
					{
					}
					while (enumerator2.MoveNext())
					{
						kvp2 = enumerator2.Current;
						if (kvp2.Value != this && kvp2.Value.identifier == this.identifier)
						{
							this.$current = "backstory identifier used more than once: " + this.identifier;
							if (!this.$disposing)
							{
								this.$PC = 9;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				IL_3E3:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 8u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 9u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Backstory.<ConfigErrors>c__Iterator2 <ConfigErrors>c__Iterator = new Backstory.<ConfigErrors>c__Iterator2();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.ignoreNoSpawnCategories = ignoreNoSpawnCategories;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
