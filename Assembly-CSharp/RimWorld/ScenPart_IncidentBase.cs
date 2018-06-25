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
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		protected IncidentDef incident;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache0;

		protected ScenPart_IncidentBase()
		{
		}

		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		protected abstract string IncidentTag { get; }

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.incident == null)
				{
					this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
					Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>(), false);
				}
			}
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate()
				{
					this.incident = id;
				});
			}
		}

		[CompilerGenerated]
		private static string <DoIncidentEditInterface>m__0(IncidentDef id)
		{
			return id.LabelCap;
		}

		[CompilerGenerated]
		private Action <DoIncidentEditInterface>m__1(IncidentDef id)
		{
			return delegate()
			{
				this.incident = id;
			};
		}

		[CompilerGenerated]
		private sealed class <GetSummaryListEntries>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string tag;

			internal ScenPart_IncidentBase $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetSummaryListEntries>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (tag == this.IncidentTag)
					{
						this.$current = this.incident.LabelCap;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
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
				ScenPart_IncidentBase.<GetSummaryListEntries>c__Iterator0 <GetSummaryListEntries>c__Iterator = new ScenPart_IncidentBase.<GetSummaryListEntries>c__Iterator0();
				<GetSummaryListEntries>c__Iterator.$this = this;
				<GetSummaryListEntries>c__Iterator.tag = tag;
				return <GetSummaryListEntries>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <DoIncidentEditInterface>c__AnonStorey1
		{
			internal IncidentDef id;

			internal ScenPart_IncidentBase $this;

			public <DoIncidentEditInterface>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.incident = this.id;
			}
		}
	}
}
