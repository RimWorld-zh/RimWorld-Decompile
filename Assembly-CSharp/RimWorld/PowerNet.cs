using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PowerNet
	{
		private const int MaxRestartTryInterval = 200;

		private const int MinRestartTryInterval = 30;

		private const int ShutdownInterval = 20;

		private const float MinStoredEnergyToTurnOn = 5f;

		public PowerNetManager powerNetManager;

		public bool hasPowerSource;

		public List<CompPower> connectors = new List<CompPower>();

		public List<CompPower> transmitters = new List<CompPower>();

		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		private float debugLastCreatedEnergy;

		private float debugLastRawStoredEnergy;

		private float debugLastApparentStoredEnergy;

		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();

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
			if (cp is CompPowerBattery)
			{
				return true;
			}
			if (cp is CompPowerTrader && cp.Props.basePowerConsumption < 0.0)
			{
				return true;
			}
			return false;
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
			if (DebugSettings.unlimitedPower)
			{
				return 100000f;
			}
			float num = 0f;
			for (int i = 0; i < this.powerComps.Count; i++)
			{
				if (this.powerComps[i].PowerOn)
				{
					num += this.powerComps[i].EnergyOutputPerTick;
				}
			}
			return num;
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
			if (num2 + num >= -1.0000000116860974E-07 && !this.powerNetManager.map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
			{
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
							CompPowerTrader compPowerTrader = PowerNet.partsWantingPowerOn.RandomElement();
							if (num + num2 >= 0.0 - (compPowerTrader.EnergyOutputPerTick + 1.0000000116860974E-07))
							{
								compPowerTrader.PowerOn = true;
								num += compPowerTrader.EnergyOutputPerTick;
							}
						}
					}
				}
				this.ChangeStoredEnergy(num);
			}
			else if (Find.TickManager.TicksGame % 20 == 0)
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
					PowerNet.potentialShutdownParts.RandomElement().PowerOn = false;
				}
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
			List<CompPower>.Enumerator enumerator = this.connectors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CompPower current = enumerator.Current;
					stringBuilder.AppendLine("      " + current.parent);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			stringBuilder.AppendLine("  Transmitters: ");
			List<CompPower>.Enumerator enumerator2 = this.transmitters.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					CompPower current2 = enumerator2.Current;
					stringBuilder.AppendLine("      " + current2.parent);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			stringBuilder.AppendLine("  powerComps: ");
			List<CompPowerTrader>.Enumerator enumerator3 = this.powerComps.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					CompPowerTrader current3 = enumerator3.Current;
					stringBuilder.AppendLine("      " + current3.parent);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator3).Dispose();
			}
			stringBuilder.AppendLine("  batteryComps: ");
			List<CompPowerBattery>.Enumerator enumerator4 = this.batteryComps.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					CompPowerBattery current4 = enumerator4.Current;
					stringBuilder.AppendLine("      " + current4.parent);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator4).Dispose();
			}
			return stringBuilder.ToString();
		}
	}
}
