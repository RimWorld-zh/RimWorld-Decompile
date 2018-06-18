using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E03 RID: 3587
	public static class CompColorableUtility
	{
		// Token: 0x0600512E RID: 20782 RVA: 0x0029A63C File Offset: 0x00298A3C
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
