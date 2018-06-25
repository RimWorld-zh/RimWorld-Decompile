using System;
using UnityEngine;

namespace Verse
{
	public class Graphic_Multi : Graphic
	{
		private Material[] mats = new Material[4];

		private bool westFlipped = false;

		public Graphic_Multi()
		{
		}

		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		public override bool ShouldDrawRotated
		{
			get
			{
				return this.MatEast == this.MatNorth;
			}
		}

		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			if (array[0] == null)
			{
				Log.Error("Failed to find any texture while constructing " + this.ToString() + ". Filenames have changed; if you are converting an old mod, recommend renaming textures from *_back to *_north, *_side to *_east, and *_front to *_south.", false);
			}
			else
			{
				array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
				if (array[1] == null)
				{
					array[1] = array[0];
				}
				array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
				if (array[2] == null)
				{
					array[2] = array[0];
				}
				array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
				if (array[3] == null)
				{
					array[3] = array[1];
					this.westFlipped = base.WestFlipped;
				}
				Texture2D[] array2 = new Texture2D[this.mats.Length];
				if (req.shader.SupportsMaskTex())
				{
					array2[0] = ContentFinder<Texture2D>.Get(req.path + "_northm", false);
					if (array2[0] != null)
					{
						array2[1] = ContentFinder<Texture2D>.Get(req.path + "_eastm", false);
						if (array2[1] == null)
						{
							array2[1] = array2[0];
						}
						array2[2] = ContentFinder<Texture2D>.Get(req.path + "_southm", false);
						if (array2[2] == null)
						{
							array2[2] = array2[0];
						}
						array2[3] = ContentFinder<Texture2D>.Get(req.path + "_westm", false);
						if (array2[3] == null)
						{
							array2[3] = array2[1];
						}
					}
				}
				for (int i = 0; i < this.mats.Length; i++)
				{
					MaterialRequest req2 = default(MaterialRequest);
					req2.mainTex = array[i];
					req2.shader = req.shader;
					req2.color = this.color;
					req2.colorTwo = this.colorTwo;
					req2.maskTex = array2[i];
					req2.shaderParameters = req.shaderParameters;
					this.mats[i] = MaterialPool.MatFrom(req2);
				}
			}
		}

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Multi(initPath=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<string>(seed, this.path);
			seed = Gen.HashCombineStruct<Color>(seed, this.color);
			return Gen.HashCombineStruct<Color>(seed, this.colorTwo);
		}
	}
}
