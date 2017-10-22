using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompTempControl : ThingComp
	{
		private const float DefaultTargetTemperature = 21f;

		[Unsaved]
		public bool operatingAtHighPower;

		public float targetTemperature = -99999f;

		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)base.props;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.targetTemperature < -2000.0)
			{
				this.targetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		private float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			float f = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			f = (float)Mathf.RoundToInt(f);
			return GenTemperature.ConvertTemperatureOffset(f, Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo item in base.CompGetGizmosExtra())
			{
				yield return item;
			}
			float offset4 = this.RoundedToCurrentTempModeOffset(-10f);
			yield return (Gizmo)new Command_Action
			{
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_00e1: stateMachine*/)._003C_003Ef__this.InterfaceChangeTargetTemperature(((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_00e1: stateMachine*/)._003Coffset_003E__2);
				},
				defaultLabel = offset4.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			float offset3 = this.RoundedToCurrentTempModeOffset(-1f);
			yield return (Gizmo)new Command_Action
			{
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_0187: stateMachine*/)._003C_003Ef__this.InterfaceChangeTargetTemperature(((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_0187: stateMachine*/)._003Coffset_003E__4);
				},
				defaultLabel = offset3.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			yield return (Gizmo)new Command_Action
			{
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_0217: stateMachine*/)._003C_003Ef__this.targetTemperature = 21f;
					SoundDefOf.TickTiny.PlayOneShotOnCamera(null);
					((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_0217: stateMachine*/)._003C_003Ef__this.ThrowCurrentTemperatureText();
				},
				defaultLabel = "CommandResetTemp".Translate(),
				defaultDesc = "CommandResetTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true)
			};
			float offset2 = this.RoundedToCurrentTempModeOffset(1f);
			yield return (Gizmo)new Command_Action
			{
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_02b7: stateMachine*/)._003C_003Ef__this.InterfaceChangeTargetTemperature(((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_02b7: stateMachine*/)._003Coffset_003E__7);
				},
				defaultLabel = "+" + offset2.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			float offset = this.RoundedToCurrentTempModeOffset(10f);
			yield return (Gizmo)new Command_Action
			{
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_0367: stateMachine*/)._003C_003Ef__this.InterfaceChangeTargetTemperature(((_003CCompGetGizmosExtra_003Ec__Iterator16D)/*Error near IL_0367: stateMachine*/)._003Coffset_003E__9);
				},
				defaultLabel = "+" + offset.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
		}

		private void InterfaceChangeTargetTemperature(float offset)
		{
			if (offset > 0.0)
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
			}
			else
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
			}
			this.targetTemperature += offset;
			this.targetTemperature = Mathf.Clamp(this.targetTemperature, -270f, 2000f);
			this.ThrowCurrentTemperatureText();
		}

		private void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(base.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), base.parent.Map, this.targetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

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
