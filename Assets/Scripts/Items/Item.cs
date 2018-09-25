namespace Items
{
    public abstract class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        protected Item(string name, int weight)
        {
            Name = name;
            Weight = weight;
        }
    }
}
