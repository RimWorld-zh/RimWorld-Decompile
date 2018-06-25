using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A5 RID: 1445
	public static class HillinessUtility
	{
		// Token: 0x06001B92 RID: 7058 RVA: 0x000EE480 File Offset: 0x000EC880
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

		// Token: 0x06001B93 RID: 7059 RVA: 0x000EE538 File Offset: 0x000EC938
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
