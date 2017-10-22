using Verse;

namespace RimWorld
{
	public static class BuildDesignatorUtility
	{
		public static void TryDrawPowerGridAndAnticipatedConnection(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				if (!thingDef.EverTransmitsPower && !thingDef.ConnectToPower)
					return;
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
				if (thingDef.ConnectToPower)
				{
					IntVec3 intVec = UI.MouseCell();
					CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(intVec, Find.VisibleMap, null);
					if (compPower != null)
					{
						PowerNetGraphics.RenderAnticipatedWirePieceConnecting(intVec, compPower.parent);
					}
				}
			}
		}
	}
}
