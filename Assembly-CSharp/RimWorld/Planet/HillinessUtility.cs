using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A3 RID: 1443
	public static class HillinessUtility
	{
		// Token: 0x06001B8F RID: 7055 RVA: 0x000EE0C8 File Offset: 0x000EC4C8
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

		// Token: 0x06001B90 RID: 7056 RVA: 0x000EE180 File Offset: 0x000EC580
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
