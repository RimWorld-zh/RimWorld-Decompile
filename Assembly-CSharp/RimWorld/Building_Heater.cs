using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200069A RID: 1690
	public class Building_Heater : Building_TempControl
	{
		// Token: 0x060023CC RID: 9164 RVA: 0x00132AB0 File Offset: 0x00130EB0
		public override void TickRare()
		{
			if (this.compPowerTrader.PowerOn)
			{
				float ambientTemperature = base.AmbientTemperature;
				float num;
				if (ambientTemperature < 20f)
				{
					num = 1f;
				}
				else if (ambientTemperature > 120f)
				{
					num = 0f;
				}
				else
				{
					num = Mathf.InverseLerp(120f, 20f, ambientTemperature);
				}
				float energyLimit = this.compTempControl.Props.energyPerSecond * num * 4.16666651f;
				float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, this.compTempControl.targetTemperature);
				bool flag = !Mathf.Approximately(num2, 0f);
				CompProperties_Power props = this.compPowerTrader.Props;
				if (flag)
				{
					this.GetRoomGroup().Temperature += num2;
					this.compPowerTrader.PowerOutput = -props.basePowerConsumption;
				}
				else
				{
					this.compPowerTrader.PowerOutput = -props.basePowerConsumption * this.compTempControl.Props.lowPowerConsumptionFactor;
				}
				this.compTempControl.operatingAtHighPower = flag;
			}
		}

		// Token: 0x040013FC RID: 5116
		private const float EfficiencyFalloffSpan = 100f;
	}
}
