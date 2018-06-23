using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F62 RID: 3938
	public static class MatLoader
	{
		// Token: 0x04003E79 RID: 15993
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x06005F4E RID: 24398 RVA: 0x00309880 File Offset: 0x00307C80
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

		// Token: 0x02000F63 RID: 3939
		private struct Request
		{
			// Token: 0x04003E7A RID: 15994
			public string path;

			// Token: 0x04003E7B RID: 15995
			public int renderQueue;

			// Token: 0x06005F50 RID: 24400 RVA: 0x00309930 File Offset: 0x00307D30
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.path);
				return Gen.HashCombineInt(seed, this.renderQueue);
			}

			// Token: 0x06005F51 RID: 24401 RVA: 0x00309964 File Offset: 0x00307D64
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06005F52 RID: 24402 RVA: 0x00309998 File Offset: 0x00307D98
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06005F53 RID: 24403 RVA: 0x003099D8 File Offset: 0x00307DD8
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06005F54 RID: 24404 RVA: 0x003099F8 File Offset: 0x00307DF8
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06005F55 RID: 24405 RVA: 0x00309A18 File Offset: 0x00307E18
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
		}
	}
}
