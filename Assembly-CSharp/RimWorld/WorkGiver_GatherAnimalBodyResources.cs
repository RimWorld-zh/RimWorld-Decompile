using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		protected WorkGiver_GatherAnimalBodyResources()
		{
		}

		protected abstract JobDef JobDef { get; }

		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < pawns.Count; i++)
			{
				yield return pawns[i];
			}
			yield break;
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				result = false;
			}
			else
			{
				CompHasGatherableBodyResource comp = this.GetComp(pawn2);
				if (comp != null && comp.ActiveAndFull && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false))
				{
					LocalTargetInfo target = pawn2;
					if (pawn.CanReserve(target, 1, -1, null, forced))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(this.JobDef, t);
		}

		[CompilerGenerated]
		private sealed class <PotentialWorkThingsGlobal>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn pawn;

			internal List<Pawn> <pawns>__0;

			internal int <i>__1;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PotentialWorkThingsGlobal>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < pawns.Count)
				{
					this.$current = pawns[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorkGiver_GatherAnimalBodyResources.<PotentialWorkThingsGlobal>c__Iterator0 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_GatherAnimalBodyResources.<PotentialWorkThingsGlobal>c__Iterator0();
				<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
				return <PotentialWorkThingsGlobal>c__Iterator;
			}
		}
	}
}
