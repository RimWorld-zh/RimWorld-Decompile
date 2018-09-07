using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		public float onDays;

		public float offDays;

		public float minSpacingDays;

		public FloatRange numIncidentsRange = FloatRange.Zero;

		public SimpleCurve acceptFractionByDaysPassedCurve;

		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		public IncidentDef incident;

		private IncidentCategoryDef category;

		public bool applyRaidBeaconThreatMtbFactor;

		public float forceRaidEnemyBeforeDaysPassed;

		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		public IncidentCategoryDef IncidentCategory
		{
			get
			{
				if (this.incident != null)
				{
					return this.incident.category;
				}
				return this.category;
			}
		}

		public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.incident != null && this.category != null)
			{
				yield return "incident and category should not both be defined";
			}
			if (this.onDays <= 0f)
			{
				yield return "onDays must be above zero";
			}
			if (this.numIncidentsRange.TrueMax <= 0f)
			{
				yield return "numIncidentRange not configured";
			}
			if (this.minSpacingDays * this.numIncidentsRange.TrueMax > this.onDays * 0.9f)
			{
				yield return "minSpacingDays too high compared to max number of incidents.";
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal StorytellerCompProperties_OnOffCycle $this;

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
				switch (num)
				{
				case 0u:
					if (this.incident != null && this.category != null)
					{
						this.$current = "incident and category should not both be defined";
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
					goto IL_A0;
				case 3u:
					goto IL_D9;
				case 4u:
					goto IL_12A;
				default:
					return false;
				}
				if (this.onDays <= 0f)
				{
					this.$current = "onDays must be above zero";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_A0:
				if (this.numIncidentsRange.TrueMax <= 0f)
				{
					this.$current = "numIncidentRange not configured";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_D9:
				if (this.minSpacingDays * this.numIncidentsRange.TrueMax > this.onDays * 0.9f)
				{
					this.$current = "minSpacingDays too high compared to max number of incidents.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_12A:
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StorytellerCompProperties_OnOffCycle.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new StorytellerCompProperties_OnOffCycle.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
