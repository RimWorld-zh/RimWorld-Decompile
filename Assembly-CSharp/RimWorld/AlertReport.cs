using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000789 RID: 1929
	public struct AlertReport
	{
		// Token: 0x0400171A RID: 5914
		public bool active;

		// Token: 0x0400171B RID: 5915
		public IEnumerable<GlobalTargetInfo> culprits;

		// Token: 0x0400171C RID: 5916
		private static Func<Thing, GlobalTargetInfo> ThingToTargetInfo = (Thing x) => new GlobalTargetInfo(x);

		// Token: 0x0400171D RID: 5917
		private static Func<Pawn, GlobalTargetInfo> PawnToTargetInfo = (Pawn x) => new GlobalTargetInfo(x);

		// Token: 0x0400171E RID: 5918
		private static Func<Building, GlobalTargetInfo> BuildingToTargetInfo = (Building x) => new GlobalTargetInfo(x);

		// Token: 0x0400171F RID: 5919
		private static Func<WorldObject, GlobalTargetInfo> WorldObjectToTargetInfo = (WorldObject x) => new GlobalTargetInfo(x);

		// Token: 0x04001720 RID: 5920
		private static Func<Caravan, GlobalTargetInfo> CaravanToTargetInfo = (Caravan x) => new GlobalTargetInfo(x);

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002AC4 RID: 10948 RVA: 0x0016A010 File Offset: 0x00168410
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

		// Token: 0x06002AC5 RID: 10949 RVA: 0x0016A094 File Offset: 0x00168494
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

		// Token: 0x06002AC6 RID: 10950 RVA: 0x0016A0DC File Offset: 0x001684DC
		public static AlertReport CulpritsAre(IEnumerable<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culprits = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0016A110 File Offset: 0x00168510
		public static AlertReport CulpritsAre(IEnumerable<Thing> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.ThingToTargetInfo));
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0016A144 File Offset: 0x00168544
		public static AlertReport CulpritsAre(IEnumerable<Pawn> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.PawnToTargetInfo));
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0016A178 File Offset: 0x00168578
		public static AlertReport CulpritsAre(IEnumerable<Building> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.BuildingToTargetInfo));
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x0016A1AC File Offset: 0x001685AC
		public static AlertReport CulpritsAre(IEnumerable<WorldObject> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.WorldObjectToTargetInfo));
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x0016A1E0 File Offset: 0x001685E0
		public static AlertReport CulpritsAre(IEnumerable<Caravan> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.CaravanToTargetInfo));
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x0016A214 File Offset: 0x00168614
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x0016A23C File Offset: 0x0016863C
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x0016A25C File Offset: 0x0016865C
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0016A27C File Offset: 0x0016867C
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x0016A298 File Offset: 0x00168698
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

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x0016A2C0 File Offset: 0x001686C0
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
	}
}
