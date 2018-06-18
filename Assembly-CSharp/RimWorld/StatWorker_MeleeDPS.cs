using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C6 RID: 2502
	public class StatWorker_MeleeDPS : StatWorker
	{
		// Token: 0x0600380F RID: 14351 RVA: 0x001DDF58 File Offset: 0x001DC358
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x001DDF8C File Offset: 0x001DC38C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				Log.Error("Getting MeleeDPS stat for " + req.Def + " without concrete pawn. This always returns 0.", false);
			}
			return this.GetMeleeDamage(req, applyPostProcess) * this.GetMeleeHitChance(req, applyPostProcess) / this.GetMeleeCooldown(req, applyPostProcess);
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x001DDFE4 File Offset: 0x001DC3E4
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_MeleeDPSExplanation".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_MeleeDamage".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
			stringBuilder.AppendLine("  " + this.GetMeleeDamage(req, true).ToString("0.##"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_Cooldown".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
			stringBuilder.AppendLine("  " + "StatsReport_CooldownFormat".Translate(new object[]
			{
				this.GetMeleeCooldown(req, true).ToString("0.##")
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_MeleeHitChance".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(StatDefOf.MeleeHitChance.Worker.GetExplanationUnfinalized(req, StatDefOf.MeleeHitChance.toStringNumberSense).TrimEndNewlines().Indented("    "));
			stringBuilder.AppendLine();
			stringBuilder.Append(StatDefOf.MeleeHitChance.Worker.GetExplanationFinalizePart(req, StatDefOf.MeleeHitChance.toStringNumberSense, this.GetMeleeHitChance(req, true)).Indented("    "));
			return stringBuilder.ToString();
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x001DE160 File Offset: 0x001DC560
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq)
		{
			return string.Format("{0} ( {1} x {2} / {3} )", new object[]
			{
				value.ToStringByStyle(stat.toStringStyle, numberSense),
				this.GetMeleeDamage(optionalReq, true).ToString("0.##"),
				StatDefOf.MeleeHitChance.ValueToString(this.GetMeleeHitChance(optionalReq, true), ToStringNumberSense.Absolute),
				this.GetMeleeCooldown(optionalReq, true).ToString("0.##")
			});
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x001DE1E0 File Offset: 0x001DC5E0
		private float GetMeleeDamage(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			float result;
			if (pawn == null)
			{
				result = 0f;
			}
			else
			{
				List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList();
				if (updatedAvailableVerbsList.Count == 0)
				{
					result = 0f;
				}
				else
				{
					float num = 0f;
					for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
					{
						if (updatedAvailableVerbsList[i].IsMeleeAttack)
						{
							num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
						}
					}
					float num2 = 0f;
					for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
					{
						if (updatedAvailableVerbsList[j].IsMeleeAttack)
						{
							ThingWithComps ownerEquipment = updatedAvailableVerbsList[j].verb.ownerEquipment;
							num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedMeleeDamageAmount(updatedAvailableVerbsList[j].verb, pawn, ownerEquipment);
						}
					}
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x001DE324 File Offset: 0x001DC724
		private float GetMeleeHitChance(StatRequest req, bool applyPostProcess = true)
		{
			float result;
			if (req.HasThing)
			{
				result = req.Thing.GetStatValue(StatDefOf.MeleeHitChance, applyPostProcess);
			}
			else
			{
				result = req.Def.GetStatValueAbstract(StatDefOf.MeleeHitChance, null);
			}
			return result;
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x001DE370 File Offset: 0x001DC770
		private float GetMeleeCooldown(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			float result;
			if (pawn == null)
			{
				result = 1f;
			}
			else
			{
				List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList();
				if (updatedAvailableVerbsList.Count == 0)
				{
					result = 1f;
				}
				else
				{
					float num = 0f;
					for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
					{
						if (updatedAvailableVerbsList[i].IsMeleeAttack)
						{
							num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
						}
					}
					float num2 = 0f;
					for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
					{
						if (updatedAvailableVerbsList[j].IsMeleeAttack)
						{
							ThingWithComps ownerEquipment = updatedAvailableVerbsList[j].verb.ownerEquipment;
							num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * (float)updatedAvailableVerbsList[j].verb.verbProps.AdjustedCooldownTicks(updatedAvailableVerbsList[j].verb, pawn, ownerEquipment);
						}
					}
					result = num2 / 60f;
				}
			}
			return result;
		}
	}
}
