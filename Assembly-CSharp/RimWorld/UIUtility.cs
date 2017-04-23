using System;

namespace RimWorld
{
	public static class UIUtility
	{
		public static int CalculateAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement10x.IsDown && KeyBindingDefOf.ModifierIncrement100x.IsDown)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement100x.IsDown)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement10x.IsDown)
			{
				return 10;
			}
			return 1;
		}
	}
}
