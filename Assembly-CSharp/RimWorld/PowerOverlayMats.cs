using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000426 RID: 1062
	[StaticConstructorOnStartup]
	public static class PowerOverlayMats
	{
		// Token: 0x04000B4C RID: 2892
		private const string TransmitterAtlasPath = "Things/Special/Power/TransmitterAtlas";

		// Token: 0x04000B4D RID: 2893
		private static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;

		// Token: 0x04000B4E RID: 2894
		public static readonly Graphic LinkedOverlayGraphic;

		// Token: 0x04000B4F RID: 2895
		public static readonly Material MatConnectorBase = MaterialPool.MatFrom("Things/Special/Power/OverlayBase", ShaderDatabase.MetaOverlay);

		// Token: 0x04000B50 RID: 2896
		public static readonly Material MatConnectorLine = MaterialPool.MatFrom("Things/Special/Power/OverlayWire", ShaderDatabase.MetaOverlay);

		// Token: 0x04000B51 RID: 2897
		public static readonly Material MatConnectorAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayWireAnticipated", ShaderDatabase.MetaOverlay);

		// Token: 0x04000B52 RID: 2898
		public static readonly Material MatConnectorBaseAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayBaseAnticipated", ShaderDatabase.MetaOverlay);

		// Token: 0x06001288 RID: 4744 RVA: 0x000A1054 File Offset: 0x0009F454
		static PowerOverlayMats()
		{
			Graphic graphic = GraphicDatabase.Get<Graphic_Single>("Things/Special/Power/TransmitterAtlas", PowerOverlayMats.TransmitterShader);
			PowerOverlayMats.LinkedOverlayGraphic = GraphicUtility.WrapLinked(graphic, LinkDrawerType.TransmitterOverlay);
			graphic.MatSingle.renderQueue = 3600;
			PowerOverlayMats.MatConnectorBase.renderQueue = 3600;
			PowerOverlayMats.MatConnectorLine.renderQueue = 3600;
		}
	}
}
