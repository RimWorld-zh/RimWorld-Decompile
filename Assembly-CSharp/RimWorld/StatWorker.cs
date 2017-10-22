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
			float valueUnfinalized = this.GetValueUnfinalized(req, applyPostProcess);
			this.FinalizeValue(req, ref valueUnfinalized, applyPostProcess);
			return valueUnfinalized;
		}

		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

		public virtual float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num = this.GetBaseValueFor(req.Def);
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.story != null)
				{
					for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
					{
						num += pawn.story.traits.allTraits[i].OffsetOfStat(this.stat);
					}
				}
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int j = 0; j < hediffs.Count; j++)
				{
					HediffStage curStage = hediffs[j].CurStage;
					if (curStage != null)
					{
						num += curStage.statOffsets.GetStatOffsetFromList(this.stat);
					}
				}
				if (pawn.apparel != null)
				{
					for (int k = 0; k < pawn.apparel.WornApparel.Count; k++)
					{
						num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[k], this.stat);
					}
				}
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					num += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.stat);
				}
				if (pawn.story != null)
				{
					for (int l = 0; l < pawn.story.traits.allTraits.Count; l++)
					{
						num *= pawn.story.traits.allTraits[l].MultiplierOfStat(this.stat);
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
					for (int m = 0; m < this.stat.statFactors.Count; m++)
					{
						num *= req.Thing.GetStatValue(this.stat.statFactors[m], true);
					}
				}
				if (pawn != null)
				{
					if (pawn.skills != null)
					{
						if (this.stat.skillNeedFactors != null)
						{
							for (int n = 0; n < this.stat.skillNeedFactors.Count; n++)
							{
								num *= this.stat.skillNeedFactors[n].FactorFor(pawn);
							}
						}
					}
					else if (this.stat.noSkillFactor != 1.0)
					{
						num *= this.stat.noSkillFactor;
					}
					if (this.stat.capacityFactors != null)
					{
						for (int num2 = 0; num2 < this.stat.capacityFactors.Count; num2++)
						{
							PawnCapacityFactor pawnCapacityFactor = this.stat.capacityFactors[num2];
							float factor = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity));
							num = Mathf.Lerp(num, num * factor, pawnCapacityFactor.weight);
						}
					}
				}
			}
			return num;
		}

		public virtual string GetExplanation(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_BaseValue".Translate());
			float baseValueFor = this.GetBaseValueFor(req.Def);
			stringBuilder.AppendLine("    " + this.stat.ValueToString(baseValueFor, numberSense));
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
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
							stringBuilder.AppendLine();
							stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate());
							for (int i = 0; i < list.Count; i++)
							{
								Trait trait = list[i];
								string toStringAsOffset = trait.CurrentData.statOffsets.First((Func<StatModifier, bool>)((StatModifier se) => se.stat == this.stat)).ToStringAsOffset;
								stringBuilder.AppendLine("    " + trait.LabelCap + ": " + toStringAsOffset);
							}
							for (int j = 0; j < list2.Count; j++)
							{
								Trait trait2 = list2[j];
								string toStringAsFactor = trait2.CurrentData.statFactors.First((Func<StatModifier, bool>)((StatModifier se) => se.stat == this.stat)).ToStringAsFactor;
								stringBuilder.AppendLine("    " + trait2.LabelCap + ": " + toStringAsFactor);
							}
						}
					}
					if (StatWorker.RelevantGear(pawn, this.stat).Any())
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
						if (pawn.apparel != null)
						{
							for (int k = 0; k < pawn.apparel.WornApparel.Count; k++)
							{
								Apparel gear = pawn.apparel.WornApparel[k];
								stringBuilder.AppendLine(StatWorker.InfoTextLineFromGear(gear, this.stat));
							}
						}
						if (pawn.equipment != null && pawn.equipment.Primary != null)
						{
							stringBuilder.AppendLine(StatWorker.InfoTextLineFromGear(pawn.equipment.Primary, this.stat));
						}
					}
				}
				bool flag = false;
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int l = 0; l < hediffs.Count; l++)
				{
					HediffStage curStage = hediffs[l].CurStage;
					if (curStage != null)
					{
						float statOffsetFromList = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (statOffsetFromList != 0.0)
						{
							if (!flag)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate());
								flag = true;
							}
							stringBuilder.AppendLine("    " + hediffs[l].LabelBase.CapitalizeFirst() + ": " + statOffsetFromList.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
						}
					}
				}
				float statFactorFromList = pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList != 1.0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_LifeStage".Translate() + " (" + pawn.ageTracker.CurLifeStage.label + "): " + statFactorFromList.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				}
			}
			if (req.StuffDef != null && (baseValueFor > 0.0 || this.stat.applyFactorsIfNegative))
			{
				float statOffsetFromList2 = req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList2 != 0.0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statOffsetFromList2.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
				}
				float statFactorFromList2 = req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList2 != 1.0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statFactorFromList2.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				}
			}
			CompAffectedByFacilities compAffectedByFacilities = req.Thing.TryGetComp<CompAffectedByFacilities>();
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.GetStatsExplanation(this.stat, stringBuilder);
			}
			if (this.stat.statFactors != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_OtherStats".Translate());
				for (int m = 0; m < this.stat.statFactors.Count; m++)
				{
					StatDef statDef = this.stat.statFactors[m];
					stringBuilder.AppendLine("    " + statDef.LabelCap + ": x" + statDef.Worker.GetValue(req, true).ToStringPercent());
				}
			}
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedFactors != null)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("StatsReport_Skills".Translate());
						for (int n = 0; n < this.stat.skillNeedFactors.Count; n++)
						{
							SkillNeed skillNeed = this.stat.skillNeedFactors[n];
							int level = pawn.skills.GetSkill(skillNeed.skill).Level;
							stringBuilder.AppendLine("    " + skillNeed.skill.LabelCap + " (" + level + "): x" + skillNeed.FactorFor(pawn).ToStringPercent());
						}
					}
				}
				else if (this.stat.noSkillFactor != 1.0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_Skills".Translate());
					stringBuilder.AppendLine("    " + "default".Translate().CapitalizeFirst() + " : x" + this.stat.noSkillFactor.ToStringPercent());
				}
				if (this.stat.capacityFactors != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_HealthFactors".Translate());
					if (this.stat.capacityFactors != null)
					{
						foreach (PawnCapacityFactor item in from hfa in this.stat.capacityFactors
						orderby hfa.capacity.listOrder
						select hfa)
						{
							string text = item.capacity.GetLabelFor(pawn).CapitalizeFirst();
							float factor = item.GetFactor(pawn.health.capacities.GetLevel(item.capacity));
							string text2 = factor.ToStringPercent();
							string text3 = "HealthFactorPercentImpact".Translate(item.weight.ToStringPercent());
							if (item.max < 100.0)
							{
								text3 = text3 + ", " + "HealthFactorMaxImpact".Translate(item.max.ToStringPercent());
							}
							stringBuilder.AppendLine("    " + text + ": x" + text2 + " (" + text3 + ")");
						}
					}
				}
			}
			return stringBuilder.ToString();
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

		public virtual void FinalizeExplanation(StringBuilder sb, StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						sb.AppendLine(text);
						sb.AppendLine();
					}
				}
			}
			if (this.stat.postProcessCurve != null)
			{
				float value = this.GetValue(req, false);
				float value2 = this.GetValue(req, true);
				if (!Mathf.Approximately(value, value2))
				{
					string text2 = this.stat.ValueToString(value, numberSense);
					string text3 = this.stat.ValueToString(value2, numberSense);
					sb.AppendLine("StatsReport_PostProcessed".Translate() + ": " + text2 + " -> " + text3);
					sb.AppendLine();
				}
			}
			float statFactor = Find.Scenario.GetStatFactor(this.stat);
			if (statFactor != 1.0)
			{
				sb.AppendLine("StatsReport_ScenarioFactor".Translate() + ": " + statFactor.ToStringPercent());
				sb.AppendLine();
			}
			sb.AppendLine("StatsReport_FinalValue".Translate() + ": " + this.stat.ValueToString(finalVal, this.stat.toStringNumberSense));
		}

		public virtual bool ShouldShowFor(BuildableDef eDef)
		{
			if (!this.stat.showIfUndefined && !eDef.statBases.StatListContains(this.stat))
			{
				return false;
			}
			ThingDef thingDef = eDef as ThingDef;
			if (thingDef != null && thingDef.category == ThingCategory.Pawn)
			{
				if (!this.stat.showOnPawns)
				{
					return false;
				}
				if (!this.stat.showOnHumanlikes && thingDef.race.Humanlike)
				{
					return false;
				}
				if (!this.stat.showOnAnimals && thingDef.race.Animal)
				{
					return false;
				}
				if (!this.stat.showOnMechanoids && thingDef.race.IsMechanoid)
				{
					return false;
				}
			}
			if (this.stat.category != StatCategoryDefOf.BasicsPawn && this.stat.category != StatCategoryDefOf.PawnCombat)
			{
				if (this.stat.category != StatCategoryDefOf.PawnMisc && this.stat.category != StatCategoryDefOf.PawnSocial && this.stat.category != StatCategoryDefOf.PawnWork)
				{
					if (this.stat.category == StatCategoryDefOf.Building)
					{
						if (thingDef == null)
						{
							return false;
						}
						if (this.stat == StatDefOf.DoorOpenSpeed)
						{
							return thingDef.IsDoor;
						}
						return thingDef.category == ThingCategory.Building;
					}
					if (this.stat.category == StatCategoryDefOf.Apparel)
					{
						return thingDef != null && (thingDef.IsApparel || thingDef.category == ThingCategory.Pawn);
					}
					if (this.stat.category == StatCategoryDefOf.Weapon)
					{
						return thingDef != null && (thingDef.IsMeleeWeapon || thingDef.IsRangedWeapon);
					}
					if (this.stat.category == StatCategoryDefOf.BasicsNonPawn)
					{
						return thingDef == null || thingDef.category != ThingCategory.Pawn;
					}
					if (this.stat.category.displayAllByDefault)
					{
						return true;
					}
					Log.Error("Unhandled case: " + this.stat + ", " + eDef);
					return false;
				}
				return thingDef != null && thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike;
			}
			return thingDef != null && thingDef.category == ThingCategory.Pawn;
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
				List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Apparel t2 = enumerator.Current;
						if (StatWorker.GearAffectsStat(t2.def, stat))
						{
							yield return (Thing)t2;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
			}
			if (pawn.equipment != null)
			{
				List<ThingWithComps>.Enumerator enumerator2 = pawn.equipment.AllEquipmentListForReading.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						ThingWithComps t = enumerator2.Current;
						if (StatWorker.GearAffectsStat(t.def, stat))
						{
							yield return (Thing)t;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
		}

		private static bool GearAffectsStat(ThingDef gearDef, StatDef stat)
		{
			if (gearDef.equippedStatOffsets != null)
			{
				for (int i = 0; i < gearDef.equippedStatOffsets.Count; i++)
				{
					if (gearDef.equippedStatOffsets[i].stat == stat && gearDef.equippedStatOffsets[i].value != 0.0)
					{
						return true;
					}
				}
			}
			return false;
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
	}
}
