using RimWorld;
using System;

namespace Verse
{
	public class Dialog_RenameZone : Dialog_Rename
	{
		private Zone zone;

		public Dialog_RenameZone(Zone zone)
		{
			this.zone = zone;
			base.curName = zone.label;
		}

		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport acceptanceReport = base.NameIsValid(name);
			return acceptanceReport.Accepted ? ((!this.zone.Map.zoneManager.AllZones.Any((Predicate<Zone>)((Zone z) => z.label == name))) ? true : "NameIsInUse".Translate()) : acceptanceReport;
		}

		protected override void SetName(string name)
		{
			this.zone.label = base.curName;
			Messages.Message("ZoneGainsName".Translate(base.curName), MessageTypeDefOf.TaskCompletion);
		}
	}
}
