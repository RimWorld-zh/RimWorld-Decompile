using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009CD RID: 2509
	public class TargetingParameters
	{
		// Token: 0x0600382F RID: 14383 RVA: 0x001DF024 File Offset: 0x001DD424
		public bool CanTarget(TargetInfo targ)
		{
			bool result;
			if (this.validator != null && !this.validator(targ))
			{
				result = false;
			}
			else if (targ.Thing == null)
			{
				result = this.canTargetLocations;
			}
			else if (this.neverTargetDoors && targ.Thing.def.IsDoor)
			{
				result = false;
			}
			else if (this.onlyTargetDamagedThings && targ.Thing.HitPoints == targ.Thing.MaxHitPoints)
			{
				result = false;
			}
			else if (this.onlyTargetFlammables && !targ.Thing.FlammableNow)
			{
				result = false;
			}
			else if (this.mustBeSelectable && !ThingSelectionUtility.SelectableByMapClick(targ.Thing))
			{
				result = false;
			}
			else if (this.targetSpecificThing != null && targ.Thing == this.targetSpecificThing)
			{
				result = true;
			}
			else if (this.canTargetFires && targ.Thing.def == ThingDefOf.Fire)
			{
				result = true;
			}
			else if (this.canTargetPawns && targ.Thing.def.category == ThingCategory.Pawn)
			{
				if (((Pawn)targ.Thing).Downed)
				{
					if (this.neverTargetIncapacitated)
					{
						return false;
					}
				}
				else if (this.onlyTargetIncapacitatedPawns)
				{
					return false;
				}
				result = (this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction));
			}
			else if (this.canTargetBuildings && targ.Thing.def.category == ThingCategory.Building)
			{
				result = ((!this.onlyTargetThingsAffectingRegions || targ.Thing.def.AffectsRegions) && (this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction)));
			}
			else
			{
				result = (this.canTargetItems && (!this.mapObjectTargetsMustBeAutoAttackable || targ.Thing.def.isAutoAttackableMapObject));
			}
			return result;
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x001DF2B4 File Offset: 0x001DD6B4
		public static TargetingParameters ForSelf(Pawn p)
		{
			return new TargetingParameters
			{
				targetSpecificThing = p,
				canTargetPawns = false,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x001DF2EC File Offset: 0x001DD6EC
		public static TargetingParameters ForArrest(Pawn arrester)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					bool result;
					if (!targ.HasThing)
					{
						result = false;
					}
					else
					{
						Pawn pawn = targ.Thing as Pawn;
						result = (pawn != null && pawn != arrester && pawn.CanBeArrestedBy(arrester) && !pawn.Downed);
					}
					return result;
				}
			};
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x001DF33C File Offset: 0x001DD73C
		public static TargetingParameters ForAttackHostile()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = true;
			targetingParameters.validator = delegate(TargetInfo targ)
			{
				bool result;
				if (!targ.HasThing)
				{
					result = false;
				}
				else if (targ.Thing.HostileTo(Faction.OfPlayer))
				{
					result = true;
				}
				else
				{
					Pawn pawn = targ.Thing as Pawn;
					result = (pawn != null && pawn.NonHumanlikeOrWildMan());
				}
				return result;
			};
			return targetingParameters;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x001DF398 File Offset: 0x001DD798
		public static TargetingParameters ForAttackAny()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = true
			};
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x001DF3D0 File Offset: 0x001DD7D0
		public static TargetingParameters ForRescue(Pawn p)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				onlyTargetIncapacitatedPawns = true,
				canTargetBuildings = false,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x001DF408 File Offset: 0x001DD808
		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = ((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x001DF45C File Offset: 0x001DD85C
		public static TargetingParameters ForTrade()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				ITrader trader = x.Thing as ITrader;
				return trader != null && trader.CanTradeNow;
			};
			return targetingParameters;
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x001DF4B0 File Offset: 0x001DD8B0
		public static TargetingParameters ForDropPodsDestination()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = ((TargetInfo x) => DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true));
			return targetingParameters;
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x001DF51C File Offset: 0x001DD91C
		public static TargetingParameters ForQuestPawnsWhoWillJoinColony(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = delegate(TargetInfo x)
			{
				Pawn pawn = x.Thing as Pawn;
				return pawn != null && !pawn.Dead && pawn.mindState.willJoinColonyIfRescued;
			};
			return targetingParameters;
		}

		// Token: 0x040023E2 RID: 9186
		public bool canTargetLocations = false;

		// Token: 0x040023E3 RID: 9187
		public bool canTargetSelf = false;

		// Token: 0x040023E4 RID: 9188
		public bool canTargetPawns = true;

		// Token: 0x040023E5 RID: 9189
		public bool canTargetFires = false;

		// Token: 0x040023E6 RID: 9190
		public bool canTargetBuildings = true;

		// Token: 0x040023E7 RID: 9191
		public bool canTargetItems = false;

		// Token: 0x040023E8 RID: 9192
		public List<Faction> onlyTargetFactions = null;

		// Token: 0x040023E9 RID: 9193
		public Predicate<TargetInfo> validator = null;

		// Token: 0x040023EA RID: 9194
		public bool onlyTargetFlammables = false;

		// Token: 0x040023EB RID: 9195
		public Thing targetSpecificThing = null;

		// Token: 0x040023EC RID: 9196
		public bool mustBeSelectable = false;

		// Token: 0x040023ED RID: 9197
		public bool neverTargetDoors = false;

		// Token: 0x040023EE RID: 9198
		public bool neverTargetIncapacitated = false;

		// Token: 0x040023EF RID: 9199
		public bool onlyTargetThingsAffectingRegions = false;

		// Token: 0x040023F0 RID: 9200
		public bool onlyTargetDamagedThings = false;

		// Token: 0x040023F1 RID: 9201
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		// Token: 0x040023F2 RID: 9202
		public bool onlyTargetIncapacitatedPawns = false;
	}
}
