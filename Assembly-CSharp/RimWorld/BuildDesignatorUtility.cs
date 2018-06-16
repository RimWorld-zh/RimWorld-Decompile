using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E5 RID: 2021
	public static class BuildDesignatorUtility
	{
		// Token: 0x06002CDB RID: 11483 RVA: 0x00179CA0 File Offset: 0x001780A0
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
