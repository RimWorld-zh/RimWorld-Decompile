using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class JobDef : Def
	{
		public Type driverClass;

		[MustTranslate]
		public string reportString = "Doing something.";

		public bool playerInterruptible = true;

		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		public bool alwaysShowWeapon;

		public bool neverShowWeapon;

		public bool suspendable = true;

		public bool casualInterruptible = true;

		public bool collideWithPawns;

		public bool makeTargetPrisoner;

		public int joyDuration = 4000;

		public int joyMaxParticipants = 1;

		public float joyGainRate = 1f;

		public SkillDef joySkill;

		public float joyXpPerTick;

		public JoyKindDef joyKind;

		public Rot4 faceDir = Rot4.Invalid;

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.joySkill != null && this.joyXpPerTick == 0.0)
			{
				yield return "funSkill is not null but funXpPerTick is zero";
			}
		}
	}
}
