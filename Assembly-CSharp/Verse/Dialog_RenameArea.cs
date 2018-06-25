using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class Dialog_RenameArea : Dialog_Rename
	{
		private Area area;

		public Dialog_RenameArea(Area area)
		{
			this.area = area;
			this.curName = area.Label;
		}

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

		protected override void SetName(string name)
		{
			this.area.SetLabel(this.curName);
		}

		[CompilerGenerated]
		private sealed class <NameIsValid>c__AnonStorey0
		{
			internal string name;

			internal Dialog_RenameArea $this;

			public <NameIsValid>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Area a)
			{
				return a != this.$this.area && a.Label == this.name;
			}
		}
	}
}
