using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000446 RID: 1094
	public static class LightningBoltMeshPool
	{
		// Token: 0x04000B86 RID: 2950
		private static List<Mesh> boltMeshes = new List<Mesh>();

		// Token: 0x04000B87 RID: 2951
		private const int NumBoltMeshesMax = 20;

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x000A3F7C File Offset: 0x000A237C
		public static Mesh RandomBoltMesh
		{
			get
			{
				Mesh result;
				if (LightningBoltMeshPool.boltMeshes.Count < 20)
				{
					Mesh mesh = LightningBoltMeshMaker.NewBoltMesh();
					LightningBoltMeshPool.boltMeshes.Add(mesh);
					result = mesh;
				}
				else
				{
					result = LightningBoltMeshPool.boltMeshes.RandomElement<Mesh>();
				}
				return result;
			}
		}
	}
}
