using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public abstract class PlaceWorker
	{
		protected PlaceWorker()
		{
		}

		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
		}

		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield break;
		}

		[CompilerGenerated]
		private sealed class <DisplayAffordances>c__Iterator0 : IEnumerable, IEnumerable<TerrainAffordanceDef>, IEnumerator, IDisposable, IEnumerator<TerrainAffordanceDef>
		{
			internal TerrainAffordanceDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <DisplayAffordances>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
				return false;
			}

			TerrainAffordanceDef IEnumerator<TerrainAffordanceDef>.Current
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
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.TerrainAffordanceDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<TerrainAffordanceDef> IEnumerable<TerrainAffordanceDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new PlaceWorker.<DisplayAffordances>c__Iterator0();
			}
		}
	}
}
