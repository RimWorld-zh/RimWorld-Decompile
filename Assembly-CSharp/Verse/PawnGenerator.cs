using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Verse
{
	public static class PawnGenerator
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct PawnGenerationStatus
		{
			public Pawn Pawn
			{
				get;
				private set;
			}

			public List<Pawn> PawnsGeneratedInTheMeantime
			{
				get;
				private set;
			}

			public PawnGenerationStatus(Pawn pawn, List<Pawn> pawnsGeneratedInTheMeantime)
			{
				this = default(PawnGenerationStatus);
				this.Pawn = pawn;
				this.PawnsGeneratedInTheMeantime = pawnsGeneratedInTheMeantime;
			}
		}

		private static List<PawnGenerationStatus> pawnsBeingGenerated = new List<PawnGenerationStatus>();

		public const float MaxStartMentalBreakThreshold = 0.4f;

		private static SimpleCurve DefaultAgeGenerationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.05f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 100f),
				true
			},
			{
				new CurvePoint(0.675f, 100f),
				true
			},
			{
				new CurvePoint(0.75f, 30f),
				true
			},
			{
				new CurvePoint(0.875f, 18f),
				true
			},
			{
				new CurvePoint(1f, 10f),
				true
			},
			{
				new CurvePoint(1.125f, 3f),
				true
			},
			{
				new CurvePoint(1.25f, 0f),
				true
			}
		};

		private static readonly SimpleCurve AgeSkillMaxFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 0.7f),
				true
			},
			{
				new CurvePoint(35f, 1f),
				true
			},
			{
				new CurvePoint(60f, 1.6f),
				true
			}
		};

		private static readonly SimpleCurve LevelFinalAdjustmentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 10f),
				true
			},
			{
				new CurvePoint(20f, 16f),
				true
			},
			{
				new CurvePoint(27f, 20f),
				true
			}
		};

		private static readonly SimpleCurve LevelRandomCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 150f),
				true
			},
			{
				new CurvePoint(4f, 150f),
				true
			},
			{
				new CurvePoint(5f, 25f),
				true
			},
			{
				new CurvePoint(10f, 5f),
				true
			},
			{
				new CurvePoint(15f, 0f),
				true
			}
		};

		[CompilerGenerated]
		private static Func<Pawn, bool> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<Pawn, float> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Func<Pawn, float> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Func<Pawn, float> _003C_003Ef__mg_0024cache3;

		public static Pawn GeneratePawn(PawnKindDef kindDef, Faction faction = null)
		{
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null));
		}

		public static Pawn GeneratePawn(PawnGenerationRequest request)
		{
			try
			{
				return PawnGenerator.GeneratePawnInternal(request);
			}
			finally
			{
			}
		}

		private static Pawn GeneratePawnInternal(PawnGenerationRequest request)
		{
			request.EnsureNonNullFaction();
			Pawn pawn = null;
			if (!request.Newborn && !request.ForceGenerateNewPawn)
			{
				if (request.ForceRedressWorldPawnIfFormerColonist)
				{
					IEnumerable<Pawn> validCandidatesToRedress = PawnGenerator.GetValidCandidatesToRedress(request);
					if (validCandidatesToRedress.Where(PawnUtility.EverBeenColonistOrTameAnimal).TryRandomElementByWeight<Pawn>((Func<Pawn, float>)PawnGenerator.WorldPawnSelectionWeight, out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
				if (pawn == null && request.Inhabitant && request.Tile != -1)
				{
					Settlement settlement = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement != null && settlement.previouslyGeneratedInhabitants.Any())
					{
						IEnumerable<Pawn> validCandidatesToRedress2 = PawnGenerator.GetValidCandidatesToRedress(request);
						if ((from x in validCandidatesToRedress2
						where settlement.previouslyGeneratedInhabitants.Contains(x)
						select x).TryRandomElementByWeight<Pawn>((Func<Pawn, float>)PawnGenerator.WorldPawnSelectionWeight, out pawn))
						{
							PawnGenerator.RedressPawn(pawn, request);
							Find.WorldPawns.RemovePawn(pawn);
						}
					}
				}
				if (pawn == null && Rand.Chance(PawnGenerator.ChanceToRedressAnyWorldPawn(request)))
				{
					IEnumerable<Pawn> validCandidatesToRedress3 = PawnGenerator.GetValidCandidatesToRedress(request);
					if (validCandidatesToRedress3.TryRandomElementByWeight<Pawn>((Func<Pawn, float>)PawnGenerator.WorldPawnSelectionWeight, out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
			if (pawn == null)
			{
				pawn = PawnGenerator.GenerateNewNakedPawn(ref request);
				if (pawn == null)
				{
					return null;
				}
				if (!request.Newborn)
				{
					PawnGenerator.GenerateGearFor(pawn, request);
				}
				if (request.Inhabitant && request.Tile != -1)
				{
					Settlement settlement2 = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement2 != null)
					{
						settlement2.previouslyGeneratedInhabitants.Add(pawn);
					}
				}
			}
			if (Find.Scenario != null)
			{
				Find.Scenario.Notify_PawnGenerated(pawn, request.Context);
			}
			return pawn;
		}

		public static void RedressPawn(Pawn pawn, PawnGenerationRequest request)
		{
			try
			{
				pawn.ChangeKind(request.KindDef);
				PawnGenerator.GenerateGearFor(pawn, request);
				if (pawn.Faction != request.Faction)
				{
					pawn.SetFaction(request.Faction, null);
				}
				if (pawn.guest != null)
				{
					pawn.guest.SetGuestStatus(null, false);
				}
			}
			finally
			{
			}
		}

		public static bool IsBeingGenerated(Pawn pawn)
		{
			return PawnGenerator.pawnsBeingGenerated.Any((PawnGenerationStatus x) => x.Pawn == pawn);
		}

		private static bool IsValidCandidateToRedress(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.def != request.KindDef.race)
			{
				return false;
			}
			if (!request.WorldPawnFactionDoesntMatter && pawn.Faction != request.Faction)
			{
				return false;
			}
			if (!request.AllowDead && (pawn.Dead || pawn.Destroyed))
			{
				return false;
			}
			if (!request.AllowDowned && pawn.Downed)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.0010000000474974513)
			{
				return false;
			}
			if (!request.CanGeneratePawnRelations && pawn.RaceProps.IsFlesh && pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return false;
			}
			if (!request.AllowGay && pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return false;
			}
			if (request.Validator != null && !request.Validator(pawn))
			{
				return false;
			}
			if (request.FixedBiologicalAge.HasValue && pawn.ageTracker.AgeBiologicalYearsFloat != request.FixedBiologicalAge)
			{
				return false;
			}
			if (request.FixedChronologicalAge.HasValue && (float)pawn.ageTracker.AgeChronologicalYears != request.FixedChronologicalAge)
			{
				return false;
			}
			if (request.FixedGender.HasValue && pawn.gender != request.FixedGender)
			{
				return false;
			}
			if (request.FixedLastName != null && ((NameTriple)pawn.Name).Last != request.FixedLastName)
			{
				return false;
			}
			if (request.FixedMelanin.HasValue && pawn.story != null && pawn.story.melanin != request.FixedMelanin)
			{
				return false;
			}
			if (request.MustBeCapableOfViolence)
			{
				if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					return false;
				}
				if (pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return false;
				}
			}
			return true;
		}

		private static Pawn GenerateNewNakedPawn(ref PawnGenerationRequest request)
		{
			Pawn pawn = null;
			string text = null;
			bool ignoreScenarioRequirements = false;
			for (int i = 0; i < 100; i++)
			{
				if (i == 70)
				{
					Log.Error("Could not generate a pawn after " + 70 + " tries. Last error: " + text + " Ignoring scenario requirements.");
					ignoreScenarioRequirements = true;
				}
				PawnGenerationRequest pawnGenerationRequest = request;
				pawn = PawnGenerator.TryGenerateNewNakedPawn(ref pawnGenerationRequest, out text, ignoreScenarioRequirements);
				if (pawn != null)
				{
					request = pawnGenerationRequest;
					break;
				}
			}
			if (pawn == null)
			{
				Log.Error("Pawn generation error: " + text + " Too many tries (" + 100 + "), returning null. Generation request: " + request);
				return null;
			}
			return pawn;
		}

		private static Pawn TryGenerateNewNakedPawn(ref PawnGenerationRequest request, out string error, bool ignoreScenarioRequirements)
		{
			error = null;
			Pawn pawn = (Pawn)ThingMaker.MakeThing(request.KindDef.race, null);
			PawnGenerator.pawnsBeingGenerated.Add(new PawnGenerationStatus(pawn, null));
			try
			{
				pawn.kindDef = request.KindDef;
				pawn.SetFactionDirect(request.Faction);
				PawnComponentsUtility.CreateInitialComponents(pawn);
				if (request.FixedGender.HasValue)
				{
					pawn.gender = request.FixedGender.Value;
				}
				else if (pawn.RaceProps.hasGenders)
				{
					if (Rand.Value < 0.5)
					{
						pawn.gender = Gender.Male;
					}
					else
					{
						pawn.gender = Gender.Female;
					}
				}
				else
				{
					pawn.gender = Gender.None;
				}
				PawnGenerator.GenerateRandomAge(pawn, request);
				pawn.needs.SetInitialLevels();
				if (!request.Newborn && request.CanGeneratePawnRelations)
				{
					PawnGenerator.GeneratePawnRelations(pawn, ref request);
				}
				if (pawn.RaceProps.Humanlike)
				{
					pawn.story.melanin = ((!request.FixedMelanin.HasValue) ? PawnSkinColors.RandomMelanin(request.Faction) : request.FixedMelanin.Value);
					pawn.story.crownType = (CrownType)((Rand.Value < 0.5) ? 1 : 2);
					pawn.story.hairColor = PawnHairColors.RandomHairColor(pawn.story.SkinColor, pawn.ageTracker.AgeBiologicalYears);
					PawnBioAndNameGenerator.GiveAppropriateBioAndNameTo(pawn, request.FixedLastName);
					pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, request.Faction.def);
					PawnGenerator.GenerateTraits(pawn, request);
					PawnGenerator.GenerateBodyType(pawn);
					PawnGenerator.GenerateSkills(pawn);
				}
				PawnGenerator.GenerateInitialHediffs(pawn, request);
				if (pawn.workSettings != null && request.Faction.IsPlayer)
				{
					pawn.workSettings.EnableAndInitialize();
				}
				if (request.Faction != null && pawn.RaceProps.Animal)
				{
					pawn.GenerateNecessaryName();
				}
				if (!request.AllowDead && (pawn.Dead || pawn.Destroyed))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated dead pawn.";
					return null;
				}
				if (!request.AllowDowned && pawn.Downed)
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated downed pawn.";
					return null;
				}
				if (request.MustBeCapableOfViolence)
				{
					if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
					{
						goto IL_02b0;
					}
					if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
						goto IL_02b0;
				}
				if (!ignoreScenarioRequirements && request.Context == PawnGenerationContext.PlayerStarter && !Find.Scenario.AllowPlayerStartingPawn(pawn))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated pawn doesn't meet scenario requirements.";
					return null;
				}
				if (request.Validator != null && !request.Validator(pawn))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated pawn didn't pass validator check.";
					return null;
				}
				for (int i = 0; i < PawnGenerator.pawnsBeingGenerated.Count - 1; i++)
				{
					if (PawnGenerator.pawnsBeingGenerated[i].PawnsGeneratedInTheMeantime == null)
					{
						PawnGenerator.pawnsBeingGenerated[i] = new PawnGenerationStatus(PawnGenerator.pawnsBeingGenerated[i].Pawn, new List<Pawn>());
					}
					PawnGenerator.pawnsBeingGenerated[i].PawnsGeneratedInTheMeantime.Add(pawn);
				}
				return pawn;
				IL_02b0:
				PawnGenerator.DiscardGeneratedPawn(pawn);
				error = "Generated pawn incapable of violence.";
				return null;
			}
			finally
			{
				PawnGenerator.pawnsBeingGenerated.RemoveLast();
			}
		}

		private static void DiscardGeneratedPawn(Pawn pawn)
		{
			if (Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.RemovePawn(pawn);
			}
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			List<Pawn> pawnsGeneratedInTheMeantime = PawnGenerator.pawnsBeingGenerated.Last().PawnsGeneratedInTheMeantime;
			if (pawnsGeneratedInTheMeantime != null)
			{
				for (int i = 0; i < pawnsGeneratedInTheMeantime.Count; i++)
				{
					Pawn pawn2 = pawnsGeneratedInTheMeantime[i];
					if (Find.WorldPawns.Contains(pawn2))
					{
						Find.WorldPawns.RemovePawn(pawn2);
					}
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					for (int j = 0; j < PawnGenerator.pawnsBeingGenerated.Count; j++)
					{
						PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime.Remove(pawn2);
					}
				}
			}
		}

		private static IEnumerable<Pawn> GetValidCandidatesToRedress(PawnGenerationRequest request)
		{
			IEnumerable<Pawn> enumerable = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Free);
			if (request.KindDef.factionLeader)
			{
				enumerable = enumerable.Concat(Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader));
			}
			return from x in enumerable
			where PawnGenerator.IsValidCandidateToRedress(x, request)
			select x;
		}

		private static float ChanceToRedressAnyWorldPawn(PawnGenerationRequest request)
		{
			int pawnsBySituationCount = Find.WorldPawns.GetPawnsBySituationCount(WorldPawnSituation.Free);
			float num = Mathf.Min((float)(0.019999999552965164 + 0.0099999997764825821 * ((float)pawnsBySituationCount / 10.0)), 0.8f);
			if (request.MinChanceToRedressWorldPawn.HasValue)
			{
				num = Mathf.Max(num, request.MinChanceToRedressWorldPawn.Value);
			}
			return num;
		}

		private static float WorldPawnSelectionWeight(Pawn p)
		{
			if (p.RaceProps.IsFlesh && !p.relations.everSeenByPlayer && p.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return 0.1f;
			}
			return 1f;
		}

		private static void GenerateGearFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnApparelGenerator.GenerateStartingApparelFor(pawn, request);
			PawnWeaponGenerator.TryGenerateWeaponFor(pawn);
			PawnInventoryGenerator.GenerateInventoryFor(pawn, request);
		}

		private static void GenerateInitialHediffs(Pawn pawn, PawnGenerationRequest request)
		{
			int num = 0;
			while (true)
			{
				AgeInjuryUtility.GenerateRandomOldAgeInjuries(pawn, !request.AllowDead);
				PawnTechHediffsGenerator.GeneratePartsAndImplantsFor(pawn);
				PawnAddictionHediffsGenerator.GenerateAddictionsAndTolerancesFor(pawn);
				if (request.AllowDead && pawn.Dead)
					return;
				if (!request.AllowDowned && pawn.Downed)
				{
					pawn.health.Reset();
					num++;
					if (num > 80)
						break;
					continue;
				}
				return;
			}
			Log.Warning("Could not generate old age injuries for " + pawn.ThingID + " of age " + pawn.ageTracker.AgeBiologicalYears + " that allow pawn to move after " + 80 + " tries. request=" + request);
		}

		private static void GenerateRandomAge(Pawn pawn, PawnGenerationRequest request)
		{
			if (request.FixedBiologicalAge.HasValue && request.FixedChronologicalAge.HasValue)
			{
				float? fixedBiologicalAge = request.FixedBiologicalAge;
				bool hasValue = fixedBiologicalAge.HasValue;
				float? fixedChronologicalAge = request.FixedChronologicalAge;
				if ((hasValue & fixedChronologicalAge.HasValue) && fixedBiologicalAge.GetValueOrDefault() > fixedChronologicalAge.GetValueOrDefault())
				{
					Log.Warning("Tried to generate age for pawn " + pawn + ", but pawn generation request demands biological age (" + request.FixedBiologicalAge + ") to be greater than chronological age (" + request.FixedChronologicalAge + ").");
				}
			}
			if (request.Newborn)
			{
				pawn.ageTracker.AgeBiologicalTicks = 0L;
			}
			else if (request.FixedBiologicalAge.HasValue)
			{
				pawn.ageTracker.AgeBiologicalTicks = (long)(request.FixedBiologicalAge.Value * 3600000.0);
			}
			else
			{
				float num = 0f;
				int num2 = 0;
				while (true)
				{
					num = ((pawn.RaceProps.ageGenerationCurve == null) ? ((!pawn.RaceProps.IsMechanoid) ? (Rand.ByCurve(PawnGenerator.DefaultAgeGenerationCurve, 200) * pawn.RaceProps.lifeExpectancy) : ((float)Rand.Range(0, 2500))) : ((float)Mathf.RoundToInt(Rand.ByCurve(pawn.RaceProps.ageGenerationCurve, 200))));
					num2++;
					if (num2 > 300)
					{
						Log.Error("Tried 300 times to generate age for " + pawn);
						break;
					}
					if (!(num > (float)pawn.kindDef.maxGenerationAge) && !(num < (float)pawn.kindDef.minGenerationAge))
						break;
				}
				pawn.ageTracker.AgeBiologicalTicks = (long)(num * 3600000.0) + Rand.Range(0, 3600000);
			}
			if (request.Newborn)
			{
				pawn.ageTracker.AgeChronologicalTicks = 0L;
			}
			else if (request.FixedChronologicalAge.HasValue)
			{
				pawn.ageTracker.AgeChronologicalTicks = (long)(request.FixedChronologicalAge.Value * 3600000.0);
			}
			else
			{
				int num3;
				if (request.CertainlyBeenInCryptosleep || Rand.Value < pawn.kindDef.backstoryCryptosleepCommonality)
				{
					float value = Rand.Value;
					if (value < 0.699999988079071)
					{
						num3 = Rand.Range(0, 100);
					}
					else if (value < 0.949999988079071)
					{
						num3 = Rand.Range(100, 1000);
					}
					else
					{
						int max = GenDate.Year(GenTicks.TicksAbs, 0f) - 2026 - pawn.ageTracker.AgeBiologicalYears;
						num3 = Rand.Range(1000, max);
					}
				}
				else
				{
					num3 = 0;
				}
				int ticksAbs = GenTicks.TicksAbs;
				long num4 = ticksAbs - pawn.ageTracker.AgeBiologicalTicks;
				num4 -= (long)num3 * 3600000L;
				pawn.ageTracker.BirthAbsTicks = num4;
			}
			if (pawn.ageTracker.AgeBiologicalTicks > pawn.ageTracker.AgeChronologicalTicks)
			{
				pawn.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
			}
		}

		public static int RandomTraitDegree(TraitDef traitDef)
		{
			if (traitDef.degreeDatas.Count == 1)
			{
				return traitDef.degreeDatas[0].degree;
			}
			return traitDef.degreeDatas.RandomElementByWeight((TraitDegreeData dd) => dd.Commonality).degree;
		}

		private static void GenerateTraits(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.story != null)
			{
				if (pawn.story.childhood.forcedTraits != null)
				{
					List<TraitEntry> forcedTraits = pawn.story.childhood.forcedTraits;
					for (int i = 0; i < forcedTraits.Count; i++)
					{
						TraitEntry traitEntry = forcedTraits[i];
						if (traitEntry.def == null)
						{
							Log.Error("Null forced trait def on " + pawn.story.childhood);
						}
						else if (!pawn.story.traits.HasTrait(traitEntry.def))
						{
							pawn.story.traits.GainTrait(new Trait(traitEntry.def, traitEntry.degree, false));
						}
					}
				}
				if (pawn.story.adulthood != null && pawn.story.adulthood.forcedTraits != null)
				{
					List<TraitEntry> forcedTraits2 = pawn.story.adulthood.forcedTraits;
					for (int j = 0; j < forcedTraits2.Count; j++)
					{
						TraitEntry traitEntry2 = forcedTraits2[j];
						if (traitEntry2.def == null)
						{
							Log.Error("Null forced trait def on " + pawn.story.adulthood);
						}
						else if (!pawn.story.traits.HasTrait(traitEntry2.def))
						{
							pawn.story.traits.GainTrait(new Trait(traitEntry2.def, traitEntry2.degree, false));
						}
					}
				}
				int num = Rand.RangeInclusive(2, 3);
				if (request.AllowGay && (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) || LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn)))
				{
					Trait trait = new Trait(TraitDefOf.Gay, PawnGenerator.RandomTraitDegree(TraitDefOf.Gay), false);
					pawn.story.traits.GainTrait(trait);
				}
				while (pawn.story.traits.allTraits.Count < num)
				{
					TraitDef newTraitDef = DefDatabase<TraitDef>.AllDefsListForReading.RandomElementByWeight((TraitDef tr) => tr.GetGenderSpecificCommonality(pawn));
					Trait trait2;
					if (!pawn.story.traits.HasTrait(newTraitDef) && (newTraitDef != TraitDefOf.Gay || (request.AllowGay && !LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) && !LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))) && (request.Faction == null || Faction.OfPlayerSilentFail == null || !request.Faction.HostileTo(Faction.OfPlayer) || newTraitDef.allowOnHostileSpawn) && !pawn.story.traits.allTraits.Any((Trait tr) => newTraitDef.ConflictsWith(tr)) && (newTraitDef.conflictingTraits == null || !newTraitDef.conflictingTraits.Any((TraitDef tr) => pawn.story.traits.HasTrait(tr))) && (newTraitDef.requiredWorkTypes == null || !pawn.story.OneOfWorkTypesIsDisabled(newTraitDef.requiredWorkTypes)) && !pawn.story.WorkTagIsDisabled(newTraitDef.requiredWorkTags))
					{
						int degree = PawnGenerator.RandomTraitDegree(newTraitDef);
						if (!pawn.story.childhood.DisallowsTrait(newTraitDef, degree) && (pawn.story.adulthood == null || !pawn.story.adulthood.DisallowsTrait(newTraitDef, degree)))
						{
							trait2 = new Trait(newTraitDef, degree, false);
							if (pawn.mindState != null && pawn.mindState.mentalBreaker != null)
							{
								float breakThresholdExtreme = pawn.mindState.mentalBreaker.BreakThresholdExtreme;
								breakThresholdExtreme += trait2.OffsetOfStat(StatDefOf.MentalBreakThreshold);
								breakThresholdExtreme *= trait2.MultiplierOfStat(StatDefOf.MentalBreakThreshold);
								if (!(breakThresholdExtreme > 0.40000000596046448))
									goto IL_04be;
								continue;
							}
							goto IL_04be;
						}
					}
					continue;
					IL_04be:
					pawn.story.traits.GainTrait(trait2);
				}
			}
		}

		private static void GenerateBodyType(Pawn pawn)
		{
			if (pawn.story.adulthood != null)
			{
				pawn.story.bodyType = pawn.story.adulthood.BodyTypeFor(pawn.gender);
			}
			else if (Rand.Value < 0.5)
			{
				pawn.story.bodyType = BodyType.Thin;
			}
			else
			{
				pawn.story.bodyType = (BodyType)((pawn.gender != Gender.Female) ? 1 : 2);
			}
		}

		private static void GenerateSkills(Pawn pawn)
		{
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				int num = PawnGenerator.FinalLevelOfSkill(pawn, skillDef);
				SkillRecord skill = pawn.skills.GetSkill(skillDef);
				skill.Level = num;
				if (!skill.TotallyDisabled)
				{
					float num2 = (float)((float)num * 0.10999999940395355);
					float value = Rand.Value;
					if (value < num2)
					{
						if (value < num2 * 0.20000000298023224)
						{
							skill.passion = Passion.Major;
						}
						else
						{
							skill.passion = Passion.Minor;
						}
					}
					skill.xpSinceLastLevel = Rand.Range((float)(skill.XpRequiredForLevelUp * 0.10000000149011612), (float)(skill.XpRequiredForLevelUp * 0.89999997615814209));
				}
			}
		}

		private static int FinalLevelOfSkill(Pawn pawn, SkillDef sk)
		{
			float num = (!sk.usuallyDefinedInBackstories) ? Rand.ByCurve(PawnGenerator.LevelRandomCurve, 100) : ((float)Rand.RangeInclusive(0, 4));
			foreach (Backstory item in from bs in pawn.story.AllBackstories
			where bs != null
			select bs)
			{
				foreach (KeyValuePair<SkillDef, int> item2 in item.skillGainsResolved)
				{
					if (item2.Key == sk)
					{
						num += (float)item2.Value * Rand.Range(1f, 1.4f);
					}
				}
			}
			for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
			{
				int num2 = 0;
				if (pawn.story.traits.allTraits[i].CurrentData.skillGains.TryGetValue(sk, out num2))
				{
					num += (float)num2;
				}
			}
			float num3 = Rand.Range(1f, PawnGenerator.AgeSkillMaxFactorCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears));
			num *= num3;
			num = PawnGenerator.LevelFinalAdjustmentCurve.Evaluate(num);
			return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
		}

		public static void PostProcessGeneratedGear(Thing gear, Pawn pawn)
		{
			CompQuality compQuality = gear.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.RandomGeneratedGearQuality(pawn.kindDef), ArtGenerationContext.Outsider);
			}
			if (gear.def.useHitPoints)
			{
				float randomInRange = pawn.kindDef.gearHealthRange.RandomInRange;
				if (randomInRange < 1.0)
				{
					int b = Mathf.RoundToInt(randomInRange * (float)gear.MaxHitPoints);
					b = (gear.HitPoints = Mathf.Max(1, b));
				}
			}
		}

		private static void GeneratePawnRelations(Pawn pawn, ref PawnGenerationRequest request)
		{
			if (pawn.RaceProps.Humanlike)
			{
				List<KeyValuePair<Pawn, PawnRelationDef>> list = new List<KeyValuePair<Pawn, PawnRelationDef>>();
				List<PawnRelationDef> allDefsListForReading = DefDatabase<PawnRelationDef>.AllDefsListForReading;
				IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
				where x.def == pawn.def
				select x;
				int num = 0;
				foreach (Pawn item in enumerable)
				{
					if (item.Discarded)
					{
						Log.Warning("Warning during generating pawn relations for " + pawn + ": Pawn " + item + " is discarded, yet he was yielded by PawnUtility. Discarding a pawn means that he is no longer managed by anything.");
					}
					else
					{
						num++;
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (!(allDefsListForReading[i].generationChanceFactor <= 0.0))
							{
								list.Add(new KeyValuePair<Pawn, PawnRelationDef>(item, allDefsListForReading[i]));
							}
						}
					}
				}
				float num2 = 82f;
				num2 = (float)(num2 + (float)num * 88.0);
				PawnGenerationRequest localReq = request;
				KeyValuePair<Pawn, PawnRelationDef> keyValuePair = list.RandomElementByWeightWithDefault(delegate(KeyValuePair<Pawn, PawnRelationDef> x)
				{
					if (!x.Value.familyByBloodRelation)
					{
						return 0f;
					}
					return x.Value.generationChanceFactor * x.Value.Worker.GenerationChance(pawn, x.Key, localReq);
				}, num2);
				if (keyValuePair.Key != null)
				{
					keyValuePair.Value.Worker.CreateRelation(pawn, keyValuePair.Key, ref request);
				}
				KeyValuePair<Pawn, PawnRelationDef> keyValuePair2 = list.RandomElementByWeightWithDefault(delegate(KeyValuePair<Pawn, PawnRelationDef> x)
				{
					if (x.Value.familyByBloodRelation)
					{
						return 0f;
					}
					return x.Value.generationChanceFactor * x.Value.Worker.GenerationChance(pawn, x.Key, localReq);
				}, num2);
				if (keyValuePair2.Key != null)
				{
					keyValuePair2.Value.Worker.CreateRelation(pawn, keyValuePair2.Key, ref request);
				}
			}
		}
	}
}
