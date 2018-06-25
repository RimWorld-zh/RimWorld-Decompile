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
			if (instance != null)
			{
				if (instance.Bleeding)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRate.ToStringPercent() + "/" + "LetterDay".Translate(), 0, "");
				}
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
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), (painOffsetToDisplay * 100f).ToString("+###0;-###0") + "%", 0, "");
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
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), "x" + painFactorToDisplay.ToStringPercent(), 0, "");
			}
			if (stage != null)
			{
				if (stage.partEfficiencyOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), 0, "");
				}
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
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), (capModsToDisplay[i].offset * 100f).ToString("+#;-#") + "%", 0, "");
					}
					if (capModsToDisplay[i].postFactor != 1f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capModsToDisplay[i].postFactor.ToStringPercent(), 0, "");
					}
					if (capModsToDisplay[i].SetMaxDefined)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate() + " " + capModsToDisplay[i].setMax.ToStringPercent(), 0, "");
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
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Affects".Translate(), affectsSb.ToString(), 0, "");
				}
				if (stage.hungerRateFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), 0, "");
				}
				if (stage.hungerRateFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), 0, "");
				}
				if (stage.restFallFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), 0, "");
				}
				if (stage.restFallFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), 0, "");
				}
				if (stage.makeImmuneTo != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PreventsInfection".Translate(), (from im in stage.makeImmuneTo
					select im.label).ToCommaList(false).CapitalizeFirst(), 0, "");
				}
				if (stage.statOffsets != null)
				{
					for (int j = 0; j < stage.statOffsets.Count; j++)
					{
						StatModifier sm = stage.statOffsets[j];
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, sm.stat.LabelCap, sm.ValueToStringAsOffset, 0, "");
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
					if (instance != null)
					{
						if (instance.Bleeding)
						{
							this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRate.ToStringPercent() + "/" + "LetterDay".Translate(), 0, "");
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1D5;
				case 3u:
					goto IL_27A;
				case 4u:
					goto IL_2E4;
				case 5u:
					goto IL_3E4;
				case 6u:
					goto IL_477;
				case 7u:
					goto IL_50F;
				case 8u:
					goto IL_641;
				case 9u:
					goto IL_6A8;
				case 10u:
					goto IL_719;
				case 11u:
					goto IL_77F;
				case 12u:
					goto IL_7F0;
				case 13u:
					goto IL_86F;
				case 14u:
					j++;
					goto IL_8FE;
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
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), (painOffsetToDisplay * 100f).ToString("+###0;-###0") + "%", 0, "");
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_1D5:
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
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), "x" + painFactorToDisplay.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_27A:
				if (stage != null)
				{
					if (stage.partEfficiencyOffset != 0f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), 0, "");
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
				}
				IL_2E4:
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
					goto IL_51E;
				}
				goto IL_535;
				IL_3E4:
				if (capModsToDisplay[i].postFactor != 1f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capModsToDisplay[i].postFactor.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_477:
				if (capModsToDisplay[i].SetMaxDefined)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate() + " " + capModsToDisplay[i].setMax.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_50F:
				i++;
				IL_51E:
				if (i < capModsToDisplay.Count)
				{
					if (capModsToDisplay[i].offset != 0f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), (capModsToDisplay[i].offset * 100f).ToString("+#;-#") + "%", 0, "");
						if (!this.$disposing)
						{
							this.$PC = 5;
						}
						return true;
					}
					goto IL_3E4;
				}
				IL_535:
				if (stage == null)
				{
					goto IL_91B;
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
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Affects".Translate(), affectsSb.ToString(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				IL_641:
				if (stage.hungerRateFactor != 1f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				}
				IL_6A8:
				if (stage.hungerRateFactorOffset != 0f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				}
				IL_719:
				if (stage.restFallFactor != 1f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				}
				IL_77F:
				if (stage.restFallFactorOffset != 0f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				}
				IL_7F0:
				if (stage.makeImmuneTo != null)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "PreventsInfection".Translate(), (from im in stage.makeImmuneTo
					select im.label).ToCommaList(false).CapitalizeFirst(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				}
				IL_86F:
				if (stage.statOffsets == null)
				{
					goto IL_91A;
				}
				j = 0;
				IL_8FE:
				if (j < stage.statOffsets.Count)
				{
					sm = stage.statOffsets[j];
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, sm.stat.LabelCap, sm.ValueToStringAsOffset, 0, "");
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				}
				IL_91A:
				IL_91B:
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
