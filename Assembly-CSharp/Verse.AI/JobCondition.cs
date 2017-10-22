namespace Verse.AI
{
	public enum JobCondition : byte
	{
		None = 0,
		Ongoing = 1,
		Succeeded = 2,
		Incompletable = 3,
		InterruptOptional = 4,
		InterruptForced = 5,
		Errored = 6,
		ErroredPather = 7
	}
}
