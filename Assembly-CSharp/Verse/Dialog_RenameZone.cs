using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000EB7 RID: 3767
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x06005905 RID: 22789 RVA: 0x002DA1F4 File Offset: 0x002D85F4
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x06005906 RID: 22790 RVA: 0x002DA210 File Offset: 0x002D8610
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

		// Token: 0x06005907 RID: 22791 RVA: 0x002DA29F File Offset: 0x002D869F
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(new object[]
			{
				this.curName
			}), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x04003B5A RID: 15194
		private Zone zone;
	}
}
