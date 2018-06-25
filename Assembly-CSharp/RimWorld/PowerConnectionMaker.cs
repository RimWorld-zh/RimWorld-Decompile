using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class PowerConnectionMaker
	{
		private const int ConnectMaxDist = 6;

		public static void ConnectAllConnectorsToTransmitter(CompPower newTransmitter)
		{
			foreach (CompPower compPower in PowerConnectionMaker.PotentialConnectorsForTransmitter(newTransmitter))
			{
				if (compPower.connectParent == null)
				{
					compPower.ConnectToTransmitter(newTransmitter, false);
				}
			}
		}

		public static void DisconnectAllFromTransmitterAndSetWantConnect(CompPower deadPc, Map map)
		{
			if (deadPc.connectChildren != null)
			{
				for (int i = 0; i < deadPc.connectChildren.Count; i++)
				{
					CompPower compPower = deadPc.connectChildren[i];
					compPower.connectParent = null;
					CompPowerTrader compPowerTrader = compPower as CompPowerTrader;
					if (compPowerTrader != null)
					{
						compPowerTrader.PowerOn = false;
					}
					map.powerNetManager.Notify_ConnectorWantsConnect(compPower);
				}
			}
		}

		public static void TryConnectToAnyPowerNet(CompPower pc, List<PowerNet> disallowedNets = null)
		{
			if (pc.connectParent == null)
			{
				if (pc.parent.Spawned)
				{
					CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(pc.parent.Position, pc.parent.Map, disallowedNets);
					if (compPower != null)
					{
						pc.ConnectToTransmitter(compPower, false);
					}
					else
					{
						pc.connectParent = null;
					}
				}
			}
		}

		public static void DisconnectFromPowerNet(CompPower pc)
		{
			if (pc.connectParent != null)
			{
				if (pc.PowerNet != null)
				{
					pc.PowerNet.DeregisterConnector(pc);
				}
				if (pc.connectParent.connectChildren != null)
				{
					pc.connectParent.connectChildren.Remove(pc);
					if (pc.connectParent.connectChildren.Count == 0)
					{
						pc.connectParent.connectChildren = null;
					}
				}
				pc.connectParent = null;
			}
		}

		private static IEnumerable<CompPower> PotentialConnectorsForTransmitter(CompPower b)
		{
			if (!b.parent.Spawned)
			{
				Log.Warning("Can't check potential connectors for " + b + " because it's unspawned.", false);
				yield break;
			}
			CellRect rect = b.parent.OccupiedRect().ExpandedBy(6).ClipInsideMap(b.parent.Map);
			for (int z = rect.minZ; z <= rect.maxZ; z++)
			{
				for (int x = rect.minX; x <= rect.maxX; x++)
				{
					IntVec3 c = new IntVec3(x, 0, z);
					List<Thing> thingList = b.parent.Map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def.ConnectToPower)
						{
							yield return ((Building)thingList[i]).PowerComp;
						}
					}
				}
			}
			yield break;
		}

		public static CompPower BestTransmitterForConnector(IntVec3 connectorPos, Map map, List<PowerNet> disallowedNets = null)
		{
			CellRect cellRect = CellRect.SingleCell(connectorPos).ExpandedBy(6).ClipInsideMap(map);
			cellRect.ClipInsideMap(map);
			float num = 999999f;
			CompPower result = null;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Building transmitter = c.GetTransmitter(map);
					if (transmitter != null && !transmitter.Destroyed)
					{
						CompPower powerComp = transmitter.PowerComp;
						if (powerComp != null && powerComp.TransmitsPowerNow && (transmitter.def.building == null || transmitter.def.building.allowWireConnection))
						{
							if (disallowedNets == null || !disallowedNets.Contains(powerComp.transNet))
							{
								float num2 = (float)(transmitter.Position - connectorPos).LengthHorizontalSquared;
								if (num2 < num)
								{
									num = num2;
									result = powerComp;
								}
							}
						}
					}
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <PotentialConnectorsForTransmitter>c__Iterator0 : IEnumerable, IEnumerable<CompPower>, IEnumerator, IDisposable, IEnumerator<CompPower>
		{
			internal CompPower b;

			internal CellRect <rect>__0;

			internal int <z>__1;

			internal int <x>__2;

			internal IntVec3 <c>__3;

			internal List<Thing> <thingList>__3;

			internal int <j>__4;

			internal CompPower $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PotentialConnectorsForTransmitter>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (!b.parent.Spawned)
					{
						Log.Warning("Can't check potential connectors for " + b + " because it's unspawned.", false);
						return false;
					}
					rect = b.parent.OccupiedRect().ExpandedBy(6).ClipInsideMap(b.parent.Map);
					z = rect.minZ;
					goto IL_1B8;
				case 1u:
					break;
				default:
					return false;
				}
				IL_15F:
				i++;
				IL_16E:
				if (i >= thingList.Count)
				{
					x++;
				}
				else
				{
					if (thingList[i].def.ConnectToPower)
					{
						this.$current = ((Building)thingList[i]).PowerComp;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_15F;
				}
				IL_193:
				if (x <= rect.maxX)
				{
					c = new IntVec3(x, 0, z);
					thingList = b.parent.Map.thingGrid.ThingsListAt(c);
					i = 0;
					goto IL_16E;
				}
				z++;
				IL_1B8:
				if (z <= rect.maxZ)
				{
					x = rect.minX;
					goto IL_193;
				}
				this.$PC = -1;
				return false;
			}

			CompPower IEnumerator<CompPower>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.CompPower>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<CompPower> IEnumerable<CompPower>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PowerConnectionMaker.<PotentialConnectorsForTransmitter>c__Iterator0 <PotentialConnectorsForTransmitter>c__Iterator = new PowerConnectionMaker.<PotentialConnectorsForTransmitter>c__Iterator0();
				<PotentialConnectorsForTransmitter>c__Iterator.b = b;
				return <PotentialConnectorsForTransmitter>c__Iterator;
			}
		}
	}
}
