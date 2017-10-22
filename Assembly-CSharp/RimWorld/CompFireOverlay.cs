using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompFireOverlay : ThingComp
	{
		private static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)base.props;
			}
		}

		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 drawPos = base.parent.DrawPos;
			drawPos.y += 0.046875f;
			CompFireOverlay.FireGraphic.Draw(drawPos, Rot4.North, base.parent, 0f);
		}
	}
}
