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
	public class StorytellerDef : Def
	{
		public int listOrder = 9999;

		public bool listVisible = true;

		public bool tutorialMode = false;

		public bool disableAdaptiveTraining = false;

		public bool disableAlerts = false;

		public bool disablePermadeath = false;

		public DifficultyDef forcedDifficulty = null;

		[NoTranslate]
		private string portraitLarge;

		[NoTranslate]
		private string portraitTiny;

		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		public float desiredPopulationMin = 3f;

		public float desiredPopulationMax = 10f;

		public float desiredPopulationCritical = 13f;

		public SimpleCurve populationIntentFromPopCurve;

		public SimpleCurve populationIntentFromTimeCurve;

		public float adaptDaysMin = 0f;

		public float adaptDaysGameStartGraceDays = 0f;

		public SimpleCurve pointsFactorFromAdaptDays = null;

		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation = null;

		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation = null;

		public SimpleCurve adaptDaysGrowthRateCurve = null;

		[Unsaved]
		public Texture2D portraitLargeTex;

		[Unsaved]
		public Texture2D portraitTinyTex;

		public StorytellerDef()
		{
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.portraitTiny.NullOrEmpty())
				{
					this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
					this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
				}
			});
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.pointsFactorFromAdaptDays == null)
			{
				yield return "pointsFactorFromAdaptDays is null";
			}
			if (this.adaptDaysLossFromColonistLostByPostPopulation == null)
			{
				yield return "adaptDaysLossFromColonistLostByPostPopulation is null";
			}
			if (this.adaptDaysLossFromColonistViolentlyDownedByPopulation == null)
			{
				yield return "adaptDaysLossFromColonistViolentlyDownedByPopulation is null";
			}
			if (this.adaptDaysGrowthRateCurve == null)
			{
				yield return "adaptDaysGrowthRateCurve is null";
			}
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (string e2 in this.comps[i].ConfigErrors(this))
				{
					yield return e2;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private void <ResolveReferences>m__0()
		{
			if (!this.portraitTiny.NullOrEmpty())
			{
				this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
				this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
			}
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

			internal int <i>__2;

			internal IEnumerator<string> $locvar1;

			internal string <e>__3;

			internal StorytellerDef $this;

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
					if (this.pointsFactorFromAdaptDays == null)
					{
						this.$current = "pointsFactorFromAdaptDays is null";
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
					goto IL_96;
				case 3u:
					goto IL_C5;
				case 4u:
					goto IL_F4;
				case 5u:
					goto IL_10E;
				case 6u:
					Block_11:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							e2 = enumerator2.Current;
							this.$current = e2;
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
					i++;
					goto IL_246;
				default:
					return false;
				}
				if (this.adaptDaysLossFromColonistLostByPostPopulation == null)
				{
					this.$current = "adaptDaysLossFromColonistLostByPostPopulation is null";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_96:
				if (this.adaptDaysLossFromColonistViolentlyDownedByPopulation == null)
				{
					this.$current = "adaptDaysLossFromColonistViolentlyDownedByPopulation is null";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_C5:
				if (this.adaptDaysGrowthRateCurve == null)
				{
					this.$current = "adaptDaysGrowthRateCurve is null";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_F4:
				enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_10E:
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						e = enumerator.Current;
						this.$current = e;
						if (!this.$disposing)
						{
							this.$PC = 5;
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
				i = 0;
				IL_246:
				if (i < this.comps.Count)
				{
					enumerator2 = this.comps[i].ConfigErrors(this).GetEnumerator();
					num = 4294967293u;
					goto Block_11;
				}
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
				case 5u:
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
				StorytellerDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new StorytellerDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
