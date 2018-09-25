
namespace Items
{
    public class BonusItem : Item
    {
        public int Value { get; }

        public BonusItem(string name, int weight, int value) : base(name, weight)
        {
            Value = value;
        }
    }
}
