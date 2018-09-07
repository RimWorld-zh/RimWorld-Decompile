using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ThoughtDef : Def
	{
		public Type thoughtClass;

		public Type workerClass;

		public List<ThoughtStage> stages = new List<ThoughtStage>();

		public int stackLimit = 1;

		public float stackedEffectMultiplier = 0.75f;

		public float durationDays;

		public bool invert;

		public bool validWhileDespawned;

		public ThoughtDef nextThought;

		public List<TraitDef> nullifyingTraits;

		public List<TaleDef> nullifyingOwnTales;

		public List<TraitDef> requiredTraits;

		public int requiredTraitsDegree = int.MinValue;

		public StatDef effectMultiplyingStat;

		public HediffDef hediff;

		public GameConditionDef gameCondition;

		public bool nullifiedIfNotColonist;

		public ThoughtDef thoughtToMake;

		[NoTranslate]
		private string icon;

		public bool showBubble;

		public int stackLimitForSameOtherPawn = -1;

		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		public float maxCumulatedOpinionOffset = float.MaxValue;

		public TaleDef taleDef;

		[Unsaved]
		private ThoughtWorker workerInt;

		[Unsaved]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		private Texture2D iconInt;

		public ThoughtDef()
		{
		}

		public string Label
		{
			get
			{
				if (!this.label.NullOrEmpty())
				{
					return this.label;
				}
				if (!this.stages.NullOrEmpty<ThoughtStage>())
				{
					if (!this.stages[0].label.NullOrEmpty())
					{
						return this.stages[0].label;
					}
					if (!this.stages[0].labelSocial.NullOrEmpty())
					{
						return this.stages[0].labelSocial;
					}
				}
				Log.Error("Cannot get good label for ThoughtDef " + this.defName, false);
				return this.defName;
			}
		}

		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		public bool IsMemory
		{
			get
			{
				if (this.isMemoryCached == BoolUnknown.Unknown)
				{
					this.isMemoryCached = ((this.durationDays <= 0f && !typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass)) ? BoolUnknown.False : BoolUnknown.True);
				}
				return this.isMemoryCached == BoolUnknown.True;
			}
		}

		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public Type ThoughtClass
		{
			get
			{
				if (this.thoughtClass != null)
				{
					return this.thoughtClass;
				}
				if (this.IsMemory)
				{
					return typeof(Thought_Memory);
				}
				return typeof(Thought_Situational);
			}
		}

		public Texture2D Icon
		{
			get
			{
				if (this.iconInt == null)
				{
					if (this.icon == null)
					{
						return null;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconInt;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in base.ConfigErrors())
			{
				yield return error;
			}
			if (this.stages.NullOrEmpty<ThoughtStage>())
			{
				yield return "no stages";
			}
			if (this.workerClass != null && this.nextThought != null)
			{
				yield return "has a nextThought but also has a workerClass. nextThought only works for memories";
			}
			if (this.IsMemory && this.workerClass != null)
			{
				yield return "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
			}
			if (!this.IsMemory && this.workerClass == null && this.IsSituational)
			{
				yield return "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
			}
			for (int i = 0; i < this.stages.Count; i++)
			{
				if (this.stages[i] != null)
				{
					foreach (string e in this.stages[i].ConfigErrors())
					{
						yield return e;
					}
				}
			}
			yield break;
		}

		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
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

			internal string <error>__1;

			internal int <i>__2;

			internal IEnumerator<string> $locvar1;

			internal string <e>__3;

			internal ThoughtDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

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
					goto IL_F8;
				case 3u:
					goto IL_137;
				case 4u:
					goto IL_176;
				case 5u:
					goto IL_1C5;
				case 6u:
					Block_16:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							e = enumerator2.Current;
							this.$current = e;
							if (!this.$disposing)
							{
								this.$PC = 6;
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
					goto IL_289;
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
						error = enumerator.Current;
						this.$current = error;
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
				if (this.stages.NullOrEmpty<ThoughtStage>())
				{
					this.$current = "no stages";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_F8:
				if (this.workerClass != null && this.nextThought != null)
				{
					this.$current = "has a nextThought but also has a workerClass. nextThought only works for memories";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_137:
				if (base.IsMemory && this.workerClass != null)
				{
					this.$current = "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_176:
				if (!base.IsMemory && this.workerClass == null && base.IsSituational)
				{
					this.$current = "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1C5:
				i = 0;
				goto IL_297;
				IL_289:
				i++;
				IL_297:
				if (i >= this.stages.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.stages[i] != null)
					{
						enumerator2 = this.stages[i].ConfigErrors().GetEnumerator();
						num = 4294967293u;
						goto Block_16;
					}
					goto IL_289;
				}
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
				case 6u:
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
				ThoughtDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new ThoughtDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
