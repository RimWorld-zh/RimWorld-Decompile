using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A01 RID: 2561
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x0400249A RID: 9370
		public string message;

		// Token: 0x0400249B RID: 9371
		public MessageTypeDef type;

		// Token: 0x0400249C RID: 9372
		public TargetInfo lookTarget;

		// Token: 0x0400249D RID: 9373
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x0400249E RID: 9374
		public string repeatAvoiderTag;

		// Token: 0x0400249F RID: 9375
		public float repeatAvoiderSeconds;

		// Token: 0x06003975 RID: 14709 RVA: 0x001E7CE7 File Offset: 0x001E60E7
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds)
		{
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x001E7CF8 File Offset: 0x001E60F8
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x001E7D29 File Offset: 0x001E6129
		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x001E7D62 File Offset: 0x001E6162
		public TransitionAction_Message(string message, MessageTypeDef messageType, Func<TargetInfo> lookTargetGetter, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTargetGetter = lookTargetGetter;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x001E7D9C File Offset: 0x001E619C
		public override void DoAction(Transition trans)
		{
			if (this.repeatAvoiderTag.NullOrEmpty() || MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
			{
				TargetInfo target = (this.lookTargetGetter == null) ? this.lookTarget : this.lookTargetGetter();
				if (!target.IsValid)
				{
					target = trans.target.lord.ownedPawns.FirstOrDefault<Pawn>();
				}
				Messages.Message(this.message, target, this.type, true);
			}
		}
	}
}
