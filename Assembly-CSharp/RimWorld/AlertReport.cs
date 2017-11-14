using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public struct AlertReport
	{
		public bool active;

		public GlobalTargetInfo culprit;

		public static AlertReport Active
		{
			get
			{
				AlertReport result = default(AlertReport);
				result.active = true;
				result.culprit = GlobalTargetInfo.Invalid;
				return result;
			}
		}

		public static AlertReport Inactive
		{
			get
			{
				AlertReport result = default(AlertReport);
				result.active = false;
				result.culprit = GlobalTargetInfo.Invalid;
				return result;
			}
		}

		public static AlertReport CulpritIs(GlobalTargetInfo culp)
		{
			AlertReport result = default(AlertReport);
			result.active = culp.IsValid;
			result.culprit = culp;
			return result;
		}

		public static implicit operator AlertReport(bool b)
		{
			AlertReport result = default(AlertReport);
			result.active = b;
			result.culprit = GlobalTargetInfo.Invalid;
			return result;
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
	}
}
