namespace Items
{
    public class AccessItem : Item
    {
        private readonly int _doorID;

        public AccessItem(string name, int weight, int doorId) : base(name, weight)
        {
            _doorID = doorId;
        }

        public bool OpensDoor(int id)
        {
            return _doorID == id;
        }

    }
}