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
	public class WorkGiver_Train : WorkGiver_InteractAnimal
	{
		public const int MinTrainInterval = 15000;

		public WorkGiver_Train()
		{
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawnList = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < pawnList.Count; i++)
			{
				yield return pawnList[i];
			}
			yield break;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Job result;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				result = null;
			}
			else if (pawn2.Faction != pawn.Faction)
			{
				result = null;
			}
			else if (Find.TickManager.TicksGame < pawn2.mindState.lastAssignedInteractTime + 15000)
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				result = null;
			}
			else if (pawn2.training == null)
			{
				result = null;
			}
			else if (pawn2.training.NextTrainableToTrain() == null)
			{
				result = null;
			}
			else if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				result = null;
			}
			else
			{
				if (pawn2.RaceProps.EatsFood)
				{
					if (!base.HasFoodToInteractAnimal(pawn, pawn2))
					{
						Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
						if (job == null)
						{
							JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans, null);
						}
						return job;
					}
				}
				result = new Job(JobDefOf.Train, t);
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <PotentialWorkThingsGlobal>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn pawn;

			internal List<Pawn> <pawnList>__0;

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
					pawnList = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < pawnList.Count)
				{
					this.$current = pawnList[i];
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
				WorkGiver_Train.<PotentialWorkThingsGlobal>c__Iterator0 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_Train.<PotentialWorkThingsGlobal>c__Iterator0();
				<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
				return <PotentialWorkThingsGlobal>c__Iterator;
			}
		}
	}
}
