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

		public bool alwaysShowWeapon = false;

		public bool neverShowWeapon = false;

		public bool suspendable = true;

		public bool casualInterruptible = true;

		public bool collideWithPawns = false;

		public bool isIdle = false;

		public TaleDef taleOnCompletion = null;

		public bool makeTargetPrisoner = false;

		public int joyDuration = 4000;

		public int joyMaxParticipants = 1;

		public float joyGainRate = 1f;

		public SkillDef joySkill = null;

		public float joyXpPerTick = 0f;

		public JoyKindDef joyKind = null;

		public Rot4 faceDir = Rot4.Invalid;

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.joySkill == null)
				yield break;
			if (this.joyXpPerTick != 0.0)
				yield break;
			yield return "funSkill is not null but funXpPerTick is zero";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0105:
			/*Error near IL_0106: Unexpected return in MoveNext()*/;
		}
	}
}
