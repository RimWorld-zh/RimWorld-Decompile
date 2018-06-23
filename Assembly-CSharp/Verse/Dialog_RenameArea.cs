using System;

namespace Verse
{
	// Token: 0x02000EB6 RID: 3766
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x04003B6A RID: 15210
		private Area area;

		// Token: 0x06005927 RID: 22823 RVA: 0x002DBF9D File Offset: 0x002DA39D
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x002DBFBC File Offset: 0x002DA3BC
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

		// Token: 0x06005929 RID: 22825 RVA: 0x002DC04B File Offset: 0x002DA44B
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}
	}
}
