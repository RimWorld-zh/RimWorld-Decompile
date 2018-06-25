using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FB7 RID: 4023
	[HasDebugOutput]
	public struct ShootLine
	{
		// Token: 0x04003F92 RID: 16274
		private IntVec3 source;

		// Token: 0x04003F93 RID: 16275
		private IntVec3 dest;

		// Token: 0x0600614D RID: 24909 RVA: 0x003128C3 File Offset: 0x00310CC3
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x0600614E RID: 24910 RVA: 0x003128D4 File Offset: 0x00310CD4
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x0600614F RID: 24911 RVA: 0x003128F0 File Offset: 0x00310CF0
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x0031290C File Offset: 0x00310D0C
		public void ChangeDestToMissWild(float aimOnChance)
		{
			float num = ShootTunings.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089, false);
			}
			IntVec3 a;
			do
			{
				Vector2 unitVector = Rand.UnitVector2;
				Vector3 b = new Vector3(unitVector.x * num, 0f, unitVector.y * num);
				a = (this.dest.ToVector3Shifted() + b).ToIntVec3();
			}
			while (Vector3.Dot((this.dest - this.source).ToVector3(), (a - this.source).ToVector3()) < 0f);
			this.dest = a;
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x003129D8 File Offset: 0x00310DD8
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x00312A00 File Offset: 0x00310E00
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.source,
				"->",
				this.dest,
				")"
			});
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x00312A54 File Offset: 0x00310E54
		[DebugOutput]
		public static void WildMissResults()
		{
			IntVec3 intVec = new IntVec3(100, 0, 0);
			ShootLine shootLine = new ShootLine(IntVec3.Zero, intVec);
			IEnumerable<int> enumerable = Enumerable.Range(0, 101);
			IEnumerable<int> colValues = Enumerable.Range(0, 12);
			int[,] results = new int[enumerable.Count<int>(), colValues.Count<int>()];
			foreach (int num in enumerable)
			{
				for (int i = 0; i < 10000; i++)
				{
					ShootLine shootLine2 = shootLine;
					shootLine2.ChangeDestToMissWild((float)num / 100f);
					if (shootLine2.dest.z == 0 && shootLine2.dest.x > intVec.x)
					{
						results[num, shootLine2.dest.x - intVec.x]++;
					}
				}
			}
			DebugTables.MakeTablesDialog<int, int>(colValues, (int cells) => cells.ToString() + "-away\ncell\nhit%", enumerable, (int hitchance) => ((float)hitchance / 100f).ToStringPercent() + " aimon chance", delegate(int cells, int hitchance)
			{
				float num2 = (float)hitchance / 100f;
				string result;
				if (cells == 0)
				{
					result = num2.ToStringPercent();
				}
				else
				{
					result = ((float)results[hitchance, cells] / 10000f * (1f - num2)).ToStringPercent();
				}
				return result;
			}, "");
		}
	}
}
