using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x02000423 RID: 1059
	public class PowerNet
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x000A0014 File Offset: 0x0009E414
		public PowerNet(IEnumerable<CompPower> newTransmitters)
		{
			foreach (CompPower compPower in newTransmitters)
			{
				this.transmitters.Add(compPower);
				compPower.transNet = this;
				this.RegisterAllComponentsOf(compPower.parent);
				if (compPower.connectChildren != null)
				{
					List<CompPower> connectChildren = compPower.connectChildren;
					for (int i = 0; i < connectChildren.Count; i++)
					{
						this.RegisterConnector(connectChildren[i]);
					}
				}
			}
			this.hasPowerSource = false;
			for (int j = 0; j < this.transmitters.Count; j++)
			{
				if (this.IsPowerSource(this.transmitters[j]))
				{
					this.hasPowerSource = true;
					break;
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x000A014C File Offset: 0x0009E54C
		public Map Map
		{
			get
			{
				return this.powerNetManager.map;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x000A016C File Offset: 0x0009E56C
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
						{
							return true;
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000A01D0 File Offset: 0x0009E5D0
		private bool IsPowerSource(CompPower cp)
		{
			return cp is CompPowerBattery || (cp is CompPowerTrader && cp.Props.basePowerConsumption < 0f);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000A0220 File Offset: 0x0009E620
		private bool IsActivePowerSource(CompPower cp)
		{
			CompPowerBattery compPowerBattery = cp as CompPowerBattery;
			bool result;
			if (compPowerBattery != null && compPowerBattery.StoredEnergy > 0f)
			{
				result = true;
			}
			else
			{
				CompPowerTrader compPowerTrader = cp as CompPowerTrader;
				result = (compPowerTrader != null && compPowerTrader.PowerOutput > 0f);
			}
			return result;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000A0280 File Offset: 0x0009E680
		public void RegisterConnector(CompPower b)
		{
			if (this.connectors.Contains(b))
			{
				Log.Error("PowerNet registered connector it already had: " + b, false);
			}
			else
			{
				this.connectors.Add(b);
				this.RegisterAllComponentsOf(b.parent);
			}
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000A02CE File Offset: 0x0009E6CE
		public void DeregisterConnector(CompPower b)
		{
			this.connectors.Remove(b);
			this.DeregisterAllComponentsOf(b.parent);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000A02EC File Offset: 0x0009E6EC
		private void RegisterAllComponentsOf(ThingWithComps parentThing)
		{
			CompPowerTrader comp = parentThing.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				if (this.powerComps.Contains(comp))
				{
					Log.Error("PowerNet adding powerComp " + comp + " which it already has.", false);
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
					Log.Error("PowerNet adding batteryComp " + comp2 + " which it already has.", false);
				}
				else
				{
					this.batteryComps.Add(comp2);
				}
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x000A0388 File Offset: 0x0009E788
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

		// Token: 0x0600127E RID: 4734 RVA: 0x000A03CC File Offset: 0x0009E7CC
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

		// Token: 0x0600127F RID: 4735 RVA: 0x000A0448 File Offset: 0x0009E848
		public float CurrentStoredEnergy()
		{
			float num = 0f;
			for (int i = 0; i < this.batteryComps.Count; i++)
			{
				num += this.batteryComps[i].StoredEnergy;
			}
			return num;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x000A0498 File Offset: 0x0009E898
		public void PowerNetTick()
		{
			float num = this.CurrentEnergyGainRate();
			float num2 = this.CurrentStoredEnergy();
			if (num2 + num >= -1E-07f && !this.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
			{
				Profiler.BeginSample("PowerNetTick Excess Energy");
				float num3;
				if (this.batteryComps.Count > 0 && num2 >= 0.1f)
				{
					num3 = num2 - 5f;
				}
				else
				{
					num3 = num2;
				}
				if (UnityData.isDebugBuild)
				{
					this.debugLastApparentStoredEnergy = num3;
					this.debugLastCreatedEnergy = num;
					this.debugLastRawStoredEnergy = num2;
				}
				if (num3 + num >= 0f)
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
							int num5 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.partsWantingPowerOn.Count * 0.05f));
							for (int j = 0; j < num5; j++)
							{
								CompPowerTrader compPowerTrader = PowerNet.partsWantingPowerOn.RandomElement<CompPowerTrader>();
								if (!compPowerTrader.PowerOn)
								{
									if (num + num2 >= -(compPowerTrader.EnergyOutputPerTick + 1E-07f))
									{
										compPowerTrader.PowerOn = true;
										num += compPowerTrader.EnergyOutputPerTick;
									}
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
					for (int k = 0; k < this.powerComps.Count; k++)
					{
						if (this.powerComps[k].PowerOn && this.powerComps[k].EnergyOutputPerTick < 0f)
						{
							PowerNet.potentialShutdownParts.Add(this.powerComps[k]);
						}
					}
					if (PowerNet.potentialShutdownParts.Count > 0)
					{
						int num6 = Mathf.Max(1, Mathf.RoundToInt((float)PowerNet.potentialShutdownParts.Count * 0.05f));
						for (int l = 0; l < num6; l++)
						{
							PowerNet.potentialShutdownParts.RandomElement<CompPowerTrader>().PowerOn = false;
						}
					}
				}
				Profiler.EndSample();
			}
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x000A0794 File Offset: 0x0009EB94
		private void ChangeStoredEnergy(float extra)
		{
			if (extra > 0f)
			{
				this.DistributeEnergyAmongBatteries(extra);
			}
			else
			{
				float num = -extra;
				this.givingBats.Clear();
				for (int i = 0; i < this.batteryComps.Count; i++)
				{
					if (this.batteryComps[i].StoredEnergy > 1E-07f)
					{
						this.givingBats.Add(this.batteryComps[i]);
					}
				}
				float a = num / (float)this.givingBats.Count;
				int num2 = 0;
				while (num > 1E-07f)
				{
					for (int j = 0; j < this.givingBats.Count; j++)
					{
						float num3 = Mathf.Min(a, this.givingBats[j].StoredEnergy);
						this.givingBats[j].DrawPower(num3);
						num -= num3;
						if (num < 1E-07f)
						{
							return;
						}
					}
					num2++;
					if (num2 > 10)
					{
						break;
					}
				}
				if (num > 1E-07f)
				{
					Log.Warning("Drew energy from a PowerNet that didn't have it.", false);
				}
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x000A08C8 File Offset: 0x0009ECC8
		private void DistributeEnergyAmongBatteries(float energy)
		{
			if (energy > 0f && this.batteryComps.Any<CompPowerBattery>())
			{
				PowerNet.batteriesShuffled.Clear();
				PowerNet.batteriesShuffled.AddRange(this.batteryComps);
				PowerNet.batteriesShuffled.Shuffle<CompPowerBattery>();
				int num = 0;
				for (;;)
				{
					num++;
					if (num > 10000)
					{
						break;
					}
					float num2 = float.MaxValue;
					for (int i = 0; i < PowerNet.batteriesShuffled.Count; i++)
					{
						num2 = Mathf.Min(num2, PowerNet.batteriesShuffled[i].AmountCanAccept);
					}
					if (energy < num2 * (float)PowerNet.batteriesShuffled.Count)
					{
						goto IL_139;
					}
					for (int j = PowerNet.batteriesShuffled.Count - 1; j >= 0; j--)
					{
						float amountCanAccept = PowerNet.batteriesShuffled[j].AmountCanAccept;
						bool flag = amountCanAccept <= 0f || amountCanAccept == num2;
						if (num2 > 0f)
						{
							PowerNet.batteriesShuffled[j].AddEnergy(num2);
							energy -= num2;
						}
						if (flag)
						{
							PowerNet.batteriesShuffled.RemoveAt(j);
						}
					}
					if (energy < 0.0005f || !PowerNet.batteriesShuffled.Any<CompPowerBattery>())
					{
						goto IL_1A3;
					}
				}
				Log.Error("Too many iterations.", false);
				goto IL_1AE;
				IL_139:
				float amount = energy / (float)PowerNet.batteriesShuffled.Count;
				for (int k = 0; k < PowerNet.batteriesShuffled.Count; k++)
				{
					PowerNet.batteriesShuffled[k].AddEnergy(amount);
				}
				energy = 0f;
				IL_1A3:
				IL_1AE:
				PowerNet.batteriesShuffled.Clear();
			}
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x000A0A90 File Offset: 0x0009EE90
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("POWERNET:");
			stringBuilder.AppendLine("  Created energy: " + this.debugLastCreatedEnergy);
			stringBuilder.AppendLine("  Raw stored energy: " + this.debugLastRawStoredEnergy);
			stringBuilder.AppendLine("  Apparent stored energy: " + this.debugLastApparentStoredEnergy);
			stringBuilder.AppendLine("  hasPowerSource: " + this.hasPowerSource);
			stringBuilder.AppendLine("  Connectors: ");
			foreach (CompPower compPower in this.connectors)
			{
				stringBuilder.AppendLine("      " + compPower.parent);
			}
			stringBuilder.AppendLine("  Transmitters: ");
			foreach (CompPower compPower2 in this.transmitters)
			{
				stringBuilder.AppendLine("      " + compPower2.parent);
			}
			stringBuilder.AppendLine("  powerComps: ");
			foreach (CompPowerTrader compPowerTrader in this.powerComps)
			{
				stringBuilder.AppendLine("      " + compPowerTrader.parent);
			}
			stringBuilder.AppendLine("  batteryComps: ");
			foreach (CompPowerBattery compPowerBattery in this.batteryComps)
			{
				stringBuilder.AppendLine("      " + compPowerBattery.parent);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000B36 RID: 2870
		public PowerNetManager powerNetManager;

		// Token: 0x04000B37 RID: 2871
		public bool hasPowerSource;

		// Token: 0x04000B38 RID: 2872
		public List<CompPower> connectors = new List<CompPower>();

		// Token: 0x04000B39 RID: 2873
		public List<CompPower> transmitters = new List<CompPower>();

		// Token: 0x04000B3A RID: 2874
		public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

		// Token: 0x04000B3B RID: 2875
		public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

		// Token: 0x04000B3C RID: 2876
		private float debugLastCreatedEnergy;

		// Token: 0x04000B3D RID: 2877
		private float debugLastRawStoredEnergy;

		// Token: 0x04000B3E RID: 2878
		private float debugLastApparentStoredEnergy;

		// Token: 0x04000B3F RID: 2879
		private const int MaxRestartTryInterval = 200;

		// Token: 0x04000B40 RID: 2880
		private const int MinRestartTryInterval = 30;

		// Token: 0x04000B41 RID: 2881
		private const float RestartMinFraction = 0.05f;

		// Token: 0x04000B42 RID: 2882
		private const int ShutdownInterval = 20;

		// Token: 0x04000B43 RID: 2883
		private const float ShutdownMinFraction = 0.05f;

		// Token: 0x04000B44 RID: 2884
		private const float MinStoredEnergyToTurnOn = 5f;

		// Token: 0x04000B45 RID: 2885
		private static List<CompPowerTrader> partsWantingPowerOn = new List<CompPowerTrader>();

		// Token: 0x04000B46 RID: 2886
		private static List<CompPowerTrader> potentialShutdownParts = new List<CompPowerTrader>();

		// Token: 0x04000B47 RID: 2887
		private List<CompPowerBattery> givingBats = new List<CompPowerBattery>();

		// Token: 0x04000B48 RID: 2888
		private static List<CompPowerBattery> batteriesShuffled = new List<CompPowerBattery>();
	}
}
