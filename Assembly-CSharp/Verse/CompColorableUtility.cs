using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E00 RID: 3584
	public static class CompColorableUtility
	{
		// Token: 0x06005142 RID: 20802 RVA: 0x0029BC18 File Offset: 0x0029A018
		public static void SetColor(this Thing t, Color newColor, bool reportFailure = true)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on non-ThingWithComps " + t, false);
				}
			}
			else
			{
				CompColorable comp = thingWithComps.GetComp<CompColorable>();
				if (comp == null)
				{
					if (reportFailure)
					{
						Log.Error("SetColor on Thing without CompColorable " + t, false);
					}
				}
				else if (comp.Color != newColor)
				{
					comp.Color = newColor;
				}
			}
		}
	}
}
