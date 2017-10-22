using Verse;

namespace RimWorld
{
	public class CompReportWorkSpeed : ThingComp
	{
		public override string CompInspectStringExtra()
		{
			bool flag = StatPart_WorkTableOutdoors.Applies(base.parent.def, base.parent.Map, base.parent.Position);
			bool flag2 = StatPart_WorkTableTemperature.Applies(base.parent);
			bool flag3 = StatPart_WorkTableUnpowered.Applies(base.parent);
			if (!flag && !flag2 && !flag3)
			{
				return (string)null;
			}
			string text = "WorkSpeedPenalty".Translate() + ": ";
			bool flag4 = false;
			if (flag)
			{
				text += "Outdoors".Translate().ToLower();
				flag4 = true;
			}
			if (flag2)
			{
				if (flag4)
				{
					text += ", ";
				}
				text += "BadTemperature".Translate().ToLower();
				flag4 = true;
			}
			if (flag3)
			{
				if (flag4)
				{
					text += ", ";
				}
				text += "NoPower".Translate().ToLower();
			}
			return text;
		}
	}
}
