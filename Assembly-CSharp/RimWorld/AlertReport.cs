using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000787 RID: 1927
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
		// (get) Token: 0x06002AC0 RID: 10944 RVA: 0x00169EC0 File Offset: 0x001682C0
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

		// Token: 0x06002AC1 RID: 10945 RVA: 0x00169F44 File Offset: 0x00168344
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

		// Token: 0x06002AC2 RID: 10946 RVA: 0x00169F8C File Offset: 0x0016838C
		public static AlertReport CulpritsAre(IEnumerable<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culprits = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x00169FC0 File Offset: 0x001683C0
		public static AlertReport CulpritsAre(IEnumerable<Thing> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.ThingToTargetInfo));
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x00169FF4 File Offset: 0x001683F4
		public static AlertReport CulpritsAre(IEnumerable<Pawn> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.PawnToTargetInfo));
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x0016A028 File Offset: 0x00168428
		public static AlertReport CulpritsAre(IEnumerable<Building> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.BuildingToTargetInfo));
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x0016A05C File Offset: 0x0016845C
		public static AlertReport CulpritsAre(IEnumerable<WorldObject> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.WorldObjectToTargetInfo));
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0016A090 File Offset: 0x00168490
		public static AlertReport CulpritsAre(IEnumerable<Caravan> culprits)
		{
			return AlertReport.CulpritsAre((culprits == null) ? null : culprits.Select(AlertReport.CaravanToTargetInfo));
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0016A0C4 File Offset: 0x001684C4
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0016A0EC File Offset: 0x001684EC
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x0016A10C File Offset: 0x0016850C
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x0016A12C File Offset: 0x0016852C
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x0016A148 File Offset: 0x00168548
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
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x0016A170 File Offset: 0x00168570
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
