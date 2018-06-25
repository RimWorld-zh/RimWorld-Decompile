using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BE1 RID: 3041
	public static class StartingPawnUtility
	{
		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x0600425F RID: 16991 RVA: 0x0022EB2C File Offset: 0x0022CF2C
		private static List<Pawn> StartingAndOptionalPawns
		{
			get
			{
				return Find.GameInitData.startingAndOptionalPawns;
			}
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x0022EB4C File Offset: 0x0022CF4C
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

		// Token: 0x06004261 RID: 16993 RVA: 0x0022EBDC File Offset: 0x0022CFDC
		public static Pawn RandomizeInPlace(Pawn p)
		{
			int index = StartingPawnUtility.StartingAndOptionalPawns.IndexOf(p);
			return StartingPawnUtility.RegenerateStartingPawnInPlace(index);
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x0022EC08 File Offset: 0x0022D008
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

		// Token: 0x06004263 RID: 16995 RVA: 0x0022ECA8 File Offset: 0x0022D0A8
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

		// Token: 0x06004264 RID: 16996 RVA: 0x0022ED78 File Offset: 0x0022D178
		public static bool WorkTypeRequirementsSatisfied()
		{
			bool result;
			if (StartingPawnUtility.StartingAndOptionalPawns.Count == 0)
			{
				result = false;
			}
			else
			{
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
				result = true;
			}
			return result;
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x0022EE84 File Offset: 0x0022D284
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
	}
}
