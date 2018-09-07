using System;
using System.Runtime.CompilerServices;
using Verse.Sound;

namespace Verse.AI
{
	public static class Toils_Effects
	{
		public static Toil MakeSound(SoundDef soundDef)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return toil;
		}

		[CompilerGenerated]
		private sealed class <MakeSound>c__AnonStorey0
		{
			internal Toil toil;

			internal SoundDef soundDef;

			public <MakeSound>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				this.soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			}
		}
	}
}
