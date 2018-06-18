using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000507 RID: 1287
	public class Need_RoomSize : Need_Seeker
	{
		// Token: 0x0600171C RID: 5916 RVA: 0x000CB81D File Offset: 0x000C9C1D
		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x000CB854 File Offset: 0x000C9C54
		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x000CB870 File Offset: 0x000C9C70
		public RoomSizeCategory CurCategory
		{
			get
			{
				RoomSizeCategory result;
				if (this.CurLevel < 0.01f)
				{
					result = RoomSizeCategory.VeryCramped;
				}
				else if (this.CurLevel < 0.3f)
				{
					result = RoomSizeCategory.Cramped;
				}
				else if (this.CurLevel < 0.7f)
				{
					result = RoomSizeCategory.Normal;
				}
				else
				{
					result = RoomSizeCategory.Spacious;
				}
				return result;
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000CB8CC File Offset: 0x000C9CCC
		public float SpacePerceptibleNow()
		{
			float result;
			if (!this.pawn.Spawned)
			{
				result = 1f;
			}
			else
			{
				IntVec3 position = this.pawn.Position;
				Need_RoomSize.tempScanRooms.Clear();
				int i = 0;
				while (i < 5)
				{
					IntVec3 loc = position + GenRadial.RadialPattern[i];
					Room room = loc.GetRoom(this.pawn.Map, RegionType.Set_Passable);
					if (room != null)
					{
						if (i == 0 && room.PsychologicallyOutdoors)
						{
							return 1f;
						}
						if (i == 0 || room.RegionType != RegionType.Portal)
						{
							if (!Need_RoomSize.tempScanRooms.Contains(room))
							{
								Need_RoomSize.tempScanRooms.Add(room);
							}
						}
					}
					IL_C1:
					i++;
					continue;
					goto IL_C1;
				}
				float num = 0f;
				for (int j = 0; j < Need_RoomSize.SampleNumCells; j++)
				{
					IntVec3 loc2 = position + GenRadial.RadialPattern[j];
					if (Need_RoomSize.tempScanRooms.Contains(loc2.GetRoom(this.pawn.Map, RegionType.Set_Passable)))
					{
						num += 1f;
					}
				}
				Need_RoomSize.tempScanRooms.Clear();
				result = Need_RoomSize.RoomCellCountSpaceCurve.Evaluate(num);
			}
			return result;
		}

		// Token: 0x04000DB1 RID: 3505
		private static List<Room> tempScanRooms = new List<Room>();

		// Token: 0x04000DB2 RID: 3506
		private const float MinCramped = 0.01f;

		// Token: 0x04000DB3 RID: 3507
		private const float MinNormal = 0.3f;

		// Token: 0x04000DB4 RID: 3508
		private const float MinSpacious = 0.7f;

		// Token: 0x04000DB5 RID: 3509
		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

		// Token: 0x04000DB6 RID: 3510
		private static readonly SimpleCurve RoomCellCountSpaceCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0f),
				true
			},
			{
				new CurvePoint(9f, 0.25f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			},
			{
				new CurvePoint(42f, 0.71f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			}
		};
	}
}
