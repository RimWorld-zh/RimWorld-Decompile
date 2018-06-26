using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ShortCircuitUtility
	{
		private static Dictionary<PowerNet, bool> tmpPowerNetHasActivePowerSource = new Dictionary<PowerNet, bool>();

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		[CompilerGenerated]
		private static Predicate<CompPowerBattery> <>f__am$cache0;

		public static IEnumerable<Building> GetShortCircuitablePowerConduits(Map map)
		{
			ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			try
			{
				List<Thing> conduits = map.listerThings.ThingsOfDef(ThingDefOf.PowerConduit);
				for (int i = 0; i < conduits.Count; i++)
				{
					Building b = (Building)conduits[i];
					CompPower power = b.PowerComp;
					if (power != null)
					{
						bool hasActivePowerSource;
						if (!ShortCircuitUtility.tmpPowerNetHasActivePowerSource.TryGetValue(power.PowerNet, out hasActivePowerSource))
						{
							hasActivePowerSource = power.PowerNet.HasActivePowerSource;
							ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Add(power.PowerNet, hasActivePowerSource);
						}
						if (hasActivePowerSource)
						{
							yield return b;
						}
					}
				}
			}
			finally
			{
				ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			}
			yield break;
		}

		public static void DoShortCircuit(Building culprit)
		{
			PowerNet powerNet = culprit.PowerComp.PowerNet;
			Map map = culprit.Map;
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			if (powerNet.batteryComps.Any((CompPowerBattery x) => x.StoredEnergy > 20f))
			{
				ShortCircuitUtility.DrainBatteriesAndCauseExplosion(powerNet, culprit, out num, out num2);
			}
			else
			{
				flag = ShortCircuitUtility.TryStartFireNear(culprit);
			}
			string text;
			if (culprit.def == ThingDefOf.PowerConduit)
			{
				text = "AnElectricalConduit".Translate();
			}
			else
			{
				text = Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(culprit.Label);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				stringBuilder.Append("ShortCircuitStartedFire".Translate(new object[]
				{
					text
				}));
			}
			else
			{
				stringBuilder.Append("ShortCircuit".Translate(new object[]
				{
					text
				}));
			}
			if (num > 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitDischargedEnergy".Translate(new object[]
				{
					num.ToString("F0")
				}));
			}
			if (num2 > 5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasLarge".Translate());
			}
			if (num2 > 8f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ShortCircuitWasHuge".Translate());
			}
			Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(culprit.Position, map, false), null, null);
		}

		public static bool TryShortCircuitInRain(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			bool result;
			if ((compPowerTrader != null && compPowerTrader.PowerOn && compPowerTrader.Props.shortCircuitInRain) || (thing.TryGetComp<CompPowerBattery>() != null && thing.TryGetComp<CompPowerBattery>().StoredEnergy > 100f))
			{
				string text = "ShortCircuitRain".Translate(new object[]
				{
					thing.Label
				});
				TargetInfo target = new TargetInfo(thing.Position, thing.Map, false);
				if (thing.Faction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), text, LetterDefOf.NegativeEvent, target, null, null);
				}
				else
				{
					Messages.Message(text, target, MessageTypeDefOf.NeutralEvent, true);
				}
				GenExplosion.DoExplosion(thing.OccupiedRect().RandomCell, thing.Map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private static void DrainBatteriesAndCauseExplosion(PowerNet net, Building culprit, out float totalEnergy, out float explosionRadius)
		{
			totalEnergy = 0f;
			for (int i = 0; i < net.batteryComps.Count; i++)
			{
				CompPowerBattery compPowerBattery = net.batteryComps[i];
				totalEnergy += compPowerBattery.StoredEnergy;
				compPowerBattery.DrawPower(compPowerBattery.StoredEnergy);
			}
			explosionRadius = Mathf.Sqrt(totalEnergy) * 0.05f;
			explosionRadius = Mathf.Clamp(explosionRadius, 1.5f, 14.9f);
			GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			if (explosionRadius > 3.5f)
			{
				GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius * 0.3f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
			}
		}

		private static bool TryStartFireNear(Building b)
		{
			ShortCircuitUtility.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(3f);
			CellRect startRect = b.OccupiedRect();
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = b.Position + GenRadial.RadialPattern[i];
				if (GenSight.LineOfSight(b.Position, intVec, b.Map, startRect, CellRect.SingleCell(intVec), null) && FireUtility.ChanceToStartFireIn(intVec, b.Map) > 0f)
				{
					ShortCircuitUtility.tmpCells.Add(intVec);
				}
			}
			return ShortCircuitUtility.tmpCells.Any<IntVec3>() && FireUtility.TryStartFireIn(ShortCircuitUtility.tmpCells.RandomElement<IntVec3>(), b.Map, Rand.Range(0.1f, 1.75f));
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ShortCircuitUtility()
		{
		}

		[CompilerGenerated]
		private static bool <DoShortCircuit>m__0(CompPowerBattery x)
		{
			return x.StoredEnergy > 20f;
		}

		[CompilerGenerated]
		private sealed class <GetShortCircuitablePowerConduits>c__Iterator0 : IEnumerable, IEnumerable<Building>, IEnumerator, IDisposable, IEnumerator<Building>
		{
			internal Map map;

			internal List<Thing> <conduits>__1;

			internal int <i>__2;

			internal Building <b>__3;

			internal CompPower <power>__3;

			internal bool <hasActivePowerSource>__3;

			internal Building $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetShortCircuitablePowerConduits>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						break;
					default:
						conduits = map.listerThings.ThingsOfDef(ThingDefOf.PowerConduit);
						i = 0;
						goto IL_137;
					}
					IL_129:
					i++;
					IL_137:
					if (i < conduits.Count)
					{
						b = (Building)conduits[i];
						power = b.PowerComp;
						if (power == null)
						{
							goto IL_129;
						}
						if (!ShortCircuitUtility.tmpPowerNetHasActivePowerSource.TryGetValue(power.PowerNet, out hasActivePowerSource))
						{
							hasActivePowerSource = power.PowerNet.HasActivePowerSource;
							ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Add(power.PowerNet, hasActivePowerSource);
						}
						if (!hasActivePowerSource)
						{
							goto IL_129;
						}
						this.$current = b;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			Building IEnumerator<Building>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Building>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Building> IEnumerable<Building>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ShortCircuitUtility.<GetShortCircuitablePowerConduits>c__Iterator0 <GetShortCircuitablePowerConduits>c__Iterator = new ShortCircuitUtility.<GetShortCircuitablePowerConduits>c__Iterator0();
				<GetShortCircuitablePowerConduits>c__Iterator.map = map;
				return <GetShortCircuitablePowerConduits>c__Iterator;
			}

			private void <>__Finally0()
			{
				ShortCircuitUtility.tmpPowerNetHasActivePowerSource.Clear();
			}
		}
	}
}
