using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatWorker
	{
		protected StatDef stat;

		public void InitSetStat(StatDef newStat)
		{
			this.stat = newStat;
		}

		public float GetValue(Thing thing, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing), true);
		}

		public float GetValue(StatRequest req, bool applyPostProcess = true)
		{
			float result;
			if (this.stat.minifiedThingInherits)
			{
				MinifiedThing minifiedThing = req.Thing as MinifiedThing;
				if (minifiedThing != null)
				{
					result = minifiedThing.InnerThing.GetStatValue(this.stat, applyPostProcess);
					goto IL_0058;
				}
			}
			float valueUnfinalized = this.GetValueUnfinalized(req, applyPostProcess);
			this.FinalizeValue(req, ref valueUnfinalized, applyPostProcess);
			result = valueUnfinalized;
			goto IL_0058;
			IL_0058:
			return result;
		}

		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

		public virtual float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (Prefs.DevMode && this.IsDisabledFor(req.Thing))
			{
				Log.ErrorOnce(string.Format("Attempted to calculate value for disabled stat {0}; this is meant as a consistency check, either set the stat to neverDisabled or ensure this pawn cannot accidentally use this stat (thing={1})", this.stat, req.Thing.ToStringSafe()), 75193282 + this.stat.index);
			}
			float num = this.GetBaseValueFor(req.Def);
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedOffsets != null)
					{
						for (int i = 0; i < this.stat.skillNeedOffsets.Count; i++)
						{
							num += this.stat.skillNeedOffsets[i].ValueFor(pawn);
						}
					}
				}
				else
				{
					num += this.stat.noSkillOffset;
				}
				if (this.stat.capacityOffsets != null)
				{
					for (int j = 0; j < this.stat.capacityOffsets.Count; j++)
					{
						PawnCapacityOffset pawnCapacityOffset = this.stat.capacityOffsets[j];
						num += pawnCapacityOffset.GetOffset(pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity));
					}
				}
				if (pawn.story != null)
				{
					for (int k = 0; k < pawn.story.traits.allTraits.Count; k++)
					{
						num += pawn.story.traits.allTraits[k].OffsetOfStat(this.stat);
					}
				}
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int l = 0; l < hediffs.Count; l++)
				{
					HediffStage curStage = hediffs[l].CurStage;
					if (curStage != null)
					{
						num += curStage.statOffsets.GetStatOffsetFromList(this.stat);
					}
				}
				if (pawn.apparel != null)
				{
					for (int m = 0; m < pawn.apparel.WornApparel.Count; m++)
					{
						num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[m], this.stat);
					}
				}
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					num += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.stat);
				}
				if (pawn.story != null)
				{
					for (int n = 0; n < pawn.story.traits.allTraits.Count; n++)
					{
						num *= pawn.story.traits.allTraits[n].MultiplierOfStat(this.stat);
					}
				}
				num *= pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
			}
			if (req.StuffDef != null && (num > 0.0 || this.stat.applyFactorsIfNegative))
			{
				num += req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				num *= req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
			}
			if (req.HasThing)
			{
				CompAffectedByFacilities compAffectedByFacilities = req.Thing.TryGetComp<CompAffectedByFacilities>();
				if (compAffectedByFacilities != null)
				{
					num += compAffectedByFacilities.GetStatOffset(this.stat);
				}
				if (this.stat.statFactors != null)
				{
					for (int num2 = 0; num2 < this.stat.statFactors.Count; num2++)
					{
						num *= req.Thing.GetStatValue(this.stat.statFactors[num2], true);
					}
				}
				if (pawn != null)
				{
					if (pawn.skills != null)
					{
						if (this.stat.skillNeedFactors != null)
						{
							for (int num3 = 0; num3 < this.stat.skillNeedFactors.Count; num3++)
							{
								num *= this.stat.skillNeedFactors[num3].ValueFor(pawn);
							}
						}
					}
					else
					{
						num *= this.stat.noSkillFactor;
					}
					if (this.stat.capacityFactors != null)
					{
						for (int num4 = 0; num4 < this.stat.capacityFactors.Count; num4++)
						{
							PawnCapacityFactor pawnCapacityFactor = this.stat.capacityFactors[num4];
							float factor = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity));
							num = Mathf.Lerp(num, num * factor, pawnCapacityFactor.weight);
						}
					}
					if (pawn.Inspired)
					{
						num += pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
						num *= pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					}
				}
			}
			return num;
		}

		public virtual string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			float baseValueFor = this.GetBaseValueFor(req.Def);
			if (baseValueFor != 0.0)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate());
				stringBuilder.AppendLine("    " + this.stat.ValueToString(baseValueFor, numberSense));
				stringBuilder.AppendLine();
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedOffsets != null)
					{
						stringBuilder.AppendLine("StatsReport_Skills".Translate());
						for (int i = 0; i < this.stat.skillNeedOffsets.Count; i++)
						{
							SkillNeed skillNeed = this.stat.skillNeedOffsets[i];
							int level = pawn.skills.GetSkill(skillNeed.skill).Level;
							float val = skillNeed.ValueFor(pawn);
							stringBuilder.AppendLine("    " + skillNeed.skill.LabelCap + " (" + level + "): " + val.ToStringSign() + this.ValueToString(val, false, ToStringNumberSense.Absolute));
						}
						stringBuilder.AppendLine();
					}
				}
				else if (this.stat.noSkillOffset != 0.0)
				{
					stringBuilder.AppendLine("StatsReport_Skills".Translate());
					stringBuilder.AppendLine("    " + "default".Translate().CapitalizeFirst() + " : " + this.stat.noSkillOffset.ToStringSign() + this.ValueToString(this.stat.noSkillOffset, false, ToStringNumberSense.Absolute));
					stringBuilder.AppendLine();
				}
				if (this.stat.capacityOffsets != null)
				{
					stringBuilder.AppendLine((!"StatsReport_Health".CanTranslate()) ? "StatsReport_HealthFactors".Translate() : "StatsReport_Health".Translate());
					foreach (PawnCapacityOffset item in from hfa in this.stat.capacityOffsets
					orderby hfa.capacity.listOrder
					select hfa)
					{
						string text = item.capacity.GetLabelFor(pawn).CapitalizeFirst();
						float level2 = pawn.health.capacities.GetLevel(item.capacity);
						float offset = item.GetOffset(pawn.health.capacities.GetLevel(item.capacity));
						string text2 = this.ValueToString(offset, false, ToStringNumberSense.Absolute);
						string text3 = Mathf.Min(level2, item.max).ToStringPercent() + ", " + "HealthOffsetScale".Translate(item.scale.ToString() + "x");
						if (item.max < 999.0)
						{
							text3 = text3 + ", " + "HealthFactorMaxImpact".Translate(item.max.ToStringPercent());
						}
						stringBuilder.AppendLine("    " + text + ": " + offset.ToStringSign() + text2 + " (" + text3 + ")");
					}
					stringBuilder.AppendLine();
				}
				if ((int)pawn.RaceProps.intelligence >= 1)
				{
					if (pawn.story != null && pawn.story.traits != null)
					{
						List<Trait> list = (from tr in pawn.story.traits.allTraits
						where tr.CurrentData.statOffsets != null && tr.CurrentData.statOffsets.Any((Predicate<StatModifier>)((StatModifier se) => se.stat == this.stat))
						select tr).ToList();
						List<Trait> list2 = (from tr in pawn.story.traits.allTraits
						where tr.CurrentData.statFactors != null && tr.CurrentData.statFactors.Any((Predicate<StatModifier>)((StatModifier se) => se.stat == this.stat))
						select tr).ToList();
						if (list.Count > 0 || list2.Count > 0)
						{
							stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate());
							for (int j = 0; j < list.Count; j++)
							{
								Trait trait = list[j];
								string valueToStringAsOffset = trait.CurrentData.statOffsets.First((Func<StatModifier, bool>)((StatModifier se) => se.stat == this.stat)).ValueToStringAsOffset;
								stringBuilder.AppendLine("    " + trait.LabelCap + ": " + valueToStringAsOffset);
							}
							for (int k = 0; k < list2.Count; k++)
							{
								Trait trait2 = list2[k];
								string toStringAsFactor = trait2.CurrentData.statFactors.First((Func<StatModifier, bool>)((StatModifier se) => se.stat == this.stat)).ToStringAsFactor;
								stringBuilder.AppendLine("    " + trait2.LabelCap + ": " + toStringAsFactor);
							}
							stringBuilder.AppendLine();
						}
					}
					if (StatWorker.RelevantGear(pawn, this.stat).Any())
					{
						stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
						if (pawn.apparel != null)
						{
							for (int l = 0; l < pawn.apparel.WornApparel.Count; l++)
							{
								Apparel gear = pawn.apparel.WornApparel[l];
								stringBuilder.AppendLine(StatWorker.InfoTextLineFromGear(gear, this.stat));
							}
						}
						if (pawn.equipment != null && pawn.equipment.Primary != null)
						{
							stringBuilder.AppendLine(StatWorker.InfoTextLineFromGear(pawn.equipment.Primary, this.stat));
						}
						stringBuilder.AppendLine();
					}
				}
				bool flag = false;
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int m = 0; m < hediffs.Count; m++)
				{
					HediffStage curStage = hediffs[m].CurStage;
					if (curStage != null)
					{
						float statOffsetFromList = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (statOffsetFromList != 0.0)
						{
							if (!flag)
							{
								stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate());
								flag = true;
							}
							stringBuilder.AppendLine("    " + hediffs[m].LabelBase.CapitalizeFirst() + ": " + this.ValueToString(statOffsetFromList, false, ToStringNumberSense.Offset));
							stringBuilder.AppendLine();
						}
					}
				}
				float statFactorFromList = pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList != 1.0)
				{
					stringBuilder.AppendLine("StatsReport_LifeStage".Translate() + " (" + pawn.ageTracker.CurLifeStage.label + "): " + statFactorFromList.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					stringBuilder.AppendLine();
				}
			}
			if (req.StuffDef != null && (baseValueFor > 0.0 || this.stat.applyFactorsIfNegative))
			{
				float statOffsetFromList2 = req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList2 != 0.0)
				{
					stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statOffsetFromList2.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
					stringBuilder.AppendLine();
				}
				float statFactorFromList2 = req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList2 != 1.0)
				{
					stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statFactorFromList2.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					stringBuilder.AppendLine();
				}
			}
			CompAffectedByFacilities compAffectedByFacilities = req.Thing.TryGetComp<CompAffectedByFacilities>();
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.GetStatsExplanation(this.stat, stringBuilder);
			}
			if (this.stat.statFactors != null)
			{
				stringBuilder.AppendLine("StatsReport_OtherStats".Translate());
				for (int n = 0; n < this.stat.statFactors.Count; n++)
				{
					StatDef statDef = this.stat.statFactors[n];
					stringBuilder.AppendLine("    " + statDef.LabelCap + ": x" + statDef.Worker.GetValue(req, true).ToStringPercent());
				}
				stringBuilder.AppendLine();
			}
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedFactors != null)
					{
						stringBuilder.AppendLine("StatsReport_Skills".Translate());
						for (int num = 0; num < this.stat.skillNeedFactors.Count; num++)
						{
							SkillNeed skillNeed2 = this.stat.skillNeedFactors[num];
							int level3 = pawn.skills.GetSkill(skillNeed2.skill).Level;
							stringBuilder.AppendLine("    " + skillNeed2.skill.LabelCap + " (" + level3 + "): x" + skillNeed2.ValueFor(pawn).ToStringPercent());
						}
						stringBuilder.AppendLine();
					}
				}
				else if (this.stat.noSkillFactor != 1.0)
				{
					stringBuilder.AppendLine("StatsReport_Skills".Translate());
					stringBuilder.AppendLine("    " + "default".Translate().CapitalizeFirst() + " : x" + this.stat.noSkillFactor.ToStringPercent());
					stringBuilder.AppendLine();
				}
				if (this.stat.capacityFactors != null)
				{
					stringBuilder.AppendLine((!"StatsReport_Health".CanTranslate()) ? "StatsReport_HealthFactors".Translate() : "StatsReport_Health".Translate());
					if (this.stat.capacityFactors != null)
					{
						foreach (PawnCapacityFactor item2 in from hfa in this.stat.capacityFactors
						orderby hfa.capacity.listOrder
						select hfa)
						{
							string text4 = item2.capacity.GetLabelFor(pawn).CapitalizeFirst();
							float factor = item2.GetFactor(pawn.health.capacities.GetLevel(item2.capacity));
							string text5 = factor.ToStringPercent();
							string text6 = "HealthFactorPercentImpact".Translate(item2.weight.ToStringPercent());
							if (item2.max < 999.0)
							{
								text6 = text6 + ", " + "HealthFactorMaxImpact".Translate(item2.max.ToStringPercent());
							}
							if (item2.allowedDefect != 0.0)
							{
								text6 = text6 + ", " + "HealthFactorAllowedDefect".Translate(((float)(1.0 - item2.allowedDefect)).ToStringPercent());
							}
							stringBuilder.AppendLine("    " + text4 + ": x" + text5 + " (" + text6 + ")");
						}
					}
					stringBuilder.AppendLine();
				}
				if (pawn.Inspired)
				{
					float statOffsetFromList3 = pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
					if (statOffsetFromList3 != 0.0)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + this.ValueToString(statOffsetFromList3, false, ToStringNumberSense.Offset));
						stringBuilder.AppendLine();
					}
					float statFactorFromList3 = pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList3 != 1.0)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + statFactorFromList3.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
						stringBuilder.AppendLine();
					}
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		public virtual void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
		{
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					this.stat.parts[i].TransformValue(req, ref val);
				}
			}
			if (applyPostProcess && this.stat.postProcessCurve != null)
			{
				val = this.stat.postProcessCurve.Evaluate(val);
			}
			if (Find.Scenario != null)
			{
				val *= Find.Scenario.GetStatFactor(this.stat);
			}
			if (Mathf.Abs(val) > this.stat.roundToFiveOver)
			{
				val = (float)(Mathf.Round((float)(val / 5.0)) * 5.0);
			}
			val = Mathf.Clamp(val, this.stat.minValue, this.stat.maxValue);
			if (this.stat.roundValue)
			{
				val = (float)Mathf.RoundToInt(val);
			}
		}

		public virtual string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
						stringBuilder.AppendLine();
					}
				}
			}
			if (this.stat.postProcessCurve != null)
			{
				float value = this.GetValue(req, false);
				float value2 = this.GetValue(req, true);
				if (!Mathf.Approximately(value, value2))
				{
					string text2 = this.ValueToString(value, false, ToStringNumberSense.Absolute);
					string text3 = this.stat.ValueToString(value2, numberSense);
					stringBuilder.AppendLine("StatsReport_PostProcessed".Translate() + ": " + text2 + " => " + text3);
					stringBuilder.AppendLine();
				}
			}
			float statFactor = Find.Scenario.GetStatFactor(this.stat);
			if (statFactor != 1.0)
			{
				stringBuilder.AppendLine("StatsReport_ScenarioFactor".Translate() + ": " + statFactor.ToStringPercent());
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + this.stat.ValueToString(finalVal, this.stat.toStringNumberSense));
			return stringBuilder.ToString();
		}

		public virtual bool ShouldShowFor(BuildableDef eDef)
		{
			bool result;
			if (!this.stat.showIfUndefined && !eDef.statBases.StatListContains(this.stat))
			{
				result = false;
			}
			else
			{
				ThingDef thingDef = eDef as ThingDef;
				if (thingDef != null && thingDef.category == ThingCategory.Pawn)
				{
					if (!this.stat.showOnPawns)
					{
						result = false;
						goto IL_02eb;
					}
					if (!this.stat.showOnHumanlikes && thingDef.race.Humanlike)
					{
						result = false;
						goto IL_02eb;
					}
					if (!this.stat.showOnAnimals && thingDef.race.Animal)
					{
						result = false;
						goto IL_02eb;
					}
					if (!this.stat.showOnMechanoids && thingDef.race.IsMechanoid)
					{
						result = false;
						goto IL_02eb;
					}
				}
				if (this.stat.category == StatCategoryDefOf.BasicsPawn || this.stat.category == StatCategoryDefOf.PawnCombat)
				{
					result = (thingDef != null && thingDef.category == ThingCategory.Pawn);
				}
				else if (this.stat.category == StatCategoryDefOf.PawnMisc || this.stat.category == StatCategoryDefOf.PawnSocial || this.stat.category == StatCategoryDefOf.PawnWork)
				{
					result = (thingDef != null && thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike);
				}
				else if (this.stat.category == StatCategoryDefOf.Building)
				{
					result = (thingDef != null && ((this.stat != StatDefOf.DoorOpenSpeed) ? ((this.stat.showOnNonWorkTables || thingDef.IsWorkTable) && thingDef.category == ThingCategory.Building) : thingDef.IsDoor));
				}
				else if (this.stat.category == StatCategoryDefOf.Apparel)
				{
					result = (thingDef != null && (thingDef.IsApparel || thingDef.category == ThingCategory.Pawn));
				}
				else if (this.stat.category == StatCategoryDefOf.Weapon)
				{
					result = (thingDef != null && (thingDef.IsMeleeWeapon || thingDef.IsRangedWeapon));
				}
				else if (this.stat.category == StatCategoryDefOf.BasicsNonPawn)
				{
					result = (thingDef == null || thingDef.category != ThingCategory.Pawn);
				}
				else if (this.stat.category.displayAllByDefault)
				{
					result = true;
				}
				else
				{
					Log.Error("Unhandled case: " + this.stat + ", " + eDef);
					result = false;
				}
			}
			goto IL_02eb;
			IL_02eb:
			return result;
		}

		public virtual bool IsDisabledFor(Thing thing)
		{
			bool result;
			if (this.stat.neverDisabled || (this.stat.skillNeedFactors.NullOrEmpty() && this.stat.skillNeedOffsets.NullOrEmpty()))
			{
				result = false;
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.story != null)
				{
					if (this.stat.skillNeedFactors != null)
					{
						for (int i = 0; i < this.stat.skillNeedFactors.Count; i++)
						{
							if (pawn.skills.GetSkill(this.stat.skillNeedFactors[i].skill).TotallyDisabled)
								goto IL_009f;
						}
					}
					if (this.stat.skillNeedOffsets != null)
					{
						for (int j = 0; j < this.stat.skillNeedOffsets.Count; j++)
						{
							if (pawn.skills.GetSkill(this.stat.skillNeedOffsets[j].skill).TotallyDisabled)
								goto IL_0106;
						}
					}
				}
				result = false;
			}
			goto IL_0131;
			IL_009f:
			result = true;
			goto IL_0131;
			IL_0106:
			result = true;
			goto IL_0131;
			IL_0131:
			return result;
		}

		public virtual string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq)
		{
			return stat.ValueToString(value, numberSense);
		}

		private static string InfoTextLineFromGear(Thing gear, StatDef stat)
		{
			float f = StatWorker.StatOffsetFromGear(gear, stat);
			return "    " + gear.LabelCap + ": " + f.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset);
		}

		private static float StatOffsetFromGear(Thing gear, StatDef stat)
		{
			return gear.def.equippedStatOffsets.GetStatOffsetFromList(stat);
		}

		private static IEnumerable<Thing> RelevantGear(Pawn pawn, StatDef stat)
		{
			if (pawn.apparel != null)
			{
				foreach (Apparel item in pawn.apparel.WornApparel)
				{
					if (StatWorker.GearAffectsStat(item.def, stat))
					{
						yield return (Thing)item;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (pawn.equipment == null)
				yield break;
			using (List<ThingWithComps>.Enumerator enumerator2 = pawn.equipment.AllEquipmentListForReading.GetEnumerator())
			{
				ThingWithComps t;
				while (true)
				{
					if (enumerator2.MoveNext())
					{
						t = enumerator2.Current;
						if (StatWorker.GearAffectsStat(t.def, stat))
							break;
						continue;
					}
					yield break;
				}
				yield return (Thing)t;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_01ab:
			/*Error near IL_01ac: Unexpected return in MoveNext()*/;
		}

		private static bool GearAffectsStat(ThingDef gearDef, StatDef stat)
		{
			if (gearDef.equippedStatOffsets != null)
			{
				for (int i = 0; i < gearDef.equippedStatOffsets.Count; i++)
				{
					if (gearDef.equippedStatOffsets[i].stat == stat && gearDef.equippedStatOffsets[i].value != 0.0)
						goto IL_0047;
				}
			}
			bool result = false;
			goto IL_006c;
			IL_006c:
			return result;
			IL_0047:
			result = true;
			goto IL_006c;
		}

		private float GetBaseValueFor(BuildableDef def)
		{
			float result = this.stat.defaultBaseValue;
			if (def.statBases != null)
			{
				int num = 0;
				while (num < def.statBases.Count)
				{
					if (def.statBases[num].stat != this.stat)
					{
						num++;
						continue;
					}
					result = def.statBases[num].value;
					break;
				}
			}
			return result;
		}

		public string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			string result;
			if (!finalized)
			{
				result = val.ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense);
			}
			else
			{
				string text = val.ToStringByStyle(this.stat.toStringStyle, numberSense);
				if (numberSense != ToStringNumberSense.Factor && !this.stat.formatString.NullOrEmpty())
				{
					text = string.Format(this.stat.formatString, text);
				}
				result = text;
			}
			return result;
		}
	}
}
