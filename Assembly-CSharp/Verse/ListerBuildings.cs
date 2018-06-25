using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using Verse.AI;

namespace Verse
{
	public sealed class ListerBuildings
	{
		public List<Building> allBuildingsColonist = new List<Building>();

		public HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		public HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		public ListerBuildings()
		{
		}

		public void Add(Building b)
		{
			if (b.def.building == null || !b.def.building.isNaturalRock)
			{
				if (b.Faction == Faction.OfPlayer)
				{
					this.allBuildingsColonist.Add(b);
					if (b is IAttackTarget)
					{
						this.allBuildingsColonistCombatTargets.Add(b);
					}
				}
				CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
				if (compProperties != null && compProperties.shortCircuitInRain)
				{
					this.allBuildingsColonistElecFire.Add(b);
				}
			}
		}

		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
		}

		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool ColonistsHaveResearchBench()
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i] is Building_ResearchBench)
				{
					return true;
				}
			}
			return false;
		}

		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					CompPowerTrader compPowerTrader = this.allBuildingsColonist[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		public IEnumerable<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					yield return this.allBuildingsColonist[i];
				}
			}
			yield break;
		}

		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				T casted = this.allBuildingsColonist[i] as T;
				if (casted != null)
				{
					yield return casted;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <AllBuildingsColonistOfDef>c__Iterator0 : IEnumerable, IEnumerable<Building>, IEnumerator, IDisposable, IEnumerator<Building>
		{
			internal int <i>__1;

			internal ThingDef def;

			internal ListerBuildings $this;

			internal Building $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllBuildingsColonistOfDef>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_85:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.allBuildingsColonist.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (this.allBuildingsColonist[i].def == def)
					{
						this.$current = this.allBuildingsColonist[i];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_85;
				}
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
				return this.System.Collections.Generic.IEnumerable<Verse.Building>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Building> IEnumerable<Building>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ListerBuildings.<AllBuildingsColonistOfDef>c__Iterator0 <AllBuildingsColonistOfDef>c__Iterator = new ListerBuildings.<AllBuildingsColonistOfDef>c__Iterator0();
				<AllBuildingsColonistOfDef>c__Iterator.$this = this;
				<AllBuildingsColonistOfDef>c__Iterator.def = def;
				return <AllBuildingsColonistOfDef>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <AllBuildingsColonistOfClass>c__Iterator1<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T> where T : Building
		{
			internal int <i>__1;

			internal T <casted>__2;

			internal ListerBuildings $this;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllBuildingsColonistOfClass>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					IL_85:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.allBuildingsColonist.Count)
				{
					this.$PC = -1;
				}
				else
				{
					casted = (this.allBuildingsColonist[i] as T);
					if (casted != null)
					{
						this.$current = casted;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_85;
				}
				return false;
			}

			T IEnumerator<T>.Current
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
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ListerBuildings.<AllBuildingsColonistOfClass>c__Iterator1<T> <AllBuildingsColonistOfClass>c__Iterator = new ListerBuildings.<AllBuildingsColonistOfClass>c__Iterator1<T>();
				<AllBuildingsColonistOfClass>c__Iterator.$this = this;
				return <AllBuildingsColonistOfClass>c__Iterator;
			}
		}
	}
}
