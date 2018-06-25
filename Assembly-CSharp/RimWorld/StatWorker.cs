using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StatWorker
	{
		protected StatDef stat;

		[CompilerGenerated]
		private static Func<PawnCapacityOffset, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnCapacityFactor, int> <>f__am$cache1;

		public StatWorker()
		{
		}

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

		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

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
			if (req.StuffDef != null)
			{
				if (num > 0f || this.stat.applyFactorsIfNegative)
				{
					num *= req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
				}
				num += req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
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
			if (req.StuffDef != null)
			{
				if (baseValueFor > 0f || this.stat.applyFactorsIfNegative)
				{
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

		[CompilerGenerated]
		private static int <GetExplanationUnfinalized>m__0(PawnCapacityOffset hfa)
		{
			return hfa.capacity.listOrder;
		}

		[CompilerGenerated]
		private bool <GetExplanationUnfinalized>m__1(Trait tr)
		{
			return tr.CurrentData.statOffsets != null && tr.CurrentData.statOffsets.Any((StatModifier se) => se.stat == this.stat);
		}

		[CompilerGenerated]
		private bool <GetExplanationUnfinalized>m__2(Trait tr)
		{
			return tr.CurrentData.statFactors != null && tr.CurrentData.statFactors.Any((StatModifier se) => se.stat == this.stat);
		}

		[CompilerGenerated]
		private bool <GetExplanationUnfinalized>m__3(StatModifier se)
		{
			return se.stat == this.stat;
		}

		[CompilerGenerated]
		private bool <GetExplanationUnfinalized>m__4(StatModifier se)
		{
			return se.stat == this.stat;
		}

		[CompilerGenerated]
		private static int <GetExplanationUnfinalized>m__5(PawnCapacityFactor hfa)
		{
			return hfa.capacity.listOrder;
		}

		[CompilerGenerated]
		private bool <GetExplanationUnfinalized>m__6(StatModifier se)
		{
			return se.stat == this.stat;
		}

		[CompilerGenerated]
		private bool <GetExplanationUnfinalized>m__7(StatModifier se)
		{
			return se.stat == this.stat;
		}

		[CompilerGenerated]
		private sealed class <RelevantGear>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn pawn;

			internal List<Apparel>.Enumerator $locvar0;

			internal Apparel <t>__1;

			internal StatDef stat;

			internal List<ThingWithComps>.Enumerator $locvar1;

			internal ThingWithComps <t>__2;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RelevantGear>c__Iterator0()
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
					if (pawn.apparel == null)
					{
						goto IL_E5;
					}
					enumerator = pawn.apparel.WornApparel.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					Block_5:
					try
					{
						switch (num)
						{
						case 2u:
							IL_175:
							break;
						}
						if (enumerator2.MoveNext())
						{
							t2 = enumerator2.Current;
							if (StatWorker.GearAffectsStat(t2.def, stat))
							{
								this.$current = t2;
								if (!this.$disposing)
								{
									this.$PC = 2;
								}
								flag = true;
								return true;
							}
							goto IL_175;
						}
					}
					finally
					{
						if (!flag)
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					goto IL_1A2;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_B8:
						break;
					}
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						if (StatWorker.GearAffectsStat(t.def, stat))
						{
							this.$current = t;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_B8;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				IL_E5:
				if (pawn.equipment != null)
				{
					enumerator2 = pawn.equipment.AllEquipmentListForReading.GetEnumerator();
					num = 4294967293u;
					goto Block_5;
				}
				IL_1A2:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StatWorker.<RelevantGear>c__Iterator0 <RelevantGear>c__Iterator = new StatWorker.<RelevantGear>c__Iterator0();
				<RelevantGear>c__Iterator.pawn = pawn;
				<RelevantGear>c__Iterator.stat = stat;
				return <RelevantGear>c__Iterator;
			}
		}
	}
}
