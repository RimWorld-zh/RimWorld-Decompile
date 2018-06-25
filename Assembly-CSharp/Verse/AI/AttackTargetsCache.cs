using System;
using System.Collections.Generic;
using RimWorld;

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

		public AttackTargetsCache(Map map)
		{
			this.map = map;
		}

		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
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
				foreach (IAttackTarget attackTarget in this.TargetsHostileToFaction(faction))
				{
					if (thing.HostileTo(attackTarget.Thing))
					{
						AttackTargetsCache.targets.Add(attackTarget);
					}
				}
			}
			foreach (Pawn pawn in this.pawnsInAggroMentalState)
			{
				if (thing.HostileTo(pawn))
				{
					AttackTargetsCache.targets.Add(pawn);
				}
			}
			Pawn pawn2 = th as Pawn;
			if (pawn2 != null && PrisonBreakUtility.IsPrisonBreaking(pawn2))
			{
				Faction hostFaction = pawn2.guest.HostFaction;
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
			HashSet<IAttackTarget> result;
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.", false);
				result = AttackTargetsCache.emptySet;
			}
			else if (this.targetsHostileToFaction.ContainsKey(f))
			{
				result = this.targetsHostileToFaction[f];
			}
			else
			{
				result = AttackTargetsCache.emptySet;
			}
			return result;
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
			foreach (IAttackTarget attackTarget in this.allTargets)
			{
				Thing thing = attackTarget.Thing;
				if (thing.Faction == f1 || thing.Faction == f2)
				{
					AttackTargetsCache.tmpTargets.Add(attackTarget);
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
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register the same target twice ",
					target.ToStringSafe<IAttackTarget>(),
					" in ",
					base.GetType()
				}), false);
			}
			else
			{
				Thing thing = target.Thing;
				if (!thing.Spawned)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to register unspawned thing ",
						thing.ToStringSafe<Thing>(),
						" in ",
						base.GetType()
					}), false);
				}
				else if (thing.Map != this.map)
				{
					Log.Warning("Tried to register attack target " + thing.ToStringSafe<Thing>() + " but its Map is not this one.", false);
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
				Log.Warning(string.Concat(new object[]
				{
					"Tried to deregister ",
					target,
					" but it's not in ",
					base.GetType()
				}), false);
			}
			else
			{
				this.allTargets.Remove(target);
				foreach (KeyValuePair<Faction, HashSet<IAttackTarget>> keyValuePair in this.targetsHostileToFaction)
				{
					HashSet<IAttackTarget> value = keyValuePair.Value;
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
			foreach (IAttackTarget attackTarget in targets)
			{
				if (!attackTarget.Thing.HostileTo(f))
				{
					AttackTargetsCache.tmpToUpdate.Add(attackTarget);
					Log.Error(string.Concat(new string[]
					{
						"Target ",
						attackTarget.ToStringSafe<IAttackTarget>(),
						" is not hostile to ",
						f.ToStringSafe<Faction>(),
						" (in ",
						base.GetType().Name,
						") but it's in the list (forgot to update the target somewhere?). Trying to update the target..."
					}), false);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}

		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static AttackTargetsCache()
		{
		}
	}
}
