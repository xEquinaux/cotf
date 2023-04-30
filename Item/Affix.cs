using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cotf.Base
{
    public class Trait
    {
        public string prefix;
        public string suffix;
        public int quality;
        public Item parent;
        public virtual int Quality(Item item)
        {
            return 0;
        }
        public virtual void Cursed(Item item, bool cursed)
        {
        }
        public virtual void Enchanted(Item item, bool enchanted)
        {
        }
        public virtual void RemoveEffect(Player player)
        {
            //  With things directly effecting player,
            //  should have buffer variables to compare with:
            //  A default player variables class perhaps
        }
        public virtual string GetName(Item item)
        {
            return item.name = $"{prefix} {item.Name} {suffix}";
        }
    }
    public interface IAffix
    {
        public abstract void Apply(Trait trait);
        public abstract void Effect(Item item);
        public abstract void Effect(Player player);
    }
    public class Prefix : Trait, IAffix
    {
        public virtual void Apply(Trait trait)
        {
        }

        public virtual void Effect(Item item)
        {
        }

        public virtual void Effect(Player player)
        {
        }
    }
    public class Suffix : Trait, IAffix
    {
        public virtual void Apply(Trait trait)
        {
        }

        public virtual void Effect(Item item)
        {
        }

        public virtual void Effect(Player player)
        {
        }
    }
}
