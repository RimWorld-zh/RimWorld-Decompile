using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200073D RID: 1853
	public class CompTempControl : ThingComp
	{
		// Token: 0x04001669 RID: 5737
		[Unsaved]
		public bool operatingAtHighPower = false;

		// Token: 0x0400166A RID: 5738
		public float targetTemperature = -99999f;

		// Token: 0x0400166B RID: 5739
		private const float DefaultTargetTemperature = 21f;

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060028EF RID: 10479 RVA: 0x0015D4AC File Offset: 0x0015B8AC
		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)this.props;
			}
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x0015D4CC File Offset: 0x0015B8CC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.targetTemperature < -2000f)
			{
				this.targetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x0015D4F7 File Offset: 0x0015B8F7
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x0015D518 File Offset: 0x0015B918
		private float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			float num = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			num = (float)Mathf.RoundToInt(num);
			return GenTemperature.ConvertTemperatureOffset(num, Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0015D550 File Offset: 0x0015B950
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

		// Token: 0x060028F4 RID: 10484 RVA: 0x0015D57C File Offset: 0x0015B97C
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

		// Token: 0x060028F5 RID: 10485 RVA: 0x0015D5E0 File Offset: 0x0015B9E0
		private void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(this.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), this.parent.Map, this.targetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x0015D63C File Offset: 0x0015BA3C
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
	}
}
