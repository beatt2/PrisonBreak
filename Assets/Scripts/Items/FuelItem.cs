namespace Items
{
    public class FuelItem : Item
    {
        public float FuelValue;

        public FuelItem(string name, int weight, float fuelValue) : base(name, weight)
        {
            FuelValue = fuelValue;
        }
    }
}
