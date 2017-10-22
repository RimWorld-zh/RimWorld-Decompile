namespace Verse.AI.Group
{
	public enum PawnLostCondition : byte
	{
		Undefined = 0,
		Vanished = 1,
		IncappedOrKilled = 2,
		MadePrisoner = 3,
		ChangedFaction = 4,
		ExitedMap = 5,
		LeftVoluntarily = 6,
		Drafted = 7,
		ForcedToJoinOtherLord = 8
	}
}
