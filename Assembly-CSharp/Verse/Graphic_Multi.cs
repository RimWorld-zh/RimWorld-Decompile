using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDD RID: 3549
	public class Graphic_Multi : Graphic
	{
		// Token: 0x040034C3 RID: 13507
		private Material[] mats = new Material[4];

		// Token: 0x040034C4 RID: 13508
		private bool westFlipped = false;

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06004F79 RID: 20345 RVA: 0x002963CC File Offset: 0x002947CC
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06004F7A RID: 20346 RVA: 0x002963E8 File Offset: 0x002947E8
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06004F7B RID: 20347 RVA: 0x00296404 File Offset: 0x00294804
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004F7C RID: 20348 RVA: 0x00296424 File Offset: 0x00294824
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004F7D RID: 20349 RVA: 0x00296444 File Offset: 0x00294844
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004F7E RID: 20350 RVA: 0x00296464 File Offset: 0x00294864
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004F7F RID: 20351 RVA: 0x00296484 File Offset: 0x00294884
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004F80 RID: 20352 RVA: 0x002964A0 File Offset: 0x002948A0
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.MatEast == this.MatNorth;
			}
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x002964C8 File Offset: 0x002948C8
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

		// Token: 0x06004F82 RID: 20354 RVA: 0x00296758 File Offset: 0x00294B58
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00296788 File Offset: 0x00294B88
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

		// Token: 0x06004F84 RID: 20356 RVA: 0x002967F0 File Offset: 0x00294BF0
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<string>(seed, this.path);
			seed = Gen.HashCombineStruct<Color>(seed, this.color);
			return Gen.HashCombineStruct<Color>(seed, this.colorTwo);
		}
	}
}
