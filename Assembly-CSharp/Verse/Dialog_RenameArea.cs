using System;

namespace Verse
{
	// Token: 0x02000EB8 RID: 3768
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x06005908 RID: 22792 RVA: 0x002DA319 File Offset: 0x002D8719
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x002DA338 File Offset: 0x002D8738
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

		// Token: 0x0600590A RID: 22794 RVA: 0x002DA3C7 File Offset: 0x002D87C7
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		// Token: 0x04003B5B RID: 15195
		private Area area;
	}
}
