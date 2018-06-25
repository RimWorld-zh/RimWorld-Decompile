using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000388 RID: 904
	public static class FlickUtility
	{
		// Token: 0x06000FA4 RID: 4004 RVA: 0x00083E84 File Offset: 0x00082284
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

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00083F54 File Offset: 0x00082354
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
