using Verse;

namespace RimWorld
{
	public static class FlickUtility
	{
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
				result = ((byte)((compSchedule == null || compSchedule.Allowed) ? 1 : 0) != 0);
			}
			return result;
		}
	}
}
