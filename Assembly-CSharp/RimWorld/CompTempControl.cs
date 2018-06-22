using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200073B RID: 1851
	public class CompTempControl : ThingComp
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x0015D0FC File Offset: 0x0015B4FC
		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)this.props;
			}
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0015D11C File Offset: 0x0015B51C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.targetTemperature < -2000f)
			{
				this.targetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x0015D147 File Offset: 0x0015B547
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x0015D168 File Offset: 0x0015B568
		private float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			float num = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			num = (float)Mathf.RoundToInt(num);
			return GenTemperature.ConvertTemperatureOffset(num, Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x0015D1A0 File Offset: 0x0015B5A0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo c in this.<CompGetGizmosExtra>__BaseCallProxy0())
			{
				yield return c;
			}
			float offset2 = this.RoundedToCurrentTempModeOffset(-10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset2);
				},
				defaultLabel = offset2.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			float offset3 = this.RoundedToCurrentTempModeOffset(-1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset3);
				},
				defaultLabel = offset3.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			yield return new Command_Action
			{
				action = delegate()
				{
					this.targetTemperature = 21f;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					this.ThrowCurrentTemperatureText();
				},
				defaultLabel = "CommandResetTemp".Translate(),
				defaultDesc = "CommandResetTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true)
			};
			float offset4 = this.RoundedToCurrentTempModeOffset(1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset4);
				},
				defaultLabel = "+" + offset4.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			float offset = this.RoundedToCurrentTempModeOffset(10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset);
				},
				defaultLabel = "+" + offset.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			yield break;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x0015D1CC File Offset: 0x0015B5CC
		private void InterfaceChangeTargetTemperature(float offset)
		{
			if (offset > 0f)
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
			}
			else
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
			}
			this.targetTemperature += offset;
			this.targetTemperature = Mathf.Clamp(this.targetTemperature, -273.15f, 2000f);
			this.ThrowCurrentTemperatureText();
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0015D230 File Offset: 0x0015B630
		private void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(this.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), this.parent.Map, this.targetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0015D28C File Offset: 0x0015B68C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TargetTemperature".Translate() + ": ");
			stringBuilder.AppendLine(this.targetTemperature.ToStringTemperature("F0"));
			stringBuilder.Append("PowerConsumptionMode".Translate() + ": ");
			if (this.operatingAtHighPower)
			{
				stringBuilder.Append("PowerConsumptionHigh".Translate());
			}
			else
			{
				stringBuilder.Append("PowerConsumptionLow".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001665 RID: 5733
		[Unsaved]
		public bool operatingAtHighPower = false;

		// Token: 0x04001666 RID: 5734
		public float targetTemperature = -99999f;

		// Token: 0x04001667 RID: 5735
		private const float DefaultTargetTemperature = 21f;
	}
}
