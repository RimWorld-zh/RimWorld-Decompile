using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public static class AgeInjuryUtility
	{
		private const int MaxPermanentInjuryAge = 100;

		private static List<Thing> emptyIngredientsList = new List<Thing>();

		[CompilerGenerated]
		private static Func<BodyPartRecord, float> <>f__am$cache0;

		public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, int age)
		{
			return AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn.def, age);
		}

		private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, int age)
		{
			List<HediffGiverSetDef> sets = raceDef.race.hediffGiverSets;
			if (sets == null)
			{
				yield break;
			}
			for (int i = 0; i < sets.Count; i++)
			{
				List<HediffGiver> givers = sets[i].hediffGivers;
				for (int j = 0; j < givers.Count; j++)
				{
					HediffGiver_Birthday agb = givers[j] as HediffGiver_Birthday;
					if (agb != null)
					{
						float ageFractionOfLifeExpectancy = (float)age / raceDef.race.lifeExpectancy;
						if (Rand.Value < agb.ageFractionChanceCurve.Evaluate(ageFractionOfLifeExpectancy))
						{
							yield return agb;
						}
					}
				}
			}
			yield break;
		}

		public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn)
		{
			float num = (!pawn.RaceProps.IsMechanoid) ? pawn.RaceProps.lifeExpectancy : 2500f;
			float num2 = num / 8f;
			float b = num * 1.5f;
			float chance = (!pawn.RaceProps.Humanlike) ? 0.03f : 0.15f;
			int num3 = 0;
			for (float num4 = num2; num4 < Mathf.Min((float)pawn.ageTracker.AgeBiologicalYears, b); num4 += num2)
			{
				if (Rand.Chance(chance))
				{
					num3++;
				}
			}
			for (int i = 0; i < num3; i++)
			{
				IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
				where x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryBaseChance != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)
				select x;
				if (source.Any<BodyPartRecord>())
				{
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					DamageDef dam = AgeInjuryUtility.RandomPermanentInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0f && pawn.RaceProps.ToolUser);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dam, pawn, bodyPartRecord);
					if (bodyPartRecord.def.pawnGeneratorCanAmputate && Rand.Chance(0.3f))
					{
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = hediffDefFromDamage;
						hediff_MissingPart.Part = bodyPartRecord;
						hediff_MissingPart.IsFresh = false;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_MissingPart))
						{
							pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, null, null);
							if (pawn.RaceProps.Humanlike && bodyPartRecord.def == BodyPartDefOf.Leg && Rand.Chance(0.5f))
							{
								RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, AgeInjuryUtility.emptyIngredientsList, null);
							}
						}
					}
					else if (bodyPartRecord.def.permanentInjuryBaseChance > 0f && hediffDefFromDamage.HasComp(typeof(HediffComp_GetsPermanent)))
					{
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Severity = (float)Rand.RangeInclusive(2, 6);
						hediff_Injury.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
						hediff_Injury.Part = bodyPartRecord;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_Injury))
						{
							pawn.health.AddHediff(hediff_Injury, bodyPartRecord, null, null);
						}
					}
				}
			}
			for (int j = 1; j < pawn.ageTracker.AgeBiologicalYears; j++)
			{
				foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, j))
				{
					hediffGiver_Birthday.TryApplyAndSimulateSeverityChange(pawn, (float)j, tryNotToKillPawn);
					if (pawn.Dead)
					{
						break;
					}
				}
				if (pawn.Dead)
				{
					break;
				}
			}
		}

		private static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite)
		{
			DamageDef result;
			switch (Rand.RangeInclusive(0, 3 + ((!allowFrostbite) ? 0 : 1)))
			{
			case 0:
				result = DamageDefOf.Bullet;
				break;
			case 1:
				result = DamageDefOf.Scratch;
				break;
			case 2:
				result = DamageDefOf.Bite;
				break;
			case 3:
				result = DamageDefOf.Stab;
				break;
			case 4:
				result = DamageDefOf.Frostbite;
				break;
			default:
				throw new Exception();
			}
			return result;
		}

		[DebugOutput]
		public static void PermanentInjuryCalculations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Theoretical injuries=========");
			for (int i = 0; i < 10; i++)
			{
				stringBuilder.AppendLine("#" + i + ":");
				List<HediffDef> list = new List<HediffDef>();
				for (int j = 0; j < 100; j++)
				{
					foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(ThingDefOf.Human, j))
					{
						if (!list.Contains(hediffGiver_Birthday.hediff))
						{
							list.Add(hediffGiver_Birthday.hediff);
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"  age ",
								j,
								" - ",
								hediffGiver_Birthday.hediff
							}));
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString(), false);
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Actual injuries=========");
			for (int k = 0; k < 200; k++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				if (pawn.ageTracker.AgeBiologicalYears >= 40)
				{
					stringBuilder.AppendLine(pawn.Name + " age " + pawn.ageTracker.AgeBiologicalYears);
					foreach (Hediff arg in pawn.health.hediffSet.hediffs)
					{
						stringBuilder.AppendLine(" - " + arg);
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static AgeInjuryUtility()
		{
		}

		[CompilerGenerated]
		private static float <GenerateRandomOldAgeInjuries>m__0(BodyPartRecord x)
		{
			return x.coverageAbs;
		}

		[CompilerGenerated]
		private sealed class <RandomHediffsToGainOnBirthday>c__Iterator0 : IEnumerable, IEnumerable<HediffGiver_Birthday>, IEnumerator, IDisposable, IEnumerator<HediffGiver_Birthday>
		{
			internal ThingDef raceDef;

			internal List<HediffGiverSetDef> <sets>__0;

			internal int <i>__1;

			internal List<HediffGiver> <givers>__2;

			internal int <j>__3;

			internal HediffGiver_Birthday <agb>__4;

			internal int age;

			internal float <ageFractionOfLifeExpectancy>__5;

			internal HediffGiver_Birthday $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RandomHediffsToGainOnBirthday>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					sets = raceDef.race.hediffGiverSets;
					if (sets == null)
					{
						return false;
					}
					i = 0;
					goto IL_139;
				case 1u:
					IL_104:
					break;
				default:
					return false;
				}
				IL_105:
				j++;
				IL_114:
				if (j >= givers.Count)
				{
					i++;
				}
				else
				{
					agb = (givers[j] as HediffGiver_Birthday);
					if (agb == null)
					{
						goto IL_105;
					}
					ageFractionOfLifeExpectancy = (float)age / raceDef.race.lifeExpectancy;
					if (Rand.Value < agb.ageFractionChanceCurve.Evaluate(ageFractionOfLifeExpectancy))
					{
						this.$current = agb;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_104;
				}
				IL_139:
				if (i < sets.Count)
				{
					givers = sets[i].hediffGivers;
					j = 0;
					goto IL_114;
				}
				this.$PC = -1;
				return false;
			}

			HediffGiver_Birthday IEnumerator<HediffGiver_Birthday>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.HediffGiver_Birthday>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<HediffGiver_Birthday> IEnumerable<HediffGiver_Birthday>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				AgeInjuryUtility.<RandomHediffsToGainOnBirthday>c__Iterator0 <RandomHediffsToGainOnBirthday>c__Iterator = new AgeInjuryUtility.<RandomHediffsToGainOnBirthday>c__Iterator0();
				<RandomHediffsToGainOnBirthday>c__Iterator.raceDef = raceDef;
				<RandomHediffsToGainOnBirthday>c__Iterator.age = age;
				return <RandomHediffsToGainOnBirthday>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateRandomOldAgeInjuries>c__AnonStorey1
		{
			internal Pawn pawn;

			public <GenerateRandomOldAgeInjuries>c__AnonStorey1()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryBaseChance != 0f || x.def.pawnGeneratorCanAmputate) && !this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x);
			}
		}
	}
}
