using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000386 RID: 902
	public static class FlickUtility
	{
		// Token: 0x06000FA0 RID: 4000 RVA: 0x00083D34 File Offset: 0x00082134
		public static void UpdateFlickDesignation(Thing t)
		{
			bool flag = false;
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps != null)
			{
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						flag = true;
						break;
					}
				}
			}
			Designation designation = t.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick);
			if (flag && designation == null)
			{
				t.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Flick));
			}
			else if (!flag && designation != null)
			{
				designation.Delete();
			}
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.SwitchFlickingDesignation);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00083E04 File Offset: 0x00082204
		public static bool WantsToBeOn(Thing t)
		{
			CompFlickable compFlickable = t.TryGetComp<CompFlickable>();
			bool result;
			if (compFlickable != null && !compFlickable.SwitchIsOn)
			{
				result = false;
			}
			else
			{
				CompSchedule compSchedule = t.TryGetComp<CompSchedule>();
				result = (compSchedule == null || compSchedule.Allowed);
			}
			return result;
		}
	}
}
