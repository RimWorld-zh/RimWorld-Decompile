using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	internal static class DebugOutputsInfection
	{
		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<HediffDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<DebugOutputsInfection.InfectionLuck, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Func<DebugOutputsInfection.InfectionLuck, float>, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<HediffDef, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<HediffDef, float> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<HediffDef, float> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<HediffDef, string> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<HediffDef, string> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<Pawn, Pawn, Pawn, string> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<HediffStage, bool> <>f__am$cacheA;

		[CompilerGenerated]
		private static Predicate<HediffStage> <>f__am$cacheB;

		[CompilerGenerated]
		private static Predicate<PawnCapacityModifier> <>f__am$cacheC;

		private static List<Pawn> GenerateDoctorArray()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, (Pawn p) => !p.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i <= 20; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				pawn.skills.GetSkill(SkillDefOf.Medicine).Level = i;
				list.Add(pawn);
			}
			return list;
		}

		private static IEnumerable<HediffDef> InfectionList()
		{
			return from hediff in DefDatabase<HediffDef>.AllDefs
			where hediff.tendable && hediff.HasComp(typeof(HediffComp_TendDuration)) && hediff.HasComp(typeof(HediffComp_Immunizable)) && hediff.lethalSeverity > 0f
			select hediff;
		}

		[DebugOutput]
		public static void Infections()
		{
			Func<DebugOutputsInfection.InfectionLuck, float> ilc = delegate(DebugOutputsInfection.InfectionLuck il)
			{
				float result = 1f;
				if (il == DebugOutputsInfection.InfectionLuck.Bad)
				{
					result = 0.8f;
				}
				if (il == DebugOutputsInfection.InfectionLuck.Good)
				{
					result = 1.2f;
				}
				return result;
			};
			Func<Func<DebugOutputsInfection.InfectionLuck, float>, string> stringizeWithLuck = (Func<DebugOutputsInfection.InfectionLuck, float> func) => string.Format("{0:F2} / {1:F2}", func(DebugOutputsInfection.InfectionLuck.Bad), func(DebugOutputsInfection.InfectionLuck.Good));
			Func<HediffDef, bool> isAnimal = (HediffDef d) => d.defName.Contains("Animal");
			Func<HediffDef, float> revealSeverity = (HediffDef d) => d.stages.First((HediffStage s) => s.becomeVisible).minSeverity;
			Func<HediffDef, float> baseSeverityIncrease = (HediffDef d) => d.CompProps<HediffCompProperties_Immunizable>().severityPerDayNotImmune;
			Func<HediffDef, DebugOutputsInfection.InfectionLuck, float> baseImmunityIncrease = (HediffDef d, DebugOutputsInfection.InfectionLuck il) => d.CompProps<HediffCompProperties_Immunizable>().immunityPerDaySick * ilc(il);
			Func<HediffDef, float, float> tendedSeverityIncrease = (HediffDef d, float tend) => baseSeverityIncrease(d) + d.CompProps<HediffCompProperties_TendDuration>().severityPerDayTended * tend;
			Func<HediffDef, DebugOutputsInfection.InfectionLuck, bool, float> immunityIncrease = delegate(HediffDef d, DebugOutputsInfection.InfectionLuck il, bool bedridden)
			{
				float b = (!isAnimal(d)) ? ThingDefOf.Bed.GetStatValueAbstract(StatDefOf.ImmunityGainSpeedFactor, null) : 1f;
				float num = Mathf.Lerp(1f, b, (!bedridden) ? 0.3f : 1f);
				float num2 = num * StatDefOf.ImmunityGainSpeed.GetStatPart<StatPart_Resting>().factor;
				return baseImmunityIncrease(d, il) * num2;
			};
			Func<HediffDef, DebugOutputsInfection.InfectionLuck, float> immunityOnReveal = (HediffDef d, DebugOutputsInfection.InfectionLuck il) => revealSeverity(d) / baseSeverityIncrease(d) * immunityIncrease(d, il, false);
			Func<HediffDef, DebugOutputsInfection.InfectionLuck, float, float> immunityOnLethality = delegate(HediffDef d, DebugOutputsInfection.InfectionLuck il, float tend)
			{
				float result;
				if (tendedSeverityIncrease(d, tend) <= 0f)
				{
					result = float.PositiveInfinity;
				}
				else
				{
					result = immunityOnReveal(d, il) + (d.lethalSeverity - revealSeverity(d)) / tendedSeverityIncrease(d, tend) * immunityIncrease(d, il, true);
				}
				return result;
			};
			List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
			list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef d) => d.defName + ((!d.stages.Any((HediffStage stage) => stage.capMods.Any((PawnCapacityModifier cap) => cap.capacity == PawnCapacityDefOf.BloodFiltration))) ? "" : " (inaccurate)")));
			list.Add(new TableDataGetter<HediffDef>("lethal\nseverity", (HediffDef d) => d.lethalSeverity.ToString("F2")));
			list.Add(new TableDataGetter<HediffDef>("base\nseverity\nincrease", (HediffDef d) => baseSeverityIncrease(d).ToString("F2")));
			list.Add(new TableDataGetter<HediffDef>("base\nimmunity\nincrease", (HediffDef d) => stringizeWithLuck((DebugOutputsInfection.InfectionLuck il) => baseImmunityIncrease(d, il))));
			list.Add(new TableDataGetter<HediffDef>("immunity\non reveal", (HediffDef d) => stringizeWithLuck((DebugOutputsInfection.InfectionLuck il) => immunityOnReveal(d, il))));
			List<Pawn> source = DebugOutputsInfection.GenerateDoctorArray();
			float tendquality;
			for (tendquality = 0f; tendquality <= 1.01f; tendquality += 0.1f)
			{
				tendquality = Mathf.Clamp01(tendquality);
				Pawn arg = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg2 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg3 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg4 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg5 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= tendquality, null);
				Pawn arg6 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= tendquality, null);
				Pawn arg7 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= tendquality, null);
				Pawn arg8 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= tendquality, null);
				Pawn arg9 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg10 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg11 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg12 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Func<Pawn, Pawn, Pawn, string> func2 = delegate(Pawn low, Pawn exp, Pawn high)
				{
					string arg13 = (low == null) ? "X" : low.skills.GetSkill(SkillDefOf.Medicine).Level.ToString();
					string arg14 = (exp == null) ? "X" : exp.skills.GetSkill(SkillDefOf.Medicine).Level.ToString();
					string arg15 = (high == null) ? "X" : high.skills.GetSkill(SkillDefOf.Medicine).Level.ToString();
					return string.Format("{0}-{1}-{2}", arg13, arg14, arg15);
				};
				string text = func2(arg, arg5, arg9);
				string text2 = func2(arg2, arg6, arg10);
				string text3 = func2(arg3, arg7, arg11);
				string text4 = func2(arg4, arg8, arg12);
				float tq = tendquality;
				list.Add(new TableDataGetter<HediffDef>(string.Format("survival chance at\ntend quality {0}\n\ndoc skill needed:\nno meds:  {1}\nherbal:  {2}\nnormal:  {3}\nglitter:  {4}", new object[]
				{
					tq.ToStringPercent(),
					text,
					text2,
					text3,
					text4
				}), delegate(HediffDef d)
				{
					float num = immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Bad, tq);
					float num2 = immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Good, tq);
					string result;
					if (num == float.PositiveInfinity)
					{
						result = float.PositiveInfinity.ToString();
					}
					else
					{
						result = Mathf.Clamp01((num2 - 1f) / (num2 - num)).ToStringPercent();
					}
					return result;
				}));
			}
			DebugTables.MakeTablesDialog<HediffDef>(DebugOutputsInfection.InfectionList(), list.ToArray());
		}

		[DebugOutput]
		public static void InfectionSimulator()
		{
			LongEventHandler.QueueLongEvent(DebugOutputsInfection.InfectionSimulatorWorker(), "Simulating . . .", null);
		}

		private static IEnumerable InfectionSimulatorWorker()
		{
			int trials = 2;
			List<Pawn> doctors = DebugOutputsInfection.GenerateDoctorArray();
			List<int> testSkill = new List<int>
			{
				4,
				10,
				16
			};
			List<ThingDef> testMedicine = new List<ThingDef>
			{
				null,
				ThingDefOf.MedicineHerbal,
				ThingDefOf.MedicineIndustrial,
				ThingDefOf.MedicineUltratech
			};
			PawnGenerationRequest pawngen = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			int originalTicks = Find.TickManager.TicksGame;
			List<DebugOutputsInfection.InfectionSimRow> results = new List<DebugOutputsInfection.InfectionSimRow>();
			int totalTests = DebugOutputsInfection.InfectionList().Count<HediffDef>() * testMedicine.Count<ThingDef>() * testSkill.Count<int>() * trials;
			int currentTest = 0;
			foreach (HediffDef hediff in DebugOutputsInfection.InfectionList())
			{
				foreach (ThingDef meds in testMedicine)
				{
					foreach (int skill in testSkill)
					{
						DebugOutputsInfection.InfectionSimRow result = default(DebugOutputsInfection.InfectionSimRow);
						result.illness = hediff;
						result.skill = skill;
						result.medicine = meds;
						Pawn doctor = doctors[skill];
						for (int i = 0; i < trials; i++)
						{
							Pawn patient = PawnGenerator.GeneratePawn(pawngen);
							int startTicks = Find.TickManager.TicksGame;
							patient.health.AddHediff(result.illness, null, null, null);
							Hediff activeHediff = patient.health.hediffSet.GetFirstHediffOfDef(result.illness, false);
							while (!patient.Dead && patient.health.hediffSet.HasHediff(result.illness, false))
							{
								if (activeHediff.TendableNow(false))
								{
									activeHediff.Tended(TendUtility.CalculateBaseTendQuality(doctor, patient, meds), 0);
									result.medicineUsed += 1f;
								}
								foreach (Hediff hediff2 in patient.health.hediffSet.GetHediffsTendable())
								{
									hediff2.Tended(TendUtility.CalculateBaseTendQuality(doctor, patient, meds), 0);
								}
								Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
								patient.health.HealthTick();
								if (Find.TickManager.TicksGame % 900 == 0)
								{
									yield return null;
								}
							}
							if (patient.Dead)
							{
								result.deathChance += 1f;
							}
							else
							{
								result.recoveryTimeDays += (Find.TickManager.TicksGame - startTicks).TicksToDays();
							}
							currentTest++;
							LongEventHandler.SetCurrentEventText(string.Format("Simulating ({0}/{1})", currentTest, totalTests));
							yield return null;
						}
						result.recoveryTimeDays /= (float)trials - result.deathChance;
						result.deathChance /= (float)trials;
						result.medicineUsed /= (float)trials;
						results.Add(result);
					}
				}
			}
			IEnumerable<DebugOutputsInfection.InfectionSimRow> dataSources = results;
			TableDataGetter<DebugOutputsInfection.InfectionSimRow>[] array = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>[6];
			array[0] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("defName", (DebugOutputsInfection.InfectionSimRow isr) => isr.illness.defName);
			array[1] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("meds", (DebugOutputsInfection.InfectionSimRow isr) => (isr.medicine == null) ? "(none)" : isr.medicine.defName);
			array[2] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("skill", (DebugOutputsInfection.InfectionSimRow isr) => isr.skill.ToString());
			array[3] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("death chance", (DebugOutputsInfection.InfectionSimRow isr) => isr.deathChance.ToStringPercent());
			array[4] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("recovery time (days)", (DebugOutputsInfection.InfectionSimRow isr) => isr.recoveryTimeDays.ToString("F1"));
			array[5] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("medicine used", (DebugOutputsInfection.InfectionSimRow isr) => isr.medicineUsed.ToString());
			DebugTables.MakeTablesDialog<DebugOutputsInfection.InfectionSimRow>(dataSources, array);
			Find.TickManager.DebugSetTicksGame(originalTicks);
			yield break;
		}

		[CompilerGenerated]
		private static bool <GenerateDoctorArray>m__0(Pawn p)
		{
			return !p.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && p.health.hediffSet.hediffs.Count == 0;
		}

		[CompilerGenerated]
		private static bool <InfectionList>m__1(HediffDef hediff)
		{
			return hediff.tendable && hediff.HasComp(typeof(HediffComp_TendDuration)) && hediff.HasComp(typeof(HediffComp_Immunizable)) && hediff.lethalSeverity > 0f;
		}

		[CompilerGenerated]
		private static float <Infections>m__2(DebugOutputsInfection.InfectionLuck il)
		{
			float result = 1f;
			if (il == DebugOutputsInfection.InfectionLuck.Bad)
			{
				result = 0.8f;
			}
			if (il == DebugOutputsInfection.InfectionLuck.Good)
			{
				result = 1.2f;
			}
			return result;
		}

		[CompilerGenerated]
		private static string <Infections>m__3(Func<DebugOutputsInfection.InfectionLuck, float> func)
		{
			return string.Format("{0:F2} / {1:F2}", func(DebugOutputsInfection.InfectionLuck.Bad), func(DebugOutputsInfection.InfectionLuck.Good));
		}

		[CompilerGenerated]
		private static bool <Infections>m__4(HediffDef d)
		{
			return d.defName.Contains("Animal");
		}

		[CompilerGenerated]
		private static float <Infections>m__5(HediffDef d)
		{
			return d.stages.First((HediffStage s) => s.becomeVisible).minSeverity;
		}

		[CompilerGenerated]
		private static float <Infections>m__6(HediffDef d)
		{
			return d.CompProps<HediffCompProperties_Immunizable>().severityPerDayNotImmune;
		}

		[CompilerGenerated]
		private static string <Infections>m__7(HediffDef d)
		{
			return d.defName + ((!d.stages.Any((HediffStage stage) => stage.capMods.Any((PawnCapacityModifier cap) => cap.capacity == PawnCapacityDefOf.BloodFiltration))) ? "" : " (inaccurate)");
		}

		[CompilerGenerated]
		private static string <Infections>m__8(HediffDef d)
		{
			return d.lethalSeverity.ToString("F2");
		}

		[CompilerGenerated]
		private static string <Infections>m__9(Pawn low, Pawn exp, Pawn high)
		{
			string arg = (low == null) ? "X" : low.skills.GetSkill(SkillDefOf.Medicine).Level.ToString();
			string arg2 = (exp == null) ? "X" : exp.skills.GetSkill(SkillDefOf.Medicine).Level.ToString();
			string arg3 = (high == null) ? "X" : high.skills.GetSkill(SkillDefOf.Medicine).Level.ToString();
			return string.Format("{0}-{1}-{2}", arg, arg2, arg3);
		}

		[CompilerGenerated]
		private static bool <Infections>m__A(HediffStage s)
		{
			return s.becomeVisible;
		}

		[CompilerGenerated]
		private static bool <Infections>m__B(HediffStage stage)
		{
			return stage.capMods.Any((PawnCapacityModifier cap) => cap.capacity == PawnCapacityDefOf.BloodFiltration);
		}

		[CompilerGenerated]
		private static bool <Infections>m__C(PawnCapacityModifier cap)
		{
			return cap.capacity == PawnCapacityDefOf.BloodFiltration;
		}

		private enum InfectionLuck
		{
			Bad,
			Normal,
			Good
		}

		private struct InfectionSimRow
		{
			public HediffDef illness;

			public int skill;

			public ThingDef medicine;

			public float deathChance;

			public float recoveryTimeDays;

			public float medicineUsed;
		}

		[CompilerGenerated]
		private sealed class <Infections>c__AnonStorey1
		{
			internal Func<DebugOutputsInfection.InfectionLuck, float> ilc;

			internal Func<HediffDef, float> baseSeverityIncrease;

			internal Func<HediffDef, bool> isAnimal;

			internal Func<HediffDef, DebugOutputsInfection.InfectionLuck, float> baseImmunityIncrease;

			internal Func<HediffDef, float> revealSeverity;

			internal Func<HediffDef, DebugOutputsInfection.InfectionLuck, bool, float> immunityIncrease;

			internal Func<HediffDef, float, float> tendedSeverityIncrease;

			internal Func<HediffDef, DebugOutputsInfection.InfectionLuck, float> immunityOnReveal;

			internal Func<Func<DebugOutputsInfection.InfectionLuck, float>, string> stringizeWithLuck;

			internal Func<HediffDef, DebugOutputsInfection.InfectionLuck, float, float> immunityOnLethality;

			public <Infections>c__AnonStorey1()
			{
			}

			internal float <>m__0(HediffDef d, DebugOutputsInfection.InfectionLuck il)
			{
				return d.CompProps<HediffCompProperties_Immunizable>().immunityPerDaySick * this.ilc(il);
			}

			internal float <>m__1(HediffDef d, float tend)
			{
				return this.baseSeverityIncrease(d) + d.CompProps<HediffCompProperties_TendDuration>().severityPerDayTended * tend;
			}

			internal float <>m__2(HediffDef d, DebugOutputsInfection.InfectionLuck il, bool bedridden)
			{
				float b = (!this.isAnimal(d)) ? ThingDefOf.Bed.GetStatValueAbstract(StatDefOf.ImmunityGainSpeedFactor, null) : 1f;
				float num = Mathf.Lerp(1f, b, (!bedridden) ? 0.3f : 1f);
				float num2 = num * StatDefOf.ImmunityGainSpeed.GetStatPart<StatPart_Resting>().factor;
				return this.baseImmunityIncrease(d, il) * num2;
			}

			internal float <>m__3(HediffDef d, DebugOutputsInfection.InfectionLuck il)
			{
				return this.revealSeverity(d) / this.baseSeverityIncrease(d) * this.immunityIncrease(d, il, false);
			}

			internal float <>m__4(HediffDef d, DebugOutputsInfection.InfectionLuck il, float tend)
			{
				float result;
				if (this.tendedSeverityIncrease(d, tend) <= 0f)
				{
					result = float.PositiveInfinity;
				}
				else
				{
					result = this.immunityOnReveal(d, il) + (d.lethalSeverity - this.revealSeverity(d)) / this.tendedSeverityIncrease(d, tend) * this.immunityIncrease(d, il, true);
				}
				return result;
			}

			internal string <>m__5(HediffDef d)
			{
				return this.baseSeverityIncrease(d).ToString("F2");
			}

			internal string <>m__6(HediffDef d)
			{
				return this.stringizeWithLuck((DebugOutputsInfection.InfectionLuck il) => this.baseImmunityIncrease(d, il));
			}

			internal string <>m__7(HediffDef d)
			{
				return this.stringizeWithLuck((DebugOutputsInfection.InfectionLuck il) => this.immunityOnReveal(d, il));
			}

			private sealed class <Infections>c__AnonStorey2
			{
				internal HediffDef d;

				internal DebugOutputsInfection.<Infections>c__AnonStorey1 <>f__ref$1;

				public <Infections>c__AnonStorey2()
				{
				}

				internal float <>m__0(DebugOutputsInfection.InfectionLuck il)
				{
					return this.<>f__ref$1.baseImmunityIncrease(this.d, il);
				}
			}

			private sealed class <Infections>c__AnonStorey3
			{
				internal HediffDef d;

				internal DebugOutputsInfection.<Infections>c__AnonStorey1 <>f__ref$1;

				public <Infections>c__AnonStorey3()
				{
				}

				internal float <>m__0(DebugOutputsInfection.InfectionLuck il)
				{
					return this.<>f__ref$1.immunityOnReveal(this.d, il);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Infections>c__AnonStorey4
		{
			internal float tendquality;

			public <Infections>c__AnonStorey4()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <Infections>c__AnonStorey5
		{
			internal float tq;

			internal DebugOutputsInfection.<Infections>c__AnonStorey1 <>f__ref$1;

			internal DebugOutputsInfection.<Infections>c__AnonStorey4 <>f__ref$4;

			public <Infections>c__AnonStorey5()
			{
			}

			internal bool <>m__0(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(this.<>f__ref$4.tendquality - 0.25f);
			}

			internal bool <>m__1(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(this.<>f__ref$4.tendquality - 0.25f);
			}

			internal bool <>m__2(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(this.<>f__ref$4.tendquality - 0.25f);
			}

			internal bool <>m__3(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(this.<>f__ref$4.tendquality - 0.25f);
			}

			internal bool <>m__4(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, null) >= this.<>f__ref$4.tendquality;
			}

			internal bool <>m__5(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= this.<>f__ref$4.tendquality;
			}

			internal bool <>m__6(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= this.<>f__ref$4.tendquality;
			}

			internal bool <>m__7(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= this.<>f__ref$4.tendquality;
			}

			internal bool <>m__8(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(this.<>f__ref$4.tendquality + 0.25f);
			}

			internal bool <>m__9(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(this.<>f__ref$4.tendquality + 0.25f);
			}

			internal bool <>m__A(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(this.<>f__ref$4.tendquality + 0.25f);
			}

			internal bool <>m__B(Pawn doc)
			{
				return TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(this.<>f__ref$4.tendquality + 0.25f);
			}

			internal string <>m__C(HediffDef d)
			{
				float num = this.<>f__ref$1.immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Bad, this.tq);
				float num2 = this.<>f__ref$1.immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Good, this.tq);
				string result;
				if (num == float.PositiveInfinity)
				{
					result = float.PositiveInfinity.ToString();
				}
				else
				{
					result = Mathf.Clamp01((num2 - 1f) / (num2 - num)).ToStringPercent();
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <InfectionSimulatorWorker>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int <trials>__0;

			internal List<Pawn> <doctors>__0;

			internal List<int> <testSkill>__0;

			internal List<ThingDef> <testMedicine>__0;

			internal PawnGenerationRequest <pawngen>__0;

			internal int <originalTicks>__0;

			internal List<DebugOutputsInfection.InfectionSimRow> <results>__0;

			internal int <totalTests>__0;

			internal int <currentTest>__0;

			internal IEnumerator<HediffDef> $locvar0;

			internal HediffDef <hediff>__1;

			internal List<ThingDef>.Enumerator $locvar1;

			internal ThingDef <meds>__2;

			internal List<int>.Enumerator $locvar2;

			internal int <skill>__3;

			internal DebugOutputsInfection.InfectionSimRow <result>__4;

			internal Pawn <doctor>__4;

			internal int <i>__5;

			internal Pawn <patient>__6;

			internal int <startTicks>__6;

			internal Hediff <activeHediff>__6;

			internal IEnumerator<Hediff> $locvar3;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<DebugOutputsInfection.InfectionSimRow, string> <>f__am$cache0;

			private static Func<DebugOutputsInfection.InfectionSimRow, string> <>f__am$cache1;

			private static Func<DebugOutputsInfection.InfectionSimRow, string> <>f__am$cache2;

			private static Func<DebugOutputsInfection.InfectionSimRow, string> <>f__am$cache3;

			private static Func<DebugOutputsInfection.InfectionSimRow, string> <>f__am$cache4;

			private static Func<DebugOutputsInfection.InfectionSimRow, string> <>f__am$cache5;

			[DebuggerHidden]
			public <InfectionSimulatorWorker>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					trials = 2;
					doctors = DebugOutputsInfection.GenerateDoctorArray();
					testSkill = new List<int>
					{
						4,
						10,
						16
					};
					testMedicine = new List<ThingDef>
					{
						null,
						ThingDefOf.MedicineHerbal,
						ThingDefOf.MedicineIndustrial,
						ThingDefOf.MedicineUltratech
					};
					pawngen = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
					originalTicks = Find.TickManager.TicksGame;
					results = new List<DebugOutputsInfection.InfectionSimRow>();
					totalTests = DebugOutputsInfection.InfectionList().Count<HediffDef>() * testMedicine.Count<ThingDef>() * testSkill.Count<int>() * trials;
					currentTest = 0;
					enumerator = DebugOutputsInfection.InfectionList().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
					case 2u:
						Block_10:
						try
						{
							switch (num)
							{
							case 1u:
							case 2u:
								Block_13:
								try
								{
									switch (num)
									{
									case 1u:
										IL_3F7:
										break;
									case 2u:
										i++;
										goto IL_4E1;
									default:
										goto IL_55B;
									}
									IL_3F8:
									if (patient.Dead || !patient.health.hediffSet.HasHediff(result.illness, false))
									{
										if (patient.Dead)
										{
											result.deathChance += 1f;
										}
										else
										{
											result.recoveryTimeDays += (Find.TickManager.TicksGame - startTicks).TicksToDays();
										}
										currentTest++;
										LongEventHandler.SetCurrentEventText(string.Format("Simulating ({0}/{1})", currentTest, totalTests));
										this.$current = null;
										if (!this.$disposing)
										{
											this.$PC = 2;
										}
										flag = true;
										return true;
									}
									if (activeHediff.TendableNow(false))
									{
										activeHediff.Tended(TendUtility.CalculateBaseTendQuality(doctor, patient, meds), 0);
										result.medicineUsed += 1f;
									}
									enumerator4 = patient.health.hediffSet.GetHediffsTendable().GetEnumerator();
									try
									{
										while (enumerator4.MoveNext())
										{
											Hediff hediff2 = enumerator4.Current;
											hediff2.Tended(TendUtility.CalculateBaseTendQuality(doctor, patient, meds), 0);
										}
									}
									finally
									{
										if (enumerator4 != null)
										{
											enumerator4.Dispose();
										}
									}
									Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1);
									patient.health.HealthTick();
									if (Find.TickManager.TicksGame % 900 == 0)
									{
										this.$current = null;
										if (!this.$disposing)
										{
											this.$PC = 1;
										}
										flag = true;
										return true;
									}
									goto IL_3F7;
									IL_4E1:
									if (i < trials)
									{
										patient = PawnGenerator.GeneratePawn(pawngen);
										startTicks = Find.TickManager.TicksGame;
										patient.health.AddHediff(result.illness, null, null, null);
										activeHediff = patient.health.hediffSet.GetFirstHediffOfDef(result.illness, false);
										goto IL_3F8;
									}
									result.recoveryTimeDays /= (float)trials - result.deathChance;
									result.deathChance /= (float)trials;
									result.medicineUsed /= (float)trials;
									results.Add(result);
									IL_55B:
									if (enumerator3.MoveNext())
									{
										skill = enumerator3.Current;
										result = default(DebugOutputsInfection.InfectionSimRow);
										result.illness = hediff;
										result.skill = skill;
										result.medicine = meds;
										doctor = doctors[skill];
										i = 0;
										goto IL_4E1;
									}
								}
								finally
								{
									if (!flag)
									{
										((IDisposable)enumerator3).Dispose();
									}
								}
								break;
							}
							if (enumerator2.MoveNext())
							{
								meds = enumerator2.Current;
								enumerator3 = testSkill.GetEnumerator();
								num = 4294967293u;
								goto Block_13;
							}
						}
						finally
						{
							if (!flag)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						hediff = enumerator.Current;
						enumerator2 = testMedicine.GetEnumerator();
						num = 4294967293u;
						goto Block_10;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IEnumerable<DebugOutputsInfection.InfectionSimRow> dataSources = results;
				TableDataGetter<DebugOutputsInfection.InfectionSimRow>[] array = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>[6];
				array[0] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("defName", (DebugOutputsInfection.InfectionSimRow isr) => isr.illness.defName);
				array[1] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("meds", (DebugOutputsInfection.InfectionSimRow isr) => (isr.medicine == null) ? "(none)" : isr.medicine.defName);
				array[2] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("skill", (DebugOutputsInfection.InfectionSimRow isr) => isr.skill.ToString());
				array[3] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("death chance", (DebugOutputsInfection.InfectionSimRow isr) => isr.deathChance.ToStringPercent());
				array[4] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("recovery time (days)", (DebugOutputsInfection.InfectionSimRow isr) => isr.recoveryTimeDays.ToString("F1"));
				array[5] = new TableDataGetter<DebugOutputsInfection.InfectionSimRow>("medicine used", (DebugOutputsInfection.InfectionSimRow isr) => isr.medicineUsed.ToString());
				DebugTables.MakeTablesDialog<DebugOutputsInfection.InfectionSimRow>(dataSources, array);
				Find.TickManager.DebugSetTicksGame(originalTicks);
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
				case 2u:
					try
					{
						try
						{
							try
							{
							}
							finally
							{
								((IDisposable)enumerator3).Dispose();
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new DebugOutputsInfection.<InfectionSimulatorWorker>c__Iterator0();
			}

			private static string <>m__0(DebugOutputsInfection.InfectionSimRow isr)
			{
				return isr.illness.defName;
			}

			private static string <>m__1(DebugOutputsInfection.InfectionSimRow isr)
			{
				return (isr.medicine == null) ? "(none)" : isr.medicine.defName;
			}

			private static string <>m__2(DebugOutputsInfection.InfectionSimRow isr)
			{
				return isr.skill.ToString();
			}

			private static string <>m__3(DebugOutputsInfection.InfectionSimRow isr)
			{
				return isr.deathChance.ToStringPercent();
			}

			private static string <>m__4(DebugOutputsInfection.InfectionSimRow isr)
			{
				return isr.recoveryTimeDays.ToString("F1");
			}

			private static string <>m__5(DebugOutputsInfection.InfectionSimRow isr)
			{
				return isr.medicineUsed.ToString();
			}
		}
	}
}
