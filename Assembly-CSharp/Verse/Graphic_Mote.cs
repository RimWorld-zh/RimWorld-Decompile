using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing)
		{
			Mote mote = (Mote)thing;
			float num = Graphic_Mote.CalculateMoteAlpha(mote);
			if (!(num <= 0.0))
			{
				Color color = base.Color * mote.instanceColor;
				color.a *= num;
				Vector3 exactScale = mote.exactScale;
				exactScale.x *= base.data.drawSize.x;
				exactScale.z *= base.data.drawSize.y;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
				Material matSingle = this.MatSingle;
				if (color.IndistinguishableFrom(matSingle.color))
				{
					Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, 0);
				}
				else
				{
					Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color);
					Graphics.DrawMesh(MeshPool.plane10, matrix, matSingle, 0, null, 0, Graphic_Mote.propertyBlock);
				}
			}
		}

		public static float CalculateMoteAlpha(Mote mote)
		{
			ThingDef def = mote.def;
			float ageSecs = mote.AgeSecs;
			if (ageSecs <= def.mote.fadeInTime)
			{
				if (def.mote.fadeInTime > 0.0)
				{
					return ageSecs / def.mote.fadeInTime;
				}
				return 1f;
			}
			if (ageSecs <= def.mote.fadeInTime + def.mote.solidTime)
			{
				return 1f;
			}
			if (def.mote.fadeOutTime > 0.0)
			{
				return (float)(1.0 - Mathf.InverseLerp(def.mote.fadeInTime + def.mote.solidTime, def.mote.fadeInTime + def.mote.solidTime + def.mote.fadeOutTime, ageSecs));
			}
			return 1f;
		}

		public override string ToString()
		{
			return "Mote(path=" + base.path + ", shader=" + base.Shader + ", color=" + base.color + ", colorTwo=unsupported)";
		}
	}
}
