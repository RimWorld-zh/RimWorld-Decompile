using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class IncidentDef : Def
	{
		public Type workerClass;

		public IncidentCategoryDef category = null;

		public List<IncidentTargetTypeDef> targetTypes = null;

		public float baseChance = 0f;

		public IncidentPopulationEffect populationEffect = IncidentPopulationEffect.None;

		public int earliestDay = 0;

		public int minPopulation = 0;

		public float minRefireDays = 0f;

		public int minDifficulty = 0;

		public bool pointsScaleable = false;

		public float minThreatPoints = -1f;

		public List<BiomeDef> allowedBiomes;

		[NoTranslate]
		public List<string> tags;

		[NoTranslate]
		public List<string> refireCheckTags;

		public SimpleCurve chanceFactorByPopulationCurve = null;

		public TaleDef tale = null;

		[MustTranslate]
		public string letterText;

		[MustTranslate]
		public string letterLabel;

		public LetterDef letterDef;

		public GameConditionDef gameCondition;

		public FloatRange durationDays;

		public HediffDef diseaseIncident = null;

		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		public int diseaseMaxVictims = 99999;

		public List<BiomeDiseaseRecord> diseaseBiomeRecords = null;

		public List<BodyPartDef> diseasePartsToAffect = null;

		public ThingDef shipPart = null;

		public List<MTBByBiome> mtbDaysByBiome;

		[Unsaved]
		private IncidentWorker workerInt = null;

		[Unsaved]
		private List<IncidentDef> cachedRefireCheckIncidents = null;

		public IncidentDef()
		{
		}

		public bool NeedsParmsPoints
		{
			get
			{
				return this.category.needsParmsPoints;
			}
		}

		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				List<IncidentDef> result;
				if (this.refireCheckTags == null)
				{
					result = null;
				}
				else
				{
					if (this.cachedRefireCheckIncidents == null)
					{
						this.cachedRefireCheckIncidents = new List<IncidentDef>();
						List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
							{
								this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
							}
						}
					}
					result = this.cachedRefireCheckIncidents;
				}
				return result;
			}
		}

		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			bool result;
			if (other.tags == null)
			{
				result = false;
			}
			else if (other == this)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < other.tags.Count; i++)
				{
					for (int j = 0; j < this.refireCheckTags.Count; j++)
					{
						if (other.tags[i] == this.refireCheckTags[j])
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string c in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.category == null)
			{
				yield return "category is undefined.";
			}
			if (this.targetTypes == null || this.targetTypes.Count == 0)
			{
				yield return "no target type";
			}
			if (this.TargetTypeAllowed(IncidentTargetTypeDefOf.World))
			{
				if (this.targetTypes.Any((IncidentTargetTypeDef tt) => tt != IncidentTargetTypeDefOf.World))
				{
					yield return "allows world target type along with other targets. World targeting incidents should only target the world.";
				}
			}
			if (this.TargetTypeAllowed(IncidentTargetTypeDefOf.World) && this.allowedBiomes != null)
			{
				yield return "world-targeting incident has a biome restriction list";
			}
			yield break;
		}

		public bool TargetTypeAllowed(IncidentTargetTypeDef target)
		{
			return this.targetTypes.Contains(target);
		}

		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTypes.Intersect(target.AcceptedTypes()).Any<IncidentTargetTypeDef>();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <c>__1;

			internal IncidentDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			private static Predicate<IncidentTargetTypeDef> <>f__am$cache0;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_F3;
				case 3u:
					goto IL_137;
				case 4u:
					goto IL_19D;
				case 5u:
					goto IL_1E1;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.category == null)
				{
					this.$current = "category is undefined.";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_F3:
				if (this.targetTypes == null || this.targetTypes.Count == 0)
				{
					this.$current = "no target type";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_137:
				if (base.TargetTypeAllowed(IncidentTargetTypeDefOf.World))
				{
					if (this.targetTypes.Any((IncidentTargetTypeDef tt) => tt != IncidentTargetTypeDefOf.World))
					{
						this.$current = "allows world target type along with other targets. World targeting incidents should only target the world.";
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
				}
				IL_19D:
				if (base.TargetTypeAllowed(IncidentTargetTypeDefOf.World) && this.allowedBiomes != null)
				{
					this.$current = "world-targeting incident has a biome restriction list";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1E1:
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
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
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
				IncidentDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new IncidentDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}

			private static bool <>m__0(IncidentTargetTypeDef tt)
			{
				return tt != IncidentTargetTypeDefOf.World;
			}
		}
	}
}
