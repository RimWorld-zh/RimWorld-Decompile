using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Graphic_Gas : Graphic_Single
	{
		private const float PositionVariance = 0.45f;

		private const float SizeVariance = 0.2f;

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Rand.PushState();
			Rand.Seed = thing.thingIDNumber.GetHashCode();
			Gas gas = thing as Gas;
			float angle = (float)((float)Rand.Range(0, 360) + ((gas != null) ? gas.graphicRotation : 0.0));
			Vector3 pos = thing.TrueCenter() + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
			Vector3 s = new Vector3(Rand.Range(0.8f, 1.2f) * base.drawSize.x, 0f, Rand.Range(0.8f, 1.2f) * base.drawSize.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.MatSingle, 0);
			Rand.PopState();
		}
	}
}
