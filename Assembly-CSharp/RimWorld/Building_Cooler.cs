using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_Cooler : Building_TempControl
	{
		private const float HeatOutputMultiplier = 1.25f;

		private const float EfficiencyLossPerDegreeDifference = 0.0076923077f;

		public override void TickRare()
		{
			if (base.compPowerTrader.PowerOn)
			{
				IntVec3 intVec = base.Position + IntVec3.South.RotatedBy(base.Rotation);
				IntVec3 intVec2 = base.Position + IntVec3.North.RotatedBy(base.Rotation);
				bool flag = false;
				if (!intVec2.Impassable(base.Map) && !intVec.Impassable(base.Map))
				{
					float temperature = intVec2.GetTemperature(base.Map);
					float temperature2 = intVec.GetTemperature(base.Map);
					float num = temperature - temperature2;
					if (temperature - 40.0 > num)
					{
						num = (float)(temperature - 40.0);
					}
					float num2 = (float)(1.0 - num * 0.0076923076994717121);
					if (num2 < 0.0)
					{
						num2 = 0f;
					}
					float num3 = (float)(base.compTempControl.Props.energyPerSecond * num2 * 4.1666665077209473);
					float num4 = GenTemperature.ControlTemperatureTempChange(intVec, base.Map, num3, base.compTempControl.targetTemperature);
					flag = !Mathf.Approximately(num4, 0f);
					if (flag)
					{
						intVec.GetRoomGroup(base.Map).Temperature += num4;
						GenTemperature.PushHeat(intVec2, base.Map, (float)((0.0 - num3) * 1.25));
					}
				}
				CompProperties_Power props = base.compPowerTrader.Props;
				if (flag)
				{
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
