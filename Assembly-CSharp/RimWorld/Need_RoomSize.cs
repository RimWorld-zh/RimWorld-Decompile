using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Need_RoomSize : Need_Seeker
	{
		private static List<Room> tempScanRooms = new List<Room>();

		private const float MinCramped = 0.01f;

		private const float MinNormal = 0.3f;

		private const float MinSpacious = 0.7f;

		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

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

		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Need_RoomSize()
		{
		}
	}
}
