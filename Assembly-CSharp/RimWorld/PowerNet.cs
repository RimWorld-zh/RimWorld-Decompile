#define ENABLE_PROFILER
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	public class PowerNet
	{
		public PowerNetManager powerNetManager;

		public bool hasPowerSource;

		public List<CompPower> connectors = new List<CompPower>();

		public List<CompPower> transmitters = new List<CompPower>();

		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		private float debugLastCreatedEnergy;

		private float debugLastRawStoredEnergy;

		private float debugLastApparentStoredEnergy;

		private const int MaxRestartTryInterval = 200;

		private const int MinRestartTryInterval = 30;

		private const float RestartMinFraction = 0.05f;

		private const int ShutdownInterval = 20;

		private const float ShutdownMinFraction = 0.05f;

		private const float MinStoredEnergyToTurnOn = 5f;

		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();

		public Map Map
		{
			get
			{
				return this.powerNetManager.map;
			}
		}

		public bool HasActivePowerSource
		{
			get
			{
				bool result;
				if (!this.hasPowerSource)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < this.transmitters.Count; i++)
					{
						if (this.IsActivePowerSource(this.transmitters[i]))
							goto IL_0032;
					}
					result = false;
				}
				goto IL_0056;
				IL_0032:
				result = true;
				goto IL_0056;
				IL_0056:
				return result;
			}
		}

		public PowerNet(IEnumerable<CompPower> newTransmitters)
		{
			foreach (CompPower item in newTransmitters)
			{
				this.transmitters.Add(item);
				item.transNet = this;
				this.RegisterAllComponentsOf(item.parent);
				if (item.connectChildren != null)
				{
					List<CompPower> connectChildren = item.connectChildren;
					for (int i = 0; i < connectChildren.Count; i++)
					{
						this.RegisterConnector(connectChildren[i]);
					}
				}
			}
			this.hasPowerSource = false;
			int num = 0;
			while (true)
			{
				if (num < this.transmitters.Count)
				{
					if (!this.IsPowerSource(this.transmitters[num]))
					{
						num++;
						continue;
					}
					break;
				}
				return;
			}
			this.hasPowerSource = true;
		}

		private bool IsPowerSource(CompPower cp)
		{
			return (byte)((cp is CompPowerBattery) ? 1 : ((cp is CompPowerTrader && cp.Props.basePowerConsumption < 0.0) ? 1 : 0)) != 0;
		}

		private bool IsActivePowerSource(CompPower cp)
		{
			CompPowerBattery compPowerBattery = cp as CompPowerBattery;
			bool result;
			if (compPowerBattery != null && compPowerBattery.StoredEnergy > 0.0)
			{
				result = true;
			}
			else
			{
				CompPowerTrader compPowerTrader = cp as CompPowerTrader;
				result = ((byte)((compPowerTrader != null && compPowerTrader.PowerOutput > 0.0) ? 1 : 0) != 0);
			}
			return result;
		}

		public void RegisterConnector(CompPower b)
		{
			if (this.connectors.Contains(b))
			{
				Log.Error("PowerNet registered connector it already had: " + b);
			}
			else
			{
				this.connectors.Add(b);
				this.RegisterAllComponentsOf(b.parent);
			}
		}

		public void DeregisterConnector(CompPower b)
		{
			this.connectors.Remove(b);
			this.DeregisterAllComponentsOf(b.parent);
		}

		private void RegisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				if (this.powerComps.Contains(comp))
				{
					Log.Error("PowerNet adding powerComp " + comp + " which it already has.");
				}
				else
				{
					this.powerComps.Add(comp);
				}
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				if (this.batteryComps.Contains(comp2))
				{
					Log.Error("PowerNet adding batteryComp " + comp2 + " which it already has.");
				}
				else
				{
					this.batteryComps.Add(comp2);
				}
			}
		}

		private void DeregisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				this.powerComps.Remove(comp);
			}
			CompPowerBattery comp2 = parentThing.GetComp<CompPowerBattery>();
			if (comp2 != null)
			{
				this.batteryComps.Remove(comp2);
			}
		}

		public float CurrentEnergyGainRate()
		{
			float result;
			if (DebugSettings.unlimitedPower)
			{
				result = 100000f;
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < this.powerComps.Count; i++)
				{
					if (this.powerComps[i].PowerOn)
					{
						num += this.powerComps[i].EnergyOutputPerTick;
					}
				}
				result = num;
			}
			return result;
		}

		public float CurrentStoredEnergy()
		{
			float num = 0f;
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				num += this.batteryComps[i].StoredEnergy;
			}
			return num;
		}

		public void PowerNetTick()
		{
			float num = this.CurrentEnergyGainRate();
			float num2 = this.CurrentStoredEnergy();
			if (num2 + num >= -1.0000000116860974E-07 && !this.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
			{
				Profiler.BeginSample("PowerNetTick Excess Energy");
				float num3 = (float)((this.batteryComps.Count <= 0 || !(num2 >= 0.10000000149011612)) ? num2 : (num2 - 5.0));
				if (UnityData.isDebugBuild)
				{
					this.debugLastApparentStoredEnergy = num3;
					this.debugLastCreatedEnergy = num;
					this.debugLastRawStoredEnergy = num2;
				}
				if (num3 + num >= 0.0)
				{
					PowerNet.partsWantingPowerOn.Clear();
					for (int i = 0; i < this.powerComps.Count; i++)
					{
						if (!this.powerComps[i].PowerOn && FlickUtility.WantsToBeOn(this.powerComps[i].parent) && !this.powerComps[i].parent.IsBrokenDown())
						{
							PowerNet.partsWantingPowerOn.Add(this.powerComps[i]);
						}
					}
					if (PowerNet.partsWantingPowerOn.Count > 0)
					{
						int num4 = 200 / PowerNet.partsWantingPowerOn.Count;
						if (num4 < 30)
						{
							num4 = 30;
						}
						if (Find.TickManager.TicksGame % num4 == 0)
						{
							int num5 = Mathf.Max(1, Mathf.RoundToInt((float)((float)PowerNet.partsWantingPowerOn.Count * 0.05000000074505806)));
							for (int num6 = 0; num6 < num5; num6++)
							{
								CompPowerTrader compPowerTrader = PowerNet.partsWantingPowerOn.RandomElement();
								if (!compPowerTrader.PowerOn && num + num2 >= 0.0 - (compPowerTrader.EnergyOutputPerTick + 1.0000000116860974E-07))
								{
									compPowerTrader.PowerOn = true;
									num += compPowerTrader.EnergyOutputPerTick;
								}
							}
						}
					}
				}
				this.ChangeStoredEnergy(num);
				Profiler.EndSample();
			}
			else
			{
				Profiler.BeginSample("PowerNetTick Shutdown");
				if (Find.TickManager.TicksGame % 20 == 0)
				{
					PowerNet.potentialShutdownParts.Clear();
					for (int j = 0; j < this.powerComps.Count; j++)
					{
						if (this.powerComps[j].PowerOn && this.powerComps[j].EnergyOutputPerTick < 0.0)
						{
							PowerNet.potentialShutdownParts.Add(this.powerComps[j]);
						}
					}
					if (PowerNet.potentialShutdownParts.Count > 0)
					{
						int num7 = Mathf.Max(1, Mathf.RoundToInt((float)((float)PowerNet.potentialShutdownParts.Count * 0.05000000074505806)));
						for (int num8 = 0; num8 < num7; num8++)
						{
							PowerNet.potentialShutdownParts.RandomElement().PowerOn = false;
						}
					}
				}
				Profiler.EndSample();
			}
		}

		private void ChangeStoredEnergy(float extra)
		{
			if (extra > 0.0)
			{
				this.DistributeEnergyAmongBatteries(extra);
			}
			else
			{
				float num = (float)(0.0 - extra);
				this.givingBats.Clear();
				for (int i = 0; i < this.batteryComps.Count; i++)
				{
					if (this.batteryComps[i].StoredEnergy > 1.0000000116860974E-07)
					{
						this.givingBats.Add(this.batteryComps[i]);
					}
				}
				float a = num / (float)this.givingBats.Count;
				int num2 = 0;
				while (num > 1.0000000116860974E-07)
				{
					int num3 = 0;
					while (num3 < this.givingBats.Count)
					{
						float num4 = Mathf.Min(a, this.givingBats[num3].StoredEnergy);
						this.givingBats[num3].DrawPower(num4);
						num -= num4;
						if (!(num < 1.0000000116860974E-07))
						{
							num3++;
							continue;
						}
						return;
					}
					num2++;
					if (num2 > 10)
						break;
				}
				if (num > 1.0000000116860974E-07)
				{
					Log.Warning("Drew energy from a PowerNet that didn't have it.");
				}
			}
		}

		private void DistributeEnergyAmongBatteries(float energy)
		{
			if (!(energy <= 0.0) && this.batteryComps.Any())
			{
				PowerNet.batteriesShuffled.Clear();
				PowerNet.batteriesShuffled.AddRange(this.batteryComps);
				PowerNet.batteriesShuffled.Shuffle();
				int num = 0;
				while (true)
				{
					num++;
					if (num > 10000)
					{
						Log.Error("Too many iterations.");
					}
					else
					{
						float num2 = 3.40282347E+38f;
						for (int i = 0; i < PowerNet.batteriesShuffled.Count; i++)
						{
							num2 = Mathf.Min(num2, PowerNet.batteriesShuffled[i].AmountCanAccept);
						}
						if (energy >= num2 * (float)PowerNet.batteriesShuffled.Count)
						{
							for (int num3 = PowerNet.batteriesShuffled.Count - 1; num3 >= 0; num3--)
							{
								float amountCanAccept = PowerNet.batteriesShuffled[num3].AmountCanAccept;
								bool flag = amountCanAccept <= 0.0 || amountCanAccept == num2;
								if (num2 > 0.0)
								{
									PowerNet.batteriesShuffled[num3].AddEnergy(num2);
									energy -= num2;
								}
								if (flag)
								{
									PowerNet.batteriesShuffled.RemoveAt(num3);
								}
							}
							if (energy < 0.00050000002374872565)
								break;
							if (!PowerNet.batteriesShuffled.Any())
								break;
							continue;
						}
						float amount = energy / (float)PowerNet.batteriesShuffled.Count;
						for (int j = 0; j < PowerNet.batteriesShuffled.Count; j++)
						{
							PowerNet.batteriesShuffled[j].AddEnergy(amount);
						}
						energy = 0f;
					}
					break;
				}
				PowerNet.batteriesShuffled.Clear();
			}
		}

		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("POWERNET:");
			stringBuilder.AppendLine("  Created energy: " + this.debugLastCreatedEnergy);
			stringBuilder.AppendLine("  Raw stored energy: " + this.debugLastRawStoredEnergy);
			stringBuilder.AppendLine("  Apparent stored energy: " + this.debugLastApparentStoredEnergy);
			stringBuilder.AppendLine("  hasPowerSource: " + this.hasPowerSource);
			stringBuilder.AppendLine("  Connectors: ");
			foreach (CompPower connector in this.connectors)
			{
				stringBuilder.AppendLine("      " + connector.parent);
			}
			stringBuilder.AppendLine("  Transmitters: ");
			foreach (CompPower transmitter in this.transmitters)
			{
				stringBuilder.AppendLine("      " + transmitter.parent);
			}
			stringBuilder.AppendLine("  powerComps: ");
			foreach (CompPowerTrader powerComp in this.powerComps)
			{
				stringBuilder.AppendLine("      " + powerComp.parent);
			}
			stringBuilder.AppendLine("  batteryComps: ");
			foreach (CompPowerBattery batteryComp in this.batteryComps)
			{
				stringBuilder.AppendLine("      " + batteryComp.parent);
			}
			return stringBuilder.ToString();
		}
	}
}
