using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000EB6 RID: 3766
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x06005903 RID: 22787 RVA: 0x002DA22C File Offset: 0x002D862C
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x06005904 RID: 22788 RVA: 0x002DA248 File Offset: 0x002D8648
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport acceptanceReport = base.NameIsValid(name);
			AcceptanceReport result;
			if (!acceptanceReport.Accepted)
			{
				result = acceptanceReport;
			}
			else if (this.zone.Map.zoneManager.AllZones.Any((Zone z) => z != this.zone && z.label == name))
			{
				result = "NameIsInUse".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06005905 RID: 22789 RVA: 0x002DA2D7 File Offset: 0x002D86D7
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(new object[]
			{
				this.curName
			}), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x04003B59 RID: 15193
		private Zone zone;
	}
}
