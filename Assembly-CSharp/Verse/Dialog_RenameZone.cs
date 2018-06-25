using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000EB8 RID: 3768
	public class Dialog_RenameZone : Dialog_Rename
	{
		// Token: 0x04003B71 RID: 15217
		private Zone zone;

		// Token: 0x06005928 RID: 22824 RVA: 0x002DC190 File Offset: 0x002DA590
		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x002DC1AC File Offset: 0x002DA5AC
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

		// Token: 0x0600592A RID: 22826 RVA: 0x002DC23B File Offset: 0x002DA63B
		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(new object[]
			{
				this.curName
			}), MessageTypeDefOf.TaskCompletion, false);
		}
	}
}
