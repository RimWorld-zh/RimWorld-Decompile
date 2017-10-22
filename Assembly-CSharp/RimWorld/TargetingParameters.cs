using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class TargetingParameters
	{
		public bool canTargetLocations = false;

		public bool canTargetSelf = false;

		public bool canTargetPawns = true;

		public bool canTargetFires = false;

		public bool canTargetBuildings = true;

		public bool canTargetItems = false;

		public List<Faction> onlyTargetFactions = null;

		public Predicate<TargetInfo> validator = null;

		public bool onlyTargetFlammables = false;

		public Thing targetSpecificThing = null;

		public bool mustBeSelectable = false;

		public bool neverTargetDoors = false;

		public bool neverTargetIncapacitated = false;

		public bool onlyTargetThingsAffectingRegions = false;

		public bool onlyTargetDamagedThings = false;

		public bool mapObjectTargetsMustBeAutoAttackable = true;

		public bool onlyTargetIncapacitatedPawns = false;

		public bool CanTarget(TargetInfo targ)
		{
			bool result;
			if ((object)this.validator != null && !this.validator(targ))
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
						result = false;
						goto IL_0282;
					}
				}
				else if (this.onlyTargetIncapacitatedPawns)
				{
					result = false;
					goto IL_0282;
				}
				result = ((byte)((this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction)) ? 1 : 0) != 0);
			}
			else
			{
				result = ((byte)((!this.canTargetBuildings || targ.Thing.def.category != ThingCategory.Building) ? (this.canTargetItems ? ((!this.mapObjectTargetsMustBeAutoAttackable || targ.Thing.def.isAutoAttackableMapObject) ? 1 : 0) : 0) : ((!this.onlyTargetThingsAffectingRegions || targ.Thing.def.AffectsRegions) ? ((this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction)) ? 1 : 0) : 0)) != 0);
			}
			goto IL_0282;
			IL_0282:
			return result;
		}

		public static TargetingParameters ForSelf(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.targetSpecificThing = p;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			return targetingParameters;
		}

		public static TargetingParameters ForArrest(Pawn arrester)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = (Predicate<TargetInfo>)delegate(TargetInfo targ)
			{
				bool result;
				if (!targ.HasThing)
				{
					result = false;
				}
				else
				{
					Pawn pawn = targ.Thing as Pawn;
					result = ((byte)((pawn != null && pawn != arrester && pawn.CanBeArrestedBy(arrester)) ? ((!pawn.Downed) ? 1 : 0) : 0) != 0);
				}
				return result;
			};
			return targetingParameters;
		}

		public static TargetingParameters ForAttackHostile()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = true;
			targetingParameters.validator = (Predicate<TargetInfo>)delegate(TargetInfo targ)
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
					result = ((byte)((pawn != null && pawn.NonHumanlikeOrWildMan()) ? 1 : 0) != 0);
				}
				return result;
			};
			return targetingParameters;
		}

		public static TargetingParameters ForAttackAny()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = true;
			return targetingParameters;
		}

		public static TargetingParameters ForRescue(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.onlyTargetIncapacitatedPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			return targetingParameters;
		}

		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = (Predicate<TargetInfo>)((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

		public static TargetingParameters ForTrade()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = (Predicate<TargetInfo>)delegate(TargetInfo x)
			{
				ITrader trader = x.Thing as ITrader;
				return trader != null && trader.CanTradeNow;
			};
			return targetingParameters;
		}

		public static TargetingParameters ForDropPodsDestination()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = (Predicate<TargetInfo>)((TargetInfo x) => DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true));
			return targetingParameters;
		}
	}
}
