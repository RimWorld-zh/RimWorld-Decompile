using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000049 RID: 73
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00018C5C File Offset: 0x0001705C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00018C78 File Offset: 0x00017078
		protected override int TotalNeededWork
		{
			get
			{
				return 90;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00018C8F File Offset: 0x0001708F
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}

		// Token: 0x040001DD RID: 477
		public const int UninstallWork = 90;
	}
}
