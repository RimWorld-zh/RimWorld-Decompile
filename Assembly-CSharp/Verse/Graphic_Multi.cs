using UnityEngine;

namespace Verse
{
	public class Graphic_Multi : Graphic
	{
		private Material[] mats = new Material[3];

		public string GraphicPath
		{
			get
			{
				return base.path;
			}
		}

		public override Material MatSingle
		{
			get
			{
				return this.mats[2];
			}
		}

		public override Material MatFront
		{
			get
			{
				return this.mats[2];
			}
		}

		public override Material MatSide
		{
			get
			{
				return this.mats[1];
			}
		}

		public override Material MatBack
		{
			get
			{
				return this.mats[0];
			}
		}

		public override bool ShouldDrawRotated
		{
			get
			{
				return (Object)this.MatSide == (Object)this.MatBack;
			}
		}

		public override void Init(GraphicRequest req)
		{
			base.data = req.graphicData;
			base.path = req.path;
			base.color = req.color;
			base.colorTwo = req.colorTwo;
			base.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[3]
			{
				ContentFinder<Texture2D>.Get(req.path + "_back", false),
				null,
				null
			};
			if ((Object)array[0] == (Object)null)
			{
				Log.Error("Failed to find any texture while constructing " + this.ToString());
			}
			else
			{
				array[1] = ContentFinder<Texture2D>.Get(req.path + "_side", false);
				if ((Object)array[1] == (Object)null)
				{
					array[1] = array[0];
				}
				array[2] = ContentFinder<Texture2D>.Get(req.path + "_front", false);
				if ((Object)array[2] == (Object)null)
				{
					array[2] = array[0];
				}
				Texture2D[] array2 = new Texture2D[3];
				if (req.shader.SupportsMaskTex())
				{
					array2[0] = ContentFinder<Texture2D>.Get(req.path + "_backm", false);
					if ((Object)array2[0] != (Object)null)
					{
						array2[1] = ContentFinder<Texture2D>.Get(req.path + "_sidem", false);
						if ((Object)array2[1] == (Object)null)
						{
							array2[1] = array2[0];
						}
						array2[2] = ContentFinder<Texture2D>.Get(req.path + "_frontm", false);
						if ((Object)array2[2] == (Object)null)
						{
							array2[2] = array2[0];
						}
					}
				}
				for (int i = 0; i < 3; i++)
				{
					MaterialRequest req2 = new MaterialRequest
					{
						mainTex = array[i],
						shader = req.shader,
						color = base.color,
						colorTwo = base.colorTwo,
						maskTex = array2[i]
					};
					this.mats[i] = MaterialPool.MatFrom(req2);
				}
			}
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(base.path, newShader, base.drawSize, newColor, newColorTwo, base.data);
		}

		public override string ToString()
		{
			return "Multi(initPath=" + base.path + ", color=" + base.color + ", colorTwo=" + base.colorTwo + ")";
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine(seed, base.path);
			seed = Gen.HashCombineStruct(seed, base.color);
			return Gen.HashCombineStruct(seed, base.colorTwo);
		}
	}
}
