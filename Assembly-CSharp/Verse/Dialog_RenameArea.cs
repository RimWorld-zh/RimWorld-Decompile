using System;

namespace Verse
{
	// Token: 0x02000EB7 RID: 3767
	public class Dialog_RenameArea : Dialog_Rename
	{
		// Token: 0x06005906 RID: 22790 RVA: 0x002DA351 File Offset: 0x002D8751
		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x002DA370 File Offset: 0x002D8770
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

		// Token: 0x06005908 RID: 22792 RVA: 0x002DA3FF File Offset: 0x002D87FF
		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		// Token: 0x04003B5A RID: 15194
		private Area area;
	}
}
