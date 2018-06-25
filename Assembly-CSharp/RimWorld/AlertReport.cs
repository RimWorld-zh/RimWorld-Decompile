using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public struct AlertReport
	{
		public bool active;

		public IEnumerable<GlobalTargetInfo> culprits;

		private static Func<Thing, GlobalTargetInfo> ThingToTargetInfo = (Thing x) => new GlobalTargetInfo(x);

		private static Func<Pawn, GlobalTargetInfo> PawnToTargetInfo = (Pawn x) => new GlobalTargetInfo(x);

		private static Func<Building, GlobalTargetInfo> BuildingToTargetInfo = (Building x) => new GlobalTargetInfo(x);

		private static Func<WorldObject, GlobalTargetInfo> WorldObjectToTargetInfo = (WorldObject x) => new GlobalTargetInfo(x);

		private static Func<Caravan, GlobalTargetInfo> CaravanToTargetInfo = (Caravan x) => new GlobalTargetInfo(x);

		public bool AnyCulpritValid
		{
			get
			{
				bool result;
				if (this.culprits == null)
				{
					result = false;
				}
				else
				{
					foreach (GlobalTargetInfo globalTargetInfo in this.culprits)
					{
						if (globalTargetInfo.IsValid)
						{
							return true;
						}
					}
					result = false;
				}
				return result;
			}
		}

		public static AlertReport CulpritIs(GlobalTargetInfo culp)
		{
			AlertReport result = default(AlertReport);
			result.active = culp.IsValid;
			if (culp.IsValid)
			{
				result.culprits = Gen.YieldSingle<GlobalTargetInfo>(culp);
			}
			return result;
		}

		public static AlertReport CulpritsAre(IEnumerable<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culprits = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		public static AlertReport CulpritsAre(IEnumerable<Thing> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.ThingToTargetInfo));
		}

		public static AlertReport CulpritsAre(IEnumerable<Pawn> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.PawnToTargetInfo));
		}

		public static AlertReport CulpritsAre(IEnumerable<Building> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.BuildingToTargetInfo));
		}

		public static AlertReport CulpritsAre(IEnumerable<WorldObject> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.WorldObjectToTargetInfo));
		}

		public static AlertReport CulpritsAre(IEnumerable<Caravan> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.CaravanToTargetInfo));
		}

		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		public static AlertReport Active
		{
			get
			{
				return new AlertReport
				{
					active = true
				};
			}
		}

		public static AlertReport Inactive
		{
			get
			{
				return new AlertReport
				{
					active = false
				};
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static AlertReport()
		{
		}

		[CompilerGenerated]
		private static GlobalTargetInfo <ThingToTargetInfo>m__0(Thing x)
		{
			return new GlobalTargetInfo(x);
		}

		[CompilerGenerated]
		private static GlobalTargetInfo <PawnToTargetInfo>m__1(Pawn x)
		{
			return new GlobalTargetInfo(x);
		}

		[CompilerGenerated]
		private static GlobalTargetInfo <BuildingToTargetInfo>m__2(Building x)
		{
			return new GlobalTargetInfo(x);
		}

		[CompilerGenerated]
		private static GlobalTargetInfo <WorldObjectToTargetInfo>m__3(WorldObject x)
		{
			return new GlobalTargetInfo(x);
		}

		[CompilerGenerated]
		private static GlobalTargetInfo <CaravanToTargetInfo>m__4(Caravan x)
		{
			return new GlobalTargetInfo(x);
		}
	}
}
