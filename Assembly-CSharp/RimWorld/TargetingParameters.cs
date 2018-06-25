using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private static Predicate<TargetInfo> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<TargetInfo> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<TargetInfo> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<TargetInfo> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<TargetInfo> <>f__am$cache4;

		public TargetingParameters()
		{
		}

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

		public static TargetingParameters ForStrip(Pawn p)
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = ((TargetInfo targ) => targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing));
			return targetingParameters;
		}

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

		[CompilerGenerated]
		private static bool <ForAttackHostile>m__0(TargetInfo targ)
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
		}

		[CompilerGenerated]
		private static bool <ForStrip>m__1(TargetInfo targ)
		{
			return targ.HasThing && StrippableUtility.CanBeStrippedByColony(targ.Thing);
		}

		[CompilerGenerated]
		private static bool <ForTrade>m__2(TargetInfo x)
		{
			ITrader trader = x.Thing as ITrader;
			return trader != null && trader.CanTradeNow;
		}

		[CompilerGenerated]
		private static bool <ForDropPodsDestination>m__3(TargetInfo x)
		{
			return DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true);
		}

		[CompilerGenerated]
		private static bool <ForQuestPawnsWhoWillJoinColony>m__4(TargetInfo x)
		{
			Pawn pawn = x.Thing as Pawn;
			return pawn != null && !pawn.Dead && pawn.mindState.willJoinColonyIfRescued;
		}

		[CompilerGenerated]
		private sealed class <ForArrest>c__AnonStorey0
		{
			internal Pawn arrester;

			public <ForArrest>c__AnonStorey0()
			{
			}

			internal bool <>m__0(TargetInfo targ)
			{
				bool result;
				if (!targ.HasThing)
				{
					result = false;
				}
				else
				{
					Pawn pawn = targ.Thing as Pawn;
					result = (pawn != null && pawn != this.arrester && pawn.CanBeArrestedBy(this.arrester) && !pawn.Downed);
				}
				return result;
			}
		}
	}
}
