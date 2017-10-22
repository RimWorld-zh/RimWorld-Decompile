using System;

namespace Verse
{
	public class Dialog_RenameArea : Dialog_Rename
	{
		private Area area;

		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			base.curName = area.Label;
		}

		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport acceptanceReport = base.NameIsValid(name);
			return acceptanceReport.Accepted ? ((!this.area.Map.areaManager.AllAreas.Any((Predicate<Area>)((Area a) => a.Label == name))) ? true : "NameIsInUse".Translate()) : acceptanceReport;
		}

		protected override void SetName(string name)
		{
			this.area.SetLabel(base.curName);
		}
	}
}
