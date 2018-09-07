using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RimWorld
{
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		public ScenPart_DisableIncident()
		{
		}

		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.TraderCaravanArrival;
			yield return IncidentDefOf.OrbitalTraderArrival;
			yield return IncidentDefOf.WandererJoin;
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
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
					this.$current = IncidentDefOf.TraderCaravanArrival;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = IncidentDefOf.OrbitalTraderArrival;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = IncidentDefOf.WandererJoin;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = IncidentDefOf.Eclipse;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = IncidentDefOf.ToxicFallout;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = IncidentDefOf.SolarFlare;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
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
				return new ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator0();
			}
		}
	}
}
