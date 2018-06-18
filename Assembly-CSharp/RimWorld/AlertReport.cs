using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078B RID: 1931
	public struct AlertReport
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x00169CE8 File Offset: 0x001680E8
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

		// Token: 0x06002AC8 RID: 10952 RVA: 0x00169D6C File Offset: 0x0016816C
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

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00169DB4 File Offset: 0x001681B4
		public static AlertReport CulpritsAre(IEnumerable<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culprits = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00169DE8 File Offset: 0x001681E8
		public static AlertReport CulpritsAre(IEnumerable<Thing> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.ThingToTargetInfo));
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x00169E1C File Offset: 0x0016821C
		public static AlertReport CulpritsAre(IEnumerable<Pawn> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.PawnToTargetInfo));
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x00169E50 File Offset: 0x00168250
		public static AlertReport CulpritsAre(IEnumerable<Building> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.BuildingToTargetInfo));
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x00169E84 File Offset: 0x00168284
		public static AlertReport CulpritsAre(IEnumerable<WorldObject> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.WorldObjectToTargetInfo));
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x00169EB8 File Offset: 0x001682B8
		public static AlertReport CulpritsAre(IEnumerable<Caravan> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.CaravanToTargetInfo));
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x00169EEC File Offset: 0x001682EC
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x00169F14 File Offset: 0x00168314
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x00169F34 File Offset: 0x00168334
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x00169F54 File Offset: 0x00168354
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x00169F70 File Offset: 0x00168370
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

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x00169F98 File Offset: 0x00168398
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

		// Token: 0x0400171C RID: 5916
		public bool active;

		// Token: 0x0400171D RID: 5917
		public IEnumerable<GlobalTargetInfo> culprits;

		// Token: 0x0400171E RID: 5918
		private static Func<Thing, GlobalTargetInfo> ThingToTargetInfo = (Thing x) => new GlobalTargetInfo(x);

		// Token: 0x0400171F RID: 5919
		private static Func<Pawn, GlobalTargetInfo> PawnToTargetInfo = (Pawn x) => new GlobalTargetInfo(x);

		// Token: 0x04001720 RID: 5920
		private static Func<Building, GlobalTargetInfo> BuildingToTargetInfo = (Building x) => new GlobalTargetInfo(x);

		// Token: 0x04001721 RID: 5921
		private static Func<WorldObject, GlobalTargetInfo> WorldObjectToTargetInfo = (WorldObject x) => new GlobalTargetInfo(x);

		// Token: 0x04001722 RID: 5922
		private static Func<Caravan, GlobalTargetInfo> CaravanToTargetInfo = (Caravan x) => new GlobalTargetInfo(x);
	}
}
