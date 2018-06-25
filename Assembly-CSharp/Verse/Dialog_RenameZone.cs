using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	public class Dialog_RenameZone : Dialog_Rename
	{
		private Zone zone;

		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			this.curName = zone.label;
		}

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

		protected override void SetName(string name)
		{
			this.zone.label = this.curName;
			Messages.Message("ZoneGainsName".Translate(new object[]
			{
				this.curName
			}), MessageTypeDefOf.TaskCompletion, false);
		}

		[CompilerGenerated]
		private sealed class <NameIsValid>c__AnonStorey0
		{
			internal string name;

			internal Dialog_RenameZone $this;

			public <NameIsValid>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Zone z)
			{
				return z != this.$this.zone && z.label == this.name;
			}
		}
	}
}
