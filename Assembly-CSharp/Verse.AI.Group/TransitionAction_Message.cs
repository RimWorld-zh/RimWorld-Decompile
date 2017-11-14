using RimWorld;
using System.Linq;

namespace Verse.AI.Group
{
	public class TransitionAction_Message : TransitionAction
	{
		public string message;

		public MessageTypeDef type;

		public TargetInfo lookTarget = TargetInfo.Invalid;

		public TransitionAction_Message(string message)
			: this(message, MessageTypeDefOf.NeutralEvent)
		{
			this.message = message;
		}

		public TransitionAction_Message(string message, MessageTypeDef messageType)
		{
			this.message = message;
			this.type = messageType;
		}

		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget)
		{
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
		}

		public override void DoAction(Transition trans)
		{
			TargetInfo target = (!this.lookTarget.IsValid) ? ((TargetInfo)trans.target.lord.ownedPawns.FirstOrDefault()) : this.lookTarget;
			Messages.Message(this.message, target, this.type);
		}
	}
}
