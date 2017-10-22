using UnityEngine;

namespace Verse
{
	public class Graphic_Single : Graphic
	{
		protected Material mat = null;

		public static readonly string MaskSuffix = "_m";

		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		public override Material MatFront
		{
			get
			{
				return this.mat;
			}
		}

		public override Material MatSide
		{
			get
			{
				return this.mat;
			}
		}

		public override Material MatBack
		{
			get
			{
				return this.mat;
			}
		}

		public override bool ShouldDrawRotated
		{
			get
			{
				return (byte)((base.data == null || base.data.drawRotated) ? 1 : 0) != 0;
			}
		}

		public override void Init(GraphicRequest req)
		{
			base.data = req.graphicData;
			base.path = req.path;
			base.color = req.color;
			base.colorTwo = req.colorTwo;
			base.drawSize = req.drawSize;
			MaterialRequest req2 = new MaterialRequest
			{
				mainTex = ContentFinder<Texture2D>.Get(req.path, true),
				shader = req.shader,
				color = base.color,
				colorTwo = base.colorTwo,
				renderQueue = req.renderQueue
			};
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(req.path + Graphic_Single.MaskSuffix, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(base.path, newShader, base.drawSize, newColor, newColorTwo, base.data);
		}

		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		public override string ToString()
		{
			return "Single(path=" + base.path + ", color=" + base.color + ", colorTwo=" + base.colorTwo + ")";
		}
	}
}
