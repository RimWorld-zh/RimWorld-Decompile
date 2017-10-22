using Verse;

namespace RimWorld.Planet
{
	public static class HillinessUtility
	{
		public static string GetLabel(this Hilliness h)
		{
			string result;
			switch (h)
			{
			case Hilliness.Flat:
			{
				result = "Hilliness_Flat".Translate();
				break;
			}
			case Hilliness.SmallHills:
			{
				result = "Hilliness_SmallHills".Translate();
				break;
			}
			case Hilliness.LargeHills:
			{
				result = "Hilliness_LargeHills".Translate();
				break;
			}
			case Hilliness.Mountainous:
			{
				result = "Hilliness_Mountainous".Translate();
				break;
			}
			case Hilliness.Impassable:
			{
				result = "Hilliness_Impassable".Translate();
				break;
			}
			default:
			{
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362);
				result = h.ToString();
				break;
			}
			}
			return result;
		}

		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
