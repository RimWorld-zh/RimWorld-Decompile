using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;

namespace Verse
{
	public static class HediffStatsUtility
	{
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
		{
			if (instance != null && instance.Bleeding)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRate.ToStringPercent() + "/" + "LetterDay".Translate(), 0, string.Empty);
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
			if (painOffsetToDisplay != 0f)
			{
				if (painOffsetToDisplay > 0f && painOffsetToDisplay < 0.01f)
				{
					painOffsetToDisplay = 0.01f;
				}
				if (painOffsetToDisplay < 0f && painOffsetToDisplay > -0.01f)
				{
					painOffsetToDisplay = -0.01f;
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), (painOffsetToDisplay * 100f).ToString("+###0;-###0") + "%", 0, string.Empty);
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
			if (painFactorToDisplay != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), "x" + painFactorToDisplay.ToStringPercent(), 0, string.Empty);
			}
			if (stage != null && stage.partEfficiencyOffset != 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), 0, string.Empty);
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
				for (int i = 0; i < capModsToDisplay.Count; i++)
				{
					if (capModsToDisplay[i].offset != 0f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), (capModsToDisplay[i].offset * 100f).ToString("+#;-#") + "%", 0, string.Empty);
					}
					if (capModsToDisplay[i].postFactor != 1f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capModsToDisplay[i].postFactor.ToStringPercent(), 0, string.Empty);
					}
					if (capModsToDisplay[i].SetMaxDefined)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate() + " " + capModsToDisplay[i].setMax.ToStringPercent(), 0, string.Empty);
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
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Affects".Translate(), affectsSb.ToString(), 0, string.Empty);
				}
				if (stage.hungerRateFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), 0, string.Empty);
				}
				if (stage.hungerRateFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), 0, string.Empty);
				}
				if (stage.restFallFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), 0, string.Empty);
				}
				if (stage.restFallFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), 0, string.Empty);
				}
				if (stage.makeImmuneTo != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PreventsInfection".Translate(), (from im in stage.makeImmuneTo
					select im.label).ToCommaList(false).CapitalizeFirst(), 0, string.Empty);
				}
				if (stage.statOffsets != null)
				{
					for (int j = 0; j < stage.statOffsets.Count; j++)
					{
						StatModifier sm = stage.statOffsets[j];
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, sm.stat.LabelCap, sm.ValueToStringAsOffset, 0, string.Empty);
					}
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator0 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal Hediff instance;

			internal float <painOffsetToDisplay>__1;

			internal HediffStage stage;

			internal float <painFactorToDisplay>__2;

			internal List<PawnCapacityModifier> <capModsToDisplay>__3;

			internal int <i>__4;

			internal StringBuilder <affectsSb>__5;

			internal int <i>__6;

			internal StatModifier <sm>__7;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<HediffDef, string> <>f__am$cache0;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (instance != null && instance.Bleeding)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRate.ToStringPercent() + "/" + "LetterDay".Translate(), 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1CD;
				case 3u:
					goto IL_26E;
				case 4u:
					goto IL_2D4;
				case 5u:
					IL_3CE:
					if (capModsToDisplay[i].postFactor != 1f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capModsToDisplay[i].postFactor.ToStringPercent(), 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						return true;
					}
					goto IL_45F;
				case 6u:
					goto IL_45F;
				case 7u:
					goto IL_4F5;
				case 8u:
					IL_61C:
					if (stage.hungerRateFactor != 1f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 9;
						}
						return true;
					}
					goto IL_680;
				case 9u:
					goto IL_680;
				case 10u:
					goto IL_6EF;
				case 11u:
					goto IL_753;
				case 12u:
					goto IL_7C2;
				case 13u:
					goto IL_83F;
				case 14u:
					j++;
					goto IL_8CB;
				default:
					return false;
				}
				painOffsetToDisplay = 0f;
				if (instance != null)
				{
					painOffsetToDisplay = instance.PainOffset;
				}
				else if (stage != null)
				{
					painOffsetToDisplay = stage.painOffset;
				}
				if (painOffsetToDisplay != 0f)
				{
					if (painOffsetToDisplay > 0f && painOffsetToDisplay < 0.01f)
					{
						painOffsetToDisplay = 0.01f;
					}
					if (painOffsetToDisplay < 0f && painOffsetToDisplay > -0.01f)
					{
						painOffsetToDisplay = -0.01f;
					}
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), (painOffsetToDisplay * 100f).ToString("+###0;-###0") + "%", 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_1CD:
				painFactorToDisplay = 1f;
				if (instance != null)
				{
					painFactorToDisplay = instance.PainFactor;
				}
				else if (stage != null)
				{
					painFactorToDisplay = stage.painFactor;
				}
				if (painFactorToDisplay != 1f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), "x" + painFactorToDisplay.ToStringPercent(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_26E:
				if (stage != null && stage.partEfficiencyOffset != 0f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_2D4:
				capModsToDisplay = null;
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
					i = 0;
					goto IL_503;
				}
				goto IL_519;
				IL_45F:
				if (capModsToDisplay[i].SetMaxDefined)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate() + " " + capModsToDisplay[i].setMax.ToStringPercent(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_4F5:
				i++;
				IL_503:
				if (i < capModsToDisplay.Count)
				{
					if (capModsToDisplay[i].offset != 0f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), (capModsToDisplay[i].offset * 100f).ToString("+#;-#") + "%", 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 5;
						}
						return true;
					}
					goto IL_3CE;
				}
				IL_519:
				if (stage == null)
				{
					goto IL_8E6;
				}
				if (stage.AffectsMemory || stage.AffectsSocialInteractions)
				{
					affectsSb = new StringBuilder();
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
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Affects".Translate(), affectsSb.ToString(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				goto IL_61C;
				IL_680:
				if (stage.hungerRateFactorOffset != 0f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				}
				IL_6EF:
				if (stage.restFallFactor != 1f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				}
				IL_753:
				if (stage.restFallFactorOffset != 0f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				}
				IL_7C2:
				if (stage.makeImmuneTo != null)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "PreventsInfection".Translate(), (from im in stage.makeImmuneTo
					select im.label).ToCommaList(false).CapitalizeFirst(), 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				}
				IL_83F:
				if (stage.statOffsets == null)
				{
					goto IL_8E6;
				}
				j = 0;
				IL_8CB:
				if (j < stage.statOffsets.Count)
				{
					sm = stage.statOffsets[j];
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, sm.stat.LabelCap, sm.ValueToStringAsOffset, 0, string.Empty);
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				}
				IL_8E6:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HediffStatsUtility.<SpecialDisplayStats>c__Iterator0 <SpecialDisplayStats>c__Iterator = new HediffStatsUtility.<SpecialDisplayStats>c__Iterator0();
				<SpecialDisplayStats>c__Iterator.instance = instance;
				<SpecialDisplayStats>c__Iterator.stage = stage;
				return <SpecialDisplayStats>c__Iterator;
			}

			private static string <>m__0(HediffDef im)
			{
				return im.label;
			}
		}
	}
}
