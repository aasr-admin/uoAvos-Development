using Server.Items;

using System;

namespace Server.Spells
{
	public enum SpellCircle
	{
		First,
		Second,
		Third,
		Fourth,
		Fifth,
		Sixth,
		Seventh,
		Eighth
	}

	public abstract class MagerySpell : Spell
	{
		public MagerySpell(Mobile caster, Item scroll, SpellInfo info)
			: base(caster, scroll, info)
		{
		}

		public abstract SpellCircle Circle { get; }

		public override bool ConsumeReagents()
		{
			if (base.ConsumeReagents())
			{
				return true;
			}

			if (ArcaneGem.ConsumeCharges(Caster, (Core.SE ? 1 : 1 + (int)Circle)))
			{
				return true;
			}

			return false;
		}

		private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;

		public override void GetCastSkills(out double min, out double max)
		{
			var circle = (int)Circle;

			if (Scroll != null)
			{
				circle -= 2;
			}

			var avg = ChanceLength * circle;

			min = avg - ChanceOffset;
			max = avg + ChanceOffset;
		}

		private static readonly int[] m_ManaTable = new int[] { 4, 6, 9, 11, 14, 20, 40, 50 };

		public override int GetMana()
		{
			if (Scroll is BaseWand)
			{
				return 0;
			}

			return m_ManaTable[(int)Circle];
		}

		public override double GetResistSkill(Mobile m)
		{
			var maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if (m.Skills[SkillName.MagicResist].Value < maxSkill)
			{
				m.CheckSkill(SkillName.MagicResist, 0.0, m.Skills[SkillName.MagicResist].Cap);
			}

			return m.Skills[SkillName.MagicResist].Value;
		}

		public virtual bool CheckResisted(Mobile target)
		{
			var n = GetResistPercent(target);

			n /= 100.0;

			if (n <= 0.0)
			{
				return false;
			}

			if (n >= 1.0)
			{
				return true;
			}

			var maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if (target.Skills[SkillName.MagicResist].Value < maxSkill)
			{
				target.CheckSkill(SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap);
			}

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
		{
			var firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			var secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent(Mobile target)
		{
			return GetResistPercentForCircle(target, Circle);
		}

		public override TimeSpan GetCastDelay()
		{
			if (!Core.ML && Scroll is BaseWand)
			{
				return TimeSpan.Zero;
			}

			if (!Core.AOS)
			{
				return TimeSpan.FromSeconds(0.5 + (0.25 * (int)Circle));
			}

			return base.GetCastDelay();
		}

		public override TimeSpan CastDelayBase => TimeSpan.FromSeconds((3 + (int)Circle) * CastDelaySecondsPerTick);
	}
}