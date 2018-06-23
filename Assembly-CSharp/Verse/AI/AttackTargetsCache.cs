using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000C27 RID: 3111
	public class AttackTargetsCache
	{
		// Token: 0x04002E71 RID: 11889
		private Map map;

		// Token: 0x04002E72 RID: 11890
		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		// Token: 0x04002E73 RID: 11891
		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		// Token: 0x04002E74 RID: 11892
		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		// Token: 0x04002E75 RID: 11893
		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		// Token: 0x04002E76 RID: 11894
		private static HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		// Token: 0x04002E77 RID: 11895
		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x04002E78 RID: 11896
		private static List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();

		// Token: 0x06004465 RID: 17509 RVA: 0x0023FB10 File Offset: 0x0023DF10
		public AttackTargetsCache(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004466 RID: 17510 RVA: 0x0023FB44 File Offset: 0x0023DF44
		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0023FB64 File Offset: 0x0023DF64
		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0023FB74 File Offset: 0x0023DF74
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

		// Token: 0x06004469 RID: 17513 RVA: 0x0023FBCC File Offset: 0x0023DFCC
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

		// Token: 0x0600446A RID: 17514 RVA: 0x0023FD70 File Offset: 0x0023E170
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

		// Token: 0x0600446B RID: 17515 RVA: 0x0023FDCC File Offset: 0x0023E1CC
		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0023FDF0 File Offset: 0x0023E1F0
		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0023FE14 File Offset: 0x0023E214
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

		// Token: 0x0600446E RID: 17518 RVA: 0x0023FEDC File Offset: 0x0023E2DC
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

		// Token: 0x0600446F RID: 17519 RVA: 0x00240064 File Offset: 0x0023E464
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

		// Token: 0x06004470 RID: 17520 RVA: 0x0024013C File Offset: 0x0023E53C
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

		// Token: 0x06004471 RID: 17521 RVA: 0x00240244 File Offset: 0x0023E644
		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x00240270 File Offset: 0x0023E670
		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}
	}
}
