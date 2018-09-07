using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public static class StartingPawnUtility
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		private static List<Pawn> StartingAndOptionalPawns
		{
			get
			{
				return Find.GameInitData.startingAndOptionalPawns;
			}
		}

		public static void ClearAllStartingPawns()
		{
			for (int i = StartingPawnUtility.StartingAndOptionalPawns.Count - 1; i >= 0; i--)
			{
				StartingPawnUtility.StartingAndOptionalPawns[i].relations.ClearAllRelations();
				if (Find.World != null)
				{
					PawnUtility.DestroyStartingColonistFamily(StartingPawnUtility.StartingAndOptionalPawns[i]);
					PawnComponentsUtility.RemoveComponentsOnDespawned(StartingPawnUtility.StartingAndOptionalPawns[i]);
					Find.WorldPawns.PassToWorld(StartingPawnUtility.StartingAndOptionalPawns[i], PawnDiscardDecideMode.Discard);
				}
				StartingPawnUtility.StartingAndOptionalPawns.RemoveAt(i);
			}
		}

		public static Pawn RandomizeInPlace(Pawn p)
		{
			int index = StartingPawnUtility.StartingAndOptionalPawns.IndexOf(p);
			return StartingPawnUtility.RegenerateStartingPawnInPlace(index);
		}

		private static Pawn RegenerateStartingPawnInPlace(int index)
		{
			Pawn pawn = StartingPawnUtility.StartingAndOptionalPawns[index];
			PawnUtility.TryDestroyStartingColonistFamily(pawn);
			pawn.relations.ClearAllRelations();
			PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			StartingPawnUtility.StartingAndOptionalPawns[index] = null;
			for (int i = 0; i < StartingPawnUtility.StartingAndOptionalPawns.Count; i++)
			{
				if (StartingPawnUtility.StartingAndOptionalPawns[i] != null)
				{
					PawnUtility.TryDestroyStartingColonistFamily(StartingPawnUtility.StartingAndOptionalPawns[i]);
				}
			}
			Pawn pawn2 = StartingPawnUtility.NewGeneratedStartingPawn();
			StartingPawnUtility.StartingAndOptionalPawns[index] = pawn2;
			return pawn2;
		}

		public static Pawn NewGeneratedStartingPawn()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer, PawnGenerationContext.PlayerStarter, -1, true, false, false, false, true, TutorSystem.TutorialMode, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = null;
			try
			{
				pawn = PawnGenerator.GeneratePawn(request);
			}
			catch (Exception arg)
			{
				Log.Error("There was an exception thrown by the PawnGenerator during generating a starting pawn. Trying one more time...\nException: " + arg, false);
				pawn = PawnGenerator.GeneratePawn(request);
			}
			pawn.relations.everSeenByPlayer = true;
			PawnComponentsUtility.AddComponentsForSpawn(pawn);
			return pawn;
		}

		public static bool WorkTypeRequirementsSatisfied()
		{
			if (StartingPawnUtility.StartingAndOptionalPawns.Count == 0)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef.requireCapableColonist)
				{
					bool flag = false;
					for (int j = 0; j < Find.GameInitData.startingPawnCount; j++)
					{
						if (!StartingPawnUtility.StartingAndOptionalPawns[j].story.WorkTypeIsDisabled(workTypeDef))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			if (TutorSystem.TutorialMode)
			{
				if (StartingPawnUtility.StartingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).Any((Pawn p) => p.story.WorkTagIsDisabled(WorkTags.Violent)))
				{
					return false;
				}
			}
			return true;
		}

		public static IEnumerable<WorkTypeDef> RequiredWorkTypesDisabledForEveryone()
		{
			List<WorkTypeDef> workTypes = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < workTypes.Count; i++)
			{
				WorkTypeDef wt = workTypes[i];
				if (wt.requireCapableColonist)
				{
					bool oneCanDoWt = false;
					List<Pawn> startingPawns = StartingPawnUtility.StartingAndOptionalPawns;
					for (int j = 0; j < Find.GameInitData.startingPawnCount; j++)
					{
						if (!startingPawns[j].story.WorkTypeIsDisabled(wt))
						{
							oneCanDoWt = true;
							break;
						}
					}
					if (!oneCanDoWt)
					{
						yield return wt;
					}
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private static bool <WorkTypeRequirementsSatisfied>m__0(Pawn p)
		{
			return p.story.WorkTagIsDisabled(WorkTags.Violent);
		}

		[CompilerGenerated]
		private sealed class <RequiredWorkTypesDisabledForEveryone>c__Iterator0 : IEnumerable, IEnumerable<WorkTypeDef>, IEnumerator, IDisposable, IEnumerator<WorkTypeDef>
		{
			internal List<WorkTypeDef> <workTypes>__0;

			internal int <i>__1;

			internal WorkTypeDef <wt>__2;

			internal bool <oneCanDoWt>__2;

			internal List<Pawn> <startingPawns>__2;

			internal WorkTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RequiredWorkTypesDisabledForEveryone>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					workTypes = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					i = 0;
					break;
				case 1u:
					IL_E9:
					i++;
					break;
				default:
					return false;
				}
				if (i >= workTypes.Count)
				{
					this.$PC = -1;
				}
				else
				{
					wt = workTypes[i];
					if (!wt.requireCapableColonist)
					{
						goto IL_E9;
					}
					oneCanDoWt = false;
					startingPawns = StartingPawnUtility.StartingAndOptionalPawns;
					for (int j = 0; j < Find.GameInitData.startingPawnCount; j++)
					{
						if (!startingPawns[j].story.WorkTypeIsDisabled(wt))
						{
							oneCanDoWt = true;
							break;
						}
					}
					if (!oneCanDoWt)
					{
						this.$current = wt;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_E9;
				}
				return false;
			}

			WorkTypeDef IEnumerator<WorkTypeDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.WorkTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<WorkTypeDef> IEnumerable<WorkTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new StartingPawnUtility.<RequiredWorkTypesDisabledForEveryone>c__Iterator0();
			}
		}
	}
}
