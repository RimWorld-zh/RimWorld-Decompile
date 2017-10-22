using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_Heater : Building_TempControl
	{
		private const float EfficiencyFalloffSpan = 100f;

		public override void TickRare()
		{
			if (base.compPowerTrader.PowerOn)
			{
				float ambientTemperature = base.AmbientTemperature;
				float num = (float)((!(ambientTemperature < 20.0)) ? ((!(ambientTemperature > 120.0)) ? Mathf.InverseLerp(120f, 20f, ambientTemperature) : 0.0) : 1.0);
				float energyLimit = (float)(base.compTempControl.Props.energyPerSecond * num * 4.1666665077209473);
				float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, base.compTempControl.targetTemperature);
				bool flag = !Mathf.Approximately(num2, 0f);
				CompProperties_Power props = base.compPowerTrader.Props;
				if (flag)
				{
					this.GetRoomGroup().Temperature += num2;
					base.compPowerTrader.PowerOutput = (float)(0.0 - props.basePowerConsumption);
				}
				else
				{
					base.compPowerTrader.PowerOutput = (float)((0.0 - props.basePowerConsumption) * base.compTempControl.Props.lowPowerConsumptionFactor);
				}
				base.compTempControl.operatingAtHighPower = flag;
			}
		}
	}
}
