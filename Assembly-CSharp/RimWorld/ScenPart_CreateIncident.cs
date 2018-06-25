using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		private const float IntervalMidpoint = 30f;

		private const float IntervalDeviation = 15f;

		private float intervalDays = 0f;

		private bool repeat = false;

		private string intervalDaysBuffer;

		private float occurTick = 0f;

		private bool isFinished = false;

		[CompilerGenerated]
		private static Func<Map, bool> <>f__am$cache0;

		public ScenPart_CreateIncident()
		{
		}

		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "intervalDays".Translate(), ref this.intervalDays, ref this.intervalDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect3, "repeat".Translate(), ref this.repeat, false, null, null, false);
		}

		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = 15f * Rand.Gaussian(0f, 1f) + 30f;
			if (this.intervalDays < 0f)
			{
				this.intervalDays = 0f;
			}
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap != null)
			{
				if (!this.isFinished)
				{
					if (this.incident == null)
					{
						Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null", false);
						this.isFinished = true;
					}
					else if ((float)Find.TickManager.TicksGame >= this.occurTick)
					{
						IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.incident.category, (from x in Find.Maps
						where x.IsPlayerHome
						select x).RandomElement<Map>());
						if (!this.incident.Worker.TryExecute(parms))
						{
							this.isFinished = true;
						}
						else if (this.repeat && this.intervalDays > 0f)
						{
							this.occurTick += this.IntervalTicks;
						}
						else
						{
							this.isFinished = true;
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private static bool <Tick>m__0(Map x)
		{
			return x.IsPlayerHome;
		}

		[CompilerGenerated]
		private sealed class <RandomizableIncidents>c__Iterator0 : IEnumerable, IEnumerable<IncidentDef>, IEnumerator, IDisposable, IEnumerator<IncidentDef>
		{
			internal IncidentDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RandomizableIncidents>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = IncidentDefOf.Eclipse;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = IncidentDefOf.ToxicFallout;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = IncidentDefOf.SolarFlare;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			IncidentDef IEnumerator<IncidentDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.IncidentDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IncidentDef> IEnumerable<IncidentDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ScenPart_CreateIncident.<RandomizableIncidents>c__Iterator0();
			}
		}
	}
}
