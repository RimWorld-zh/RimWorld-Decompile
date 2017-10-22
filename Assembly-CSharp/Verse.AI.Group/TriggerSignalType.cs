namespace Verse.AI.Group
{
	public enum TriggerSignalType : byte
	{
		Undefined = 0,
		Tick = 1,
		Memo = 2,
		PawnDamaged = 3,
		PawnArrestAttempted = 4,
		PawnLost = 5,
		BuildingDamaged = 6,
		FactionRelationsChanged = 7
	}
}
