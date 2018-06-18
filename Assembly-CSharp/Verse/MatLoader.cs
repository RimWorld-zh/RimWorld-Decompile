using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F62 RID: 3938
	public static class MatLoader
	{
		// Token: 0x06005F25 RID: 24357 RVA: 0x003077DC File Offset: 0x00305BDC
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath, false);
			}
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2;
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = MaterialAllocator.Create(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}

		// Token: 0x04003E67 RID: 15975
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x02000F63 RID: 3939
		private struct Request
		{
			// Token: 0x06005F27 RID: 24359 RVA: 0x0030788C File Offset: 0x00305C8C
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.path);
				return Gen.HashCombineInt(seed, this.renderQueue);
			}

			// Token: 0x06005F28 RID: 24360 RVA: 0x003078C0 File Offset: 0x00305CC0
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06005F29 RID: 24361 RVA: 0x003078F4 File Offset: 0x00305CF4
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06005F2A RID: 24362 RVA: 0x00307934 File Offset: 0x00305D34
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06005F2B RID: 24363 RVA: 0x00307954 File Offset: 0x00305D54
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06005F2C RID: 24364 RVA: 0x00307974 File Offset: 0x00305D74
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue,
					")"
				});
			}

			// Token: 0x04003E68 RID: 15976
			public string path;

			// Token: 0x04003E69 RID: 15977
			public int renderQueue;
		}
	}
}
