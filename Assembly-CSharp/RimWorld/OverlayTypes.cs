using System;

namespace RimWorld
{
	[Flags]
	public enum OverlayTypes
	{
		NeedsPower = 1,
		PowerOff = 2,
		BurningWick = 4,
		Forbidden = 8,
		ForbiddenBig = 0x10,
		QuestionMark = 0x20,
		BrokenDown = 0x40,
		OutOfFuel = 0x80
	}
}
