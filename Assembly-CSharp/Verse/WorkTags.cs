using System;

namespace Verse
{
	[Flags]
	public enum WorkTags
	{
		None = 0,
		ManualDumb = 2,
		ManualSkilled = 4,
		Violent = 8,
		Caring = 0x10,
		Social = 0x20,
		Intellectual = 0x40,
		Animals = 0x80,
		Artistic = 0x100,
		Crafting = 0x200,
		Cooking = 0x400,
		Firefighting = 0x800,
		Cleaning = 0x1000,
		Hauling = 0x2000,
		PlantWork = 0x4000,
		Mining = 0x8000
	}
}
