using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C8 RID: 1480
	public static class CaravanArrivalActionUtility
	{
		// Token: 0x06001CBF RID: 7359 RVA: 0x000F6CC4 File Offset: 0x000F50C4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Caravan caravan, int pathDestination, WorldObject revalidateWorldClickTarget) where T : CaravanArrivalAction
		{
			FloatMenuAcceptanceReport rep = acceptanceReportGetter();
			if (rep.Accepted || !rep.FailReason.NullOrEmpty() || !rep.FailMessage.NullOrEmpty())
			{
				if (!rep.FailReason.NullOrEmpty())
				{
					yield return new FloatMenuOption(label + " (" + rep.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					Action action = delegate()
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
						if (floatMenuAcceptanceReport.Accepted)
						{
							caravan.pather.StartPath(pathDestination, arrivalActionGetter(), true, true);
						}
						else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, false);
						}
					};
					yield return new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
					if (Prefs.DevMode)
					{
						string label2 = label + " (Dev: instantly)";
						action = delegate()
						{
							FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
							if (floatMenuAcceptanceReport.Accepted)
							{
								caravan.Tile = pathDestination;
								caravan.pather.StopDead();
								T t = arrivalActionGetter();
								t.Arrived(caravan);
							}
							else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
							{
								Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(pathDestination), MessageTypeDefOf.RejectInput, false);
							}
						};
						yield return new FloatMenuOption(label2, action, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget);
					}
				}
			}
			yield break;
		}
	}
}
