using System;

namespace Verse
{
	// Token: 0x02000EB8 RID: 3768
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x04003B6A RID: 15210
		private Area area;

		// Token: 0x0600592B RID: 22827 RVA: 0x002DC0C9 File Offset: 0x002DA4C9
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x002DC0E8 File Offset: 0x002DA4E8
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport acceptanceReport = base.NameIsValid(name);
			AcceptanceReport result;
			if (!acceptanceReport.Accepted)
			{
				result = acceptanceReport;
			}
			else if (this.area.Map.areaManager.AllAreas.Any((Area a) => a != this.area && a.Label == name))
			{
				result = "NameIsInUse".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x002DC177 File Offset: 0x002DA577
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}
	}
}
