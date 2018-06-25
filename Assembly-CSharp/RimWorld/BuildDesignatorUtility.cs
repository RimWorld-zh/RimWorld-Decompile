using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E3 RID: 2019
	public static class BuildDesignatorUtility
	{
		// Token: 0x06002CD9 RID: 11481 RVA: 0x0017A2C0 File Offset: 0x001786C0
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
