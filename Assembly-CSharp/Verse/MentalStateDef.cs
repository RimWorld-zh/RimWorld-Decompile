using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public class MentalStateDef : Def
	{
		public Type stateClass = typeof(MentalState);

		public Type workerClass = typeof(MentalStateWorker);

		public MentalStateCategory category = MentalStateCategory.Undefined;

		public bool prisonersCanDo = true;

		public bool unspawnedCanDo = false;

		public bool colonistsOnly = false;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool blockNormalThoughts = false;

		public EffecterDef stateEffecter;

		public TaleDef tale;

		public bool allowBeatfire = false;

		public DrugCategory drugCategory = DrugCategory.Any;

		public bool ignoreDrugPolicy = false;

		public float recoveryMtbDays = 1f;

		public int minTicksBeforeRecovery = 500;

		public int maxTicksBeforeRecovery = 99999999;

		public bool recoverFromSleep = false;

		public ThoughtDef moodRecoveryThought;

		[MustTranslate]
		public string beginLetter;

		[MustTranslate]
		public string beginLetterLabel;

		public LetterDef beginLetterDef;

		public Color nameColor = Color.green;

		[MustTranslate]
		public string recoveryMessage;

		[MustTranslate]
		public string baseInspectLine;

		public bool escapingPrisonersIgnore = false;

		private MentalStateWorker workerInt = null;

		public MentalStateDef()
		{
		}

		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.workerClass != null)
					{
						this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
						this.workerInt.def = this;
					}
				}
				return this.workerInt;
			}
		}

		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (!this.beginLetter.NullOrEmpty() && this.beginLetterLabel.NullOrEmpty())
			{
				yield return "no beginLetter or beginLetterLabel";
			}
			yield break;
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

			internal string <e>__1;

			internal MentalStateDef $this;

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
					goto IL_101;
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
						e = enumerator.Current;
						this.$current = e;
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
				if (this.beginLetter.NullOrEmpty() || !this.beginLetterLabel.NullOrEmpty())
				{
					goto IL_101;
				}
				this.$current = "no beginLetter or beginLetterLabel";
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_101:
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
				MentalStateDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new MentalStateDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
