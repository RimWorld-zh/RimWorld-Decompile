using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDF RID: 3551
	public class Graphic_Multi : Graphic
	{
		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06004F62 RID: 20322 RVA: 0x00294CE4 File Offset: 0x002930E4
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06004F63 RID: 20323 RVA: 0x00294D00 File Offset: 0x00293100
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06004F64 RID: 20324 RVA: 0x00294D1C File Offset: 0x0029311C
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x00294D3C File Offset: 0x0029313C
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004F66 RID: 20326 RVA: 0x00294D5C File Offset: 0x0029315C
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004F67 RID: 20327 RVA: 0x00294D7C File Offset: 0x0029317C
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x00294D9C File Offset: 0x0029319C
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004F69 RID: 20329 RVA: 0x00294DB8 File Offset: 0x002931B8
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.MatEast == this.MatNorth;
			}
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x00294DE0 File Offset: 0x002931E0
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

		// Token: 0x06004F6B RID: 20331 RVA: 0x00295070 File Offset: 0x00293470
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x002950A0 File Offset: 0x002934A0
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

		// Token: 0x06004F6D RID: 20333 RVA: 0x00295108 File Offset: 0x00293508
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<string>(seed, this.path);
			seed = Gen.HashCombineStruct<Color>(seed, this.color);
			return Gen.HashCombineStruct<Color>(seed, this.colorTwo);
		}

		// Token: 0x040034BA RID: 13498
		private Material[] mats = new Material[4];

		// Token: 0x040034BB RID: 13499
		private bool westFlipped = false;
	}
}
