using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C3 RID: 2499
	public class StatWorker
	{
		// Token: 0x060037E8 RID: 14312 RVA: 0x001DB3A8 File Offset: 0x001D97A8
		public void InitSetStat(StatDef newStat)
		{
			this.stat = newStat;
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x001DB3B4 File Offset: 0x001D97B4
		public float GetValue(Thing thing, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing), true);
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x001DB3D8 File Offset: 0x001D97D8
		public float GetValue(StatRequest req, bool applyPostProcess = true)
		{
			if (this.stat.minifiedThingInherits)
			{
				MinifiedThing minifiedThing = req.Thing as MinifiedThing;
				if (minifiedThing != null)
				{
					if (minifiedThing.InnerThing == null)
					{
						Log.Error("MinifiedThing's inner thing is null.", false);
					}
					return minifiedThing.InnerThing.GetStatValue(this.stat, applyPostProcess);
				}
			}
			float valueUnfinalized = this.GetValueUnfinalized(req, applyPostProcess);
			this.FinalizeValue(req, ref valueUnfinalized, applyPostProcess);
			return valueUnfinalized;
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x001DB458 File Offset: 0x001D9858
		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x001DB47C File Offset: 0x001D987C
		public virtual float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (Prefs.DevMode && this.IsDisabledFor(req.Thing))
			{
				Log.ErrorOnce(string.Format("Attempted to calculate value for disabled stat {0}; this is meant as a consistency check, either set the stat to neverDisabled or ensure this pawn cannot accidentally use this stat (thing={1})", this.stat, req.Thing.ToStringSafe<Thing>()), 75193282 + (int)this.stat.index, false);
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
				if (pawn.equipment != null)
				{
					if (pawn.equipment.Primary != null)
					{
						num += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.stat);
					}
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
			if (req.StuffDef != null && (num > 0f || this.stat.applyFactorsIfNegative))
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

		// Token: 0x060037ED RID: 14317 RVA: 0x001DB9D4 File Offset: 0x001D9DD4
		public virtual string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			float baseValueFor = this.GetBaseValueFor(req.Def);
			if (baseValueFor != 0f)
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
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"    ",
								skillNeed.skill.LabelCap,
								" (",
								level,
								"): ",
								val.ToStringSign(),
								this.ValueToString(val, false, ToStringNumberSense.Absolute)
							}));
						}
						stringBuilder.AppendLine();
					}
				}
				else if (this.stat.noSkillOffset != 0f)
				{
					stringBuilder.AppendLine("StatsReport_Skills".Translate());
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"    ",
						"default".Translate().CapitalizeFirst(),
						" : ",
						this.stat.noSkillOffset.ToStringSign(),
						this.ValueToString(this.stat.noSkillOffset, false, ToStringNumberSense.Absolute)
					}));
					stringBuilder.AppendLine();
				}
				if (this.stat.capacityOffsets != null)
				{
					stringBuilder.AppendLine((!"StatsReport_Health".CanTranslate()) ? "StatsReport_HealthFactors".Translate() : "StatsReport_Health".Translate());
					foreach (PawnCapacityOffset pawnCapacityOffset in from hfa in this.stat.capacityOffsets
					orderby hfa.capacity.listOrder
					select hfa)
					{
						string text = pawnCapacityOffset.capacity.GetLabelFor(pawn).CapitalizeFirst();
						float level2 = pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity);
						float offset = pawnCapacityOffset.GetOffset(pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity));
						string text2 = this.ValueToString(offset, false, ToStringNumberSense.Absolute);
						string text3 = Mathf.Min(level2, pawnCapacityOffset.max).ToStringPercent() + ", " + "HealthOffsetScale".Translate(new object[]
						{
							pawnCapacityOffset.scale.ToString() + "x"
						});
						if (pawnCapacityOffset.max < 999f)
						{
							text3 = text3 + ", " + "HealthFactorMaxImpact".Translate(new object[]
							{
								pawnCapacityOffset.max.ToStringPercent()
							});
						}
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"    ",
							text,
							": ",
							offset.ToStringSign(),
							text2,
							" (",
							text3,
							")"
						}));
					}
					stringBuilder.AppendLine();
				}
				if (pawn.RaceProps.intelligence >= Intelligence.ToolUser)
				{
					if (pawn.story != null && pawn.story.traits != null)
					{
						List<Trait> list = (from tr in pawn.story.traits.allTraits
						where tr.CurrentData.statOffsets != null && tr.CurrentData.statOffsets.Any((StatModifier se) => se.stat == this.stat)
						select tr).ToList<Trait>();
						List<Trait> list2 = (from tr in pawn.story.traits.allTraits
						where tr.CurrentData.statFactors != null && tr.CurrentData.statFactors.Any((StatModifier se) => se.stat == this.stat)
						select tr).ToList<Trait>();
						if (list.Count > 0 || list2.Count > 0)
						{
							stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate());
							for (int j = 0; j < list.Count; j++)
							{
								Trait trait = list[j];
								string valueToStringAsOffset = trait.CurrentData.statOffsets.First((StatModifier se) => se.stat == this.stat).ValueToStringAsOffset;
								stringBuilder.AppendLine("    " + trait.LabelCap + ": " + valueToStringAsOffset);
							}
							for (int k = 0; k < list2.Count; k++)
							{
								Trait trait2 = list2[k];
								string toStringAsFactor = trait2.CurrentData.statFactors.First((StatModifier se) => se.stat == this.stat).ToStringAsFactor;
								stringBuilder.AppendLine("    " + trait2.LabelCap + ": " + toStringAsFactor);
							}
							stringBuilder.AppendLine();
						}
					}
					if (StatWorker.RelevantGear(pawn, this.stat).Any<Thing>())
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
						if (statOffsetFromList != 0f)
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
				if (statFactorFromList != 1f)
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"StatsReport_LifeStage".Translate(),
						" (",
						pawn.ageTracker.CurLifeStage.label,
						"): ",
						statFactorFromList.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor)
					}));
					stringBuilder.AppendLine();
				}
			}
			if (req.StuffDef != null && (baseValueFor > 0f || this.stat.applyFactorsIfNegative))
			{
				float statOffsetFromList2 = req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList2 != 0f)
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"StatsReport_Material".Translate(),
						" (",
						req.StuffDef.LabelCap,
						"): ",
						statOffsetFromList2.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset)
					}));
					stringBuilder.AppendLine();
				}
				float statFactorFromList2 = req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList2 != 1f)
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"StatsReport_Material".Translate(),
						" (",
						req.StuffDef.LabelCap,
						"): ",
						statFactorFromList2.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor)
					}));
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
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"    ",
								skillNeed2.skill.LabelCap,
								" (",
								level3,
								"): x",
								skillNeed2.ValueFor(pawn).ToStringPercent()
							}));
						}
						stringBuilder.AppendLine();
					}
				}
				else if (this.stat.noSkillFactor != 1f)
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
						foreach (PawnCapacityFactor pawnCapacityFactor in from hfa in this.stat.capacityFactors
						orderby hfa.capacity.listOrder
						select hfa)
						{
							string text4 = pawnCapacityFactor.capacity.GetLabelFor(pawn).CapitalizeFirst();
							float factor = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity));
							string text5 = factor.ToStringPercent();
							string text6 = "HealthFactorPercentImpact".Translate(new object[]
							{
								pawnCapacityFactor.weight.ToStringPercent()
							});
							if (pawnCapacityFactor.max < 999f)
							{
								text6 = text6 + ", " + "HealthFactorMaxImpact".Translate(new object[]
								{
									pawnCapacityFactor.max.ToStringPercent()
								});
							}
							if (pawnCapacityFactor.allowedDefect != 0f)
							{
								text6 = text6 + ", " + "HealthFactorAllowedDefect".Translate(new object[]
								{
									(1f - pawnCapacityFactor.allowedDefect).ToStringPercent()
								});
							}
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"    ",
								text4,
								": x",
								text5,
								" (",
								text6,
								")"
							}));
						}
					}
					stringBuilder.AppendLine();
				}
				if (pawn.Inspired)
				{
					float statOffsetFromList3 = pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
					if (statOffsetFromList3 != 0f)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(new object[]
						{
							pawn.Inspiration.def.LabelCap
						}) + ": " + this.ValueToString(statOffsetFromList3, false, ToStringNumberSense.Offset));
						stringBuilder.AppendLine();
					}
					float statFactorFromList3 = pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList3 != 1f)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(new object[]
						{
							pawn.Inspiration.def.LabelCap
						}) + ": " + statFactorFromList3.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
						stringBuilder.AppendLine();
					}
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x001DC780 File Offset: 0x001DAB80
		public virtual void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
		{
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					this.stat.parts[i].TransformValue(req, ref val);
				}
			}
			if (applyPostProcess)
			{
				if (this.stat.postProcessCurve != null)
				{
					val = this.stat.postProcessCurve.Evaluate(val);
				}
			}
			if (Find.Scenario != null)
			{
				val *= Find.Scenario.GetStatFactor(this.stat);
			}
			if (Mathf.Abs(val) > this.stat.roundToFiveOver)
			{
				val = Mathf.Round(val / 5f) * 5f;
			}
			val = Mathf.Clamp(val, this.stat.minValue, this.stat.maxValue);
			if (this.stat.roundValue)
			{
				val = (float)Mathf.RoundToInt(val);
			}
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x001DC88C File Offset: 0x001DAC8C
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
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"StatsReport_PostProcessed".Translate(),
						": ",
						text2,
						" => ",
						text3
					}));
					stringBuilder.AppendLine();
				}
			}
			float statFactor = Find.Scenario.GetStatFactor(this.stat);
			if (statFactor != 1f)
			{
				stringBuilder.AppendLine("StatsReport_ScenarioFactor".Translate() + ": " + statFactor.ToStringPercent());
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + this.stat.ValueToString(finalVal, this.stat.toStringNumberSense));
			return stringBuilder.ToString();
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x001DCA24 File Offset: 0x001DAE24
		public string GetExplanationFull(StatRequest req, ToStringNumberSense numberSense, float value)
		{
			string result;
			if (this.IsDisabledFor(req.Thing))
			{
				result = "StatsReport_PermanentlyDisabled".Translate();
			}
			else
			{
				string text = this.stat.Worker.GetExplanationUnfinalized(req, numberSense).TrimEndNewlines();
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				result = text + this.stat.Worker.GetExplanationFinalizePart(req, numberSense, value);
			}
			return result;
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x001DCAA4 File Offset: 0x001DAEA4
		public virtual bool ShouldShowFor(StatRequest req)
		{
			bool result;
			if (this.stat.alwaysHide)
			{
				result = false;
			}
			else
			{
				BuildableDef def = req.Def;
				if (!this.stat.showIfUndefined)
				{
					if (!def.statBases.StatListContains(this.stat))
					{
						return false;
					}
				}
				ThingDef thingDef = def as ThingDef;
				if (thingDef != null)
				{
					if (thingDef.category == ThingCategory.Pawn)
					{
						if (!this.stat.showOnPawns)
						{
							return false;
						}
						if (!this.stat.showOnHumanlikes && thingDef.race.Humanlike)
						{
							return false;
						}
						if (!this.stat.showOnNonWildManHumanlikes && thingDef.race.Humanlike)
						{
							Pawn pawn = req.Thing as Pawn;
							if (pawn == null || !pawn.IsWildMan())
							{
								return false;
							}
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
					if (thingDef == null)
					{
						result = false;
					}
					else if (this.stat == StatDefOf.DoorOpenSpeed)
					{
						result = thingDef.IsDoor;
					}
					else
					{
						result = ((this.stat.showOnNonWorkTables || thingDef.IsWorkTable) && thingDef.category == ThingCategory.Building);
					}
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
					Log.Error(string.Concat(new object[]
					{
						"Unhandled case: ",
						this.stat,
						", ",
						def
					}), false);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x001DCE0C File Offset: 0x001DB20C
		public virtual bool IsDisabledFor(Thing thing)
		{
			bool result;
			if (this.stat.neverDisabled || (this.stat.skillNeedFactors.NullOrEmpty<SkillNeed>() && this.stat.skillNeedOffsets.NullOrEmpty<SkillNeed>()))
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
							{
								return true;
							}
						}
					}
					if (this.stat.skillNeedOffsets != null)
					{
						for (int j = 0; j < this.stat.skillNeedOffsets.Count; j++)
						{
							if (pawn.skills.GetSkill(this.stat.skillNeedOffsets[j].skill).TotallyDisabled)
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x001DCF4C File Offset: 0x001DB34C
		public virtual string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq)
		{
			return stat.ValueToString(value, numberSense);
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x001DCF6C File Offset: 0x001DB36C
		private static string InfoTextLineFromGear(Thing gear, StatDef stat)
		{
			float f = StatWorker.StatOffsetFromGear(gear, stat);
			return "    " + gear.LabelCap + ": " + f.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x001DCFAC File Offset: 0x001DB3AC
		private static float StatOffsetFromGear(Thing gear, StatDef stat)
		{
			return gear.def.equippedStatOffsets.GetStatOffsetFromList(stat);
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x001DCFD4 File Offset: 0x001DB3D4
		private static IEnumerable<Thing> RelevantGear(Pawn pawn, StatDef stat)
		{
			if (pawn.apparel != null)
			{
				foreach (Apparel t in pawn.apparel.WornApparel)
				{
					if (StatWorker.GearAffectsStat(t.def, stat))
					{
						yield return t;
					}
				}
			}
			if (pawn.equipment != null)
			{
				foreach (ThingWithComps t2 in pawn.equipment.AllEquipmentListForReading)
				{
					if (StatWorker.GearAffectsStat(t2.def, stat))
					{
						yield return t2;
					}
				}
			}
			yield break;
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x001DD008 File Offset: 0x001DB408
		private static bool GearAffectsStat(ThingDef gearDef, StatDef stat)
		{
			if (gearDef.equippedStatOffsets != null)
			{
				for (int i = 0; i < gearDef.equippedStatOffsets.Count; i++)
				{
					if (gearDef.equippedStatOffsets[i].stat == stat && gearDef.equippedStatOffsets[i].value != 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x001DD084 File Offset: 0x001DB484
		protected float GetBaseValueFor(BuildableDef def)
		{
			float result = this.stat.defaultBaseValue;
			if (def.statBases != null)
			{
				for (int i = 0; i < def.statBases.Count; i++)
				{
					if (def.statBases[i].stat == this.stat)
					{
						result = def.statBases[i].value;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x001DD108 File Offset: 0x001DB508
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

		// Token: 0x040023CD RID: 9165
		protected StatDef stat;
	}
}
