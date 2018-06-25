using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class PowerOverlayMats
	{
		private const string TransmitterAtlasPath = "Things/Special/Power/TransmitterAtlas";

		private static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;

		public static readonly Graphic LinkedOverlayGraphic;

		public static readonly Material MatConnectorBase = MaterialPool.MatFrom("Things/Special/Power/OverlayBase", ShaderDatabase.MetaOverlay);

		public static readonly Material MatConnectorLine = MaterialPool.MatFrom("Things/Special/Power/OverlayWire", ShaderDatabase.MetaOverlay);

		public static readonly Material MatConnectorAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayWireAnticipated", ShaderDatabase.MetaOverlay);

		public static readonly Material MatConnectorBaseAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayBaseAnticipated", ShaderDatabase.MetaOverlay);

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
