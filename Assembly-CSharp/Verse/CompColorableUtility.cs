using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E02 RID: 3586
	public static class CompColorableUtility
	{
		// Token: 0x06005146 RID: 20806 RVA: 0x0029BD44 File Offset: 0x0029A144
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
