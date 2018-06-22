using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000EB5 RID: 3765
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x06005924 RID: 22820 RVA: 0x002DBE78 File Offset: 0x002DA278
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x002DBE94 File Offset: 0x002DA294
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

		// Token: 0x06005926 RID: 22822 RVA: 0x002DBF23 File Offset: 0x002DA323
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(new object[]
			{
				this.curName
			}), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x04003B69 RID: 15209
		private Zone zone;
	}
}
