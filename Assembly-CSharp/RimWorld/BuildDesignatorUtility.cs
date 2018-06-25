using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E3 RID: 2019
	public static class BuildDesignatorUtility
	{
		// Token: 0x06002CDA RID: 11482 RVA: 0x0017A05C File Offset: 0x0017845C
		public static void TryDrawPowerGridAndAnticipatedConnection(BuildableDef def, Rot4 rotation)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && (thingDef.EverTransmitsPower || thingDef.ConnectToPower))
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
				if (thingDef.ConnectToPower)
				{
					IntVec3 intVec = UI.MouseCell();
					CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(intVec, Find.CurrentMap, null);
					if (compPower != null && !compPower.parent.Position.Fogged(compPower.parent.Map))
					{
						PowerNetGraphics.RenderAnticipatedWirePieceConnecting(intVec, rotation, def.Size, compPower.parent);
					}
				}
			}
		}
	}
}
