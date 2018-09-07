using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Building_Trap : Building
	{
		private bool autoRearm;

		private List<Pawn> touchingPawns = new List<Pawn>();

		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		private const float KnowerSpringChanceFactorOther = 0f;

		private const ushort KnowerPathFindCost = 800;

		private const ushort KnowerPathWalkCost = 40;

		protected Building_Trap()
		{
		}

		private bool CanSetAutoRearm
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, new object[0]);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.autoRearm = (this.CanSetAutoRearm && map.areaManager.Home[base.Position]);
		}

		public override void Tick()
		{
			if (base.Spawned)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && !this.touchingPawns.Contains(pawn))
					{
						this.touchingPawns.Add(pawn);
						this.CheckSpring(pawn);
					}
				}
				for (int j = 0; j < this.touchingPawns.Count; j++)
				{
					Pawn pawn2 = this.touchingPawns[j];
					if (!pawn2.Spawned || pawn2.Position != base.Position)
					{
						this.touchingPawns.Remove(pawn2);
					}
				}
			}
			base.Tick();
		}

		private void CheckSpring(Pawn p)
		{
			if (Rand.Chance(this.SpringChance(p)))
			{
				this.Spring(p);
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(new object[]
					{
						p.LabelShort
					}), "LetterFriendlyTrapSprung".Translate(new object[]
					{
						p.LabelShort
					}), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, base.Map, false), null, null);
				}
			}
		}

		protected virtual float SpringChance(Pawn p)
		{
			float num = 1f;
			if (this.KnowsOfTrap(p))
			{
				if (p.Faction == null)
				{
					if (p.RaceProps.Animal)
					{
						num = 0.2f;
						num *= this.def.building.trapPeacefulWildAnimalsSpringChanceFactor;
					}
					else
					{
						num = 0.3f;
					}
				}
				else if (p.Faction == base.Faction)
				{
					num = 0.005f;
				}
				else
				{
					num = 0f;
				}
			}
			num *= this.GetStatValue(StatDefOf.TrapSpringChance, true);
			return Mathf.Clamp01(num);
		}

		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction) || (p.RaceProps.Humanlike && p.IsFormingCaravan()) || (p.IsPrisoner && p.guest.ShouldWaitInsteadOfEscaping && base.Faction == p.HostFaction);
		}

		public override ushort PathFindCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 800;
		}

		public override ushort PathWalkCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 40;
		}

		public override bool IsDangerousFor(Pawn p)
		{
			return this.KnowsOfTrap(p);
		}

		public void Spring(Pawn p)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			this.SpringSub(p);
			if (this.def.building.trapDestroyOnSpring)
			{
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				if (spawned)
				{
					this.CheckAutoRebuild(map);
				}
			}
		}

		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Kill(dinfo, exactCulprit);
			if (spawned)
			{
				this.CheckAutoRebuild(map);
			}
		}

		protected abstract void SpringSub(Pawn p);

		private void CheckAutoRebuild(Map map)
		{
			if (this.autoRearm && this.CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in base.GetGizmos())
			{
				yield return g;
			}
			if (this.CanSetAutoRearm)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAutoRearm".Translate(),
					defaultDesc = "CommandAutoRearmDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.RearmTrap,
					isActive = (() => this.autoRearm),
					toggleAction = delegate()
					{
						this.autoRearm = !this.autoRearm;
					}
				};
			}
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Toggle <com>__2;

			internal Building_Trap $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
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
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_167;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (!base.CanSetAutoRearm)
				{
					goto IL_167;
				}
				Command_Toggle com = new Command_Toggle();
				com.defaultLabel = "CommandAutoRearm".Translate();
				com.defaultDesc = "CommandAutoRearmDesc".Translate();
				com.hotKey = KeyBindingDefOf.Misc3;
				com.icon = TexCommand.RearmTrap;
				com.isActive = (() => this.autoRearm);
				com.toggleAction = delegate()
				{
					this.autoRearm = !this.autoRearm;
				};
				this.$current = com;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_167:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Building_Trap.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Building_Trap.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal bool <>m__0()
			{
				return this.autoRearm;
			}

			internal void <>m__1()
			{
				this.autoRearm = !this.autoRearm;
			}
		}
	}
}
