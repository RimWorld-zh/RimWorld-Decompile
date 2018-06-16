using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A7 RID: 1447
	public static class HillinessUtility
	{
		// Token: 0x06001B97 RID: 7063 RVA: 0x000EE008 File Offset: 0x000EC408
		public static string GetLabel(this Hilliness h)
		{
			string result;
			switch (h)
			{
			case Hilliness.Flat:
				result = "Hilliness_Flat".Translate();
				break;
			case Hilliness.SmallHills:
				result = "Hilliness_SmallHills".Translate();
				break;
			case Hilliness.LargeHills:
				result = "Hilliness_LargeHills".Translate();
				break;
			case Hilliness.Mountainous:
				result = "Hilliness_Mountainous".Translate();
				break;
			case Hilliness.Impassable:
				result = "Hilliness_Impassable".Translate();
				break;
			default:
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362, false);
				result = h.ToString();
				break;
			}
			return result;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000EE0C0 File Offset: 0x000EC4C0
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
