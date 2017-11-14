using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompTempControl : ThingComp
	{
		[Unsaved]
		public bool operatingAtHighPower;

		public float targetTemperature = -99999f;

		private const float DefaultTargetTemperature = 21f;

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
			using (IEnumerator<Gizmo> enumerator = base.CompGetGizmosExtra().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			_003CCompGetGizmosExtra_003Ec__Iterator0 _003CCompGetGizmosExtra_003Ec__Iterator = (_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_00d5: stateMachine*/;
			float offset = this.RoundedToCurrentTempModeOffset(-10f);
			yield return (Gizmo)new Command_Action
			{
				action = delegate
				{
					_003CCompGetGizmosExtra_003Ec__Iterator._0024this.InterfaceChangeTargetTemperature(offset);
				},
				defaultLabel = offset.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_04c3:
			/*Error near IL_04c4: Unexpected return in MoveNext()*/;
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
