using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	public static class HediffStatsUtility
	{
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
		{
			if (instance != null && instance.Bleeding)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRate.ToStringPercent() + "/" + "LetterDay".Translate(), 0);
			}
			float painOffsetToDisplay = 0f;
			if (instance != null)
			{
				painOffsetToDisplay = instance.PainOffset;
			}
			else if (stage != null)
			{
				painOffsetToDisplay = stage.painOffset;
			}
			if (painOffsetToDisplay != 0.0)
			{
				if (painOffsetToDisplay > 0.0 && painOffsetToDisplay < 0.0099999997764825821)
				{
					painOffsetToDisplay = 0.01f;
				}
				if (painOffsetToDisplay < 0.0 && painOffsetToDisplay > -0.0099999997764825821)
				{
					painOffsetToDisplay = -0.01f;
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), ((float)(painOffsetToDisplay * 100.0)).ToString("+###0;-###0") + "%", 0);
			}
			float painFactorToDisplay = 1f;
			if (instance != null)
			{
				painFactorToDisplay = instance.PainFactor;
			}
			else if (stage != null)
			{
				painFactorToDisplay = stage.painFactor;
			}
			if (painFactorToDisplay != 1.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), "x" + painFactorToDisplay.ToStringPercent(), 0);
			}
			if (stage != null && stage.partEfficiencyOffset != 0.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), 0);
			}
			List<PawnCapacityModifier> capModsToDisplay = null;
			if (instance != null)
			{
				capModsToDisplay = instance.CapMods;
			}
			else if (stage != null)
			{
				capModsToDisplay = stage.capMods;
			}
			if (capModsToDisplay != null)
			{
				for (int j = 0; j < capModsToDisplay.Count; j++)
				{
					if (capModsToDisplay[j].offset != 0.0)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[j].capacity.GetLabelFor(true, true).CapitalizeFirst(), ((float)(capModsToDisplay[j].offset * 100.0)).ToString("+#;-#") + "%", 0);
					}
					if (capModsToDisplay[j].postFactor != 1.0)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[j].capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capModsToDisplay[j].postFactor.ToStringPercent(), 0);
					}
					if (capModsToDisplay[j].SetMaxDefined)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[j].capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate() + " " + capModsToDisplay[j].setMax.ToStringPercent(), 0);
					}
				}
			}
			if (stage != null)
			{
				if (stage.AffectsMemory || stage.AffectsSocialInteractions)
				{
					StringBuilder affectsSb = new StringBuilder();
					if (stage.AffectsMemory)
					{
						if (affectsSb.Length != 0)
						{
							affectsSb.Append(", ");
						}
						affectsSb.Append("MemoryLower".Translate());
					}
					if (stage.AffectsSocialInteractions)
					{
						if (affectsSb.Length != 0)
						{
							affectsSb.Append(", ");
						}
						affectsSb.Append("SocialInteractionsLower".Translate());
					}
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Affects".Translate(), affectsSb.ToString(), 0);
				}
				if (stage.hungerRateFactor != 1.0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), 0);
				}
				if (stage.restFallFactor != 1.0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), 0);
				}
				if (stage.makeImmuneTo != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PreventsInfection".Translate(), GenText.ToCommaList(from im in stage.makeImmuneTo
					select im.label, false).CapitalizeFirst(), 0);
				}
				if (stage.statOffsets != null)
				{
					for (int i = 0; i < stage.statOffsets.Count; i++)
					{
						StatModifier sm = stage.statOffsets[i];
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, sm.stat.LabelCap, sm.ToStringAsOffset, 0);
					}
				}
			}
		}
	}
}
