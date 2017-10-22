using System.Linq;

namespace Verse.AI.Group
{
	public class TransitionAction_Message : TransitionAction
	{
		public string message;

		public MessageSound sound;

		public TargetInfo lookTarget = TargetInfo.Invalid;

		public TransitionAction_Message(string message) : this(message, MessageSound.Standard)
		{
			this.message = message;
		}

		public TransitionAction_Message(string message, MessageSound messageSound)
		{
			this.message = message;
			this.sound = messageSound;
		}

		public TransitionAction_Message(string message, MessageSound messageSound, TargetInfo lookTarget)
		{
			this.message = message;
			this.sound = messageSound;
			this.lookTarget = lookTarget;
		}

		public override void DoAction(Transition trans)
		{
			TargetInfo target = (!this.lookTarget.IsValid) ? ((Thing)trans.target.lord.ownedPawns.FirstOrDefault()) : this.lookTarget;
			Messages.Message(this.message, target, this.sound);
		}
	}
}
