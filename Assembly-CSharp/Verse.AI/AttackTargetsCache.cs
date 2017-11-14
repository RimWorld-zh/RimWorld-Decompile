using RimWorld;
using System.Collections.Generic;

namespace Verse.AI
{
	public class AttackTargetsCache
	{
		private Map map;

		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		private static HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		private static List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();

		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		public AttackTargetsCache(Map map)
		{
			this.map = map;
		}

		public void UpdateTarget(IAttackTarget t)
		{
			if (this.allTargets.Contains(t))
			{
				this.DeregisterTarget(t);
				Thing thing = t.Thing;
				if (thing.Spawned && thing.Map == this.map)
				{
					this.RegisterTarget(t);
				}
			}
		}

		public List<IAttackTarget> GetPotentialTargetsFor(IAttackTargetSearcher th)
		{
			Thing thing = th.Thing;
			AttackTargetsCache.targets.Clear();
			Faction faction = thing.Faction;
			if (faction != null)
			{
				if (UnityData.isDebugBuild)
				{
					this.Debug_AssertHostile(faction, this.TargetsHostileToFaction(faction));
				}
				foreach (IAttackTarget item in this.TargetsHostileToFaction(faction))
				{
					if (thing.HostileTo(item.Thing))
					{
						AttackTargetsCache.targets.Add(item);
					}
				}
			}
			foreach (Pawn item2 in this.pawnsInAggroMentalState)
			{
				if (thing.HostileTo(item2))
				{
					AttackTargetsCache.targets.Add(item2);
				}
			}
			Pawn pawn = th as Pawn;
			if (pawn != null && PrisonBreakUtility.IsPrisonBreaking(pawn))
			{
				Faction hostFaction = pawn.guest.HostFaction;
				List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(hostFaction);
				for (int i = 0; i < list.Count; i++)
				{
					if (thing.HostileTo(list[i]))
					{
						AttackTargetsCache.targets.Add(list[i]);
					}
				}
			}
			return AttackTargetsCache.targets;
		}

		public HashSet<IAttackTarget> TargetsHostileToFaction(Faction f)
		{
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.");
				return AttackTargetsCache.emptySet;
			}
			if (this.targetsHostileToFaction.ContainsKey(f))
			{
				return this.targetsHostileToFaction[f];
			}
			return AttackTargetsCache.emptySet;
		}

		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		public void Notify_FactionHostilityChanged(Faction f1, Faction f2)
		{
			AttackTargetsCache.tmpTargets.Clear();
			foreach (IAttackTarget allTarget in this.allTargets)
			{
				Thing thing = allTarget.Thing;
				if (thing.Faction == f1 || thing.Faction == f2)
				{
					AttackTargetsCache.tmpTargets.Add(allTarget);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpTargets.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpTargets[i]);
			}
			AttackTargetsCache.tmpTargets.Clear();
		}

		private void RegisterTarget(IAttackTarget target)
		{
			if (this.allTargets.Contains(target))
			{
				Log.Warning("Tried to register the same target twice " + target + " in " + base.GetType());
			}
			else
			{
				Thing thing = target.Thing;
				if (!thing.Spawned)
				{
					Log.Warning("Tried to register unspawned thing " + thing + " in " + base.GetType());
				}
				else if (thing.Map != this.map)
				{
					Log.Warning("Tried to register attack target " + thing + " but its Map is not this one.");
				}
				else
				{
					this.allTargets.Add(target);
					List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
					for (int i = 0; i < allFactionsListForReading.Count; i++)
					{
						if (thing.HostileTo(allFactionsListForReading[i]))
						{
							if (!this.targetsHostileToFaction.ContainsKey(allFactionsListForReading[i]))
							{
								this.targetsHostileToFaction.Add(allFactionsListForReading[i], new HashSet<IAttackTarget>());
							}
							this.targetsHostileToFaction[allFactionsListForReading[i]].Add(target);
						}
					}
					Pawn pawn = target as Pawn;
					if (pawn != null && pawn.InAggroMentalState)
					{
						this.pawnsInAggroMentalState.Add(pawn);
					}
				}
			}
		}

		private void DeregisterTarget(IAttackTarget target)
		{
			if (!this.allTargets.Contains(target))
			{
				Log.Warning("Tried to deregister " + target + " but it's not in " + base.GetType());
			}
			else
			{
				this.allTargets.Remove(target);
				foreach (KeyValuePair<Faction, HashSet<IAttackTarget>> item in this.targetsHostileToFaction)
				{
					HashSet<IAttackTarget> value = item.Value;
					value.Remove(target);
				}
				Pawn pawn = target as Pawn;
				if (pawn != null)
				{
					this.pawnsInAggroMentalState.Remove(pawn);
				}
			}
		}

		private void Debug_AssertHostile(Faction f, HashSet<IAttackTarget> targets)
		{
			AttackTargetsCache.tmpToUpdate.Clear();
			foreach (IAttackTarget target in targets)
			{
				if (!target.Thing.HostileTo(f))
				{
					AttackTargetsCache.tmpToUpdate.Add(target);
					Log.Error("Target " + ((target == null) ? "null" : target.ToString()) + " is not hostile to " + ((f == null) ? "null" : f.ToString()) + " (in " + base.GetType().Name + ") but it's in the list (forgot to update the target somewhere?). Trying to update the target...");
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}
	}
}
