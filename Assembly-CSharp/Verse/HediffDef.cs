using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class HediffDef : Def
	{
		public Type hediffClass = typeof(Hediff);

		public List<HediffCompProperties> comps;

		public float initialSeverity = 0.5f;

		public float lethalSeverity = -1f;

		public List<HediffStage> stages;

		public bool tendable;

		public bool isBad = true;

		public ThingDef spawnThingOnRemoved;

		public float chanceToCauseNoPain;

		public bool makesSickThought;

		public bool makesAlert = true;

		public NeedDef causesNeed;

		public float minSeverity;

		public float maxSeverity = float.MaxValue;

		public bool scenarioCanAdd;

		public List<HediffGiver> hediffGivers;

		public bool cureAllAtOnceIfCuredByItem;

		public TaleDef taleOnVisible;

		public bool everCurableByItem = true;

		public string battleStateLabel;

		public string labelNounPretty;

		public bool displayWound;

		public Color defaultLabelColor = Color.white;

		public InjuryProps injuryProps;

		public AddedBodyPartProps addedPartProps;

		[MustTranslate]
		public string labelNoun;

		private bool alwaysAllowMothballCached;

		private bool alwaysAllowMothball;

		private Hediff concreteExampleInt;

		public HediffDef()
		{
		}

		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		public Hediff ConcreteExample
		{
			get
			{
				if (this.concreteExampleInt == null)
				{
					this.concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this);
				}
				return this.concreteExampleInt;
			}
		}

		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		public T CompProps<T>() where T : HediffCompProperties
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return (T)((object)null);
		}

		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return string.Format(this.labelNounPretty, this.label, bodyPart.Label);
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in base.ConfigErrors())
			{
				yield return err;
			}
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					foreach (string compErr in this.comps[i].ConfigErrors(this))
					{
						yield return this.comps[i] + ": " + compErr;
					}
				}
			}
			if (this.stages != null)
			{
				if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					for (int j = 0; j < this.stages.Count; j++)
					{
						if (j >= 1 && this.stages[j].minSeverity <= this.stages[j - 1].minSeverity)
						{
							yield return "stages are not in order of minSeverity";
						}
					}
				}
				for (int k = 0; k < this.stages.Count; k++)
				{
					if (this.stages[k].makeImmuneTo != null)
					{
						if (!this.stages[k].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
						{
							yield return "makes immune to hediff which doesn't have comp immunizable";
						}
					}
				}
			}
			yield break;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry de in this.stages[0].SpecialDisplayStats())
				{
					yield return de;
				}
			}
			yield break;
		}

		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
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

			internal string <err>__1;

			internal int <i>__2;

			internal IEnumerator<string> $locvar1;

			internal string <compErr>__3;

			internal int <i>__4;

			internal int <i>__5;

			internal HediffDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			private static Predicate<HediffDef> <>f__am$cache0;

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
					goto IL_FF;
				case 3u:
					goto IL_152;
				case 4u:
					goto IL_18C;
				case 5u:
					goto IL_1C6;
				case 6u:
					goto IL_20F;
				case 7u:
					Block_16:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							compErr = enumerator2.Current;
							this.$current = this.comps[i] + ": " + compErr;
							if (!this.$disposing)
							{
								this.$PC = 7;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					i++;
					goto IL_2FC;
				case 8u:
					IL_3BA:
					j++;
					goto IL_3C8;
				case 9u:
					IL_471:
					k++;
					goto IL_47F;
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
						err = enumerator.Current;
						this.$current = err;
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
				if (this.hediffClass == null)
				{
					this.$current = "hediffClass is null";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_FF:
				if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
				{
					this.$current = "has comps but hediffClass is not HediffWithComps or subclass thereof";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_152:
				if (this.minSeverity > this.initialSeverity)
				{
					this.$current = "minSeverity is greater than initialSeverity";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_18C:
				if (this.maxSeverity < this.initialSeverity)
				{
					this.$current = "maxSeverity is lower than initialSeverity";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1C6:
				if (!this.tendable && base.HasComp(typeof(HediffComp_TendDuration)))
				{
					this.$current = "has HediffComp_TendDuration but tendable = false";
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_20F:
				if (this.comps == null)
				{
					goto IL_317;
				}
				i = 0;
				IL_2FC:
				if (i < this.comps.Count)
				{
					enumerator2 = this.comps[i].ConfigErrors(this).GetEnumerator();
					num = 4294967293u;
					goto Block_16;
				}
				IL_317:
				if (this.stages == null)
				{
					goto IL_49A;
				}
				if (typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					goto IL_3E3;
				}
				j = 0;
				IL_3C8:
				if (j < this.stages.Count)
				{
					if (j >= 1 && this.stages[j].minSeverity <= this.stages[j - 1].minSeverity)
					{
						this.$current = "stages are not in order of minSeverity";
						if (!this.$disposing)
						{
							this.$PC = 8;
						}
						return true;
					}
					goto IL_3BA;
				}
				IL_3E3:
				k = 0;
				IL_47F:
				if (k < this.stages.Count)
				{
					if (this.stages[k].makeImmuneTo == null)
					{
						goto IL_471;
					}
					if (!this.stages[k].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
					{
						this.$current = "makes immune to hediff which doesn't have comp immunizable";
						if (!this.$disposing)
						{
							this.$PC = 9;
						}
						return true;
					}
					goto IL_471;
				}
				IL_49A:
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
				case 7u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				HediffDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new HediffDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}

			private static bool <>m__0(HediffDef im)
			{
				return im.HasComp(typeof(HediffComp_Immunizable));
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator1 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <de>__1;

			internal HediffDef $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator1()
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
					if (this.stages == null || this.stages.Count != 1)
					{
						goto IL_E1;
					}
					enumerator = this.stages[0].SpecialDisplayStats().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
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
						de = enumerator.Current;
						this.$current = de;
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
				IL_E1:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffDef.<SpecialDisplayStats>c__Iterator1 <SpecialDisplayStats>c__Iterator = new HediffDef.<SpecialDisplayStats>c__Iterator1();
				<SpecialDisplayStats>c__Iterator.$this = this;
				return <SpecialDisplayStats>c__Iterator;
			}
		}
	}
}
