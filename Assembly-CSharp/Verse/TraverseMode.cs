namespace Verse
{
	public enum TraverseMode : byte
	{
		ByPawn = 0,
		PassDoors = 1,
		NoPassClosedDoors = 2,
		PassAllDestroyableThings = 3,
		NoPassClosedDoorsOrWater = 4,
		PassAllDestroyableThingsNotWater = 5
	}
}
