namespace Assets.Items
{
    public abstract class RestorationItem : IItem
    {
        public string Name { get; }
        //private FighterStat stat;

        public RestorationItem(string name)
        {
            Name = name;
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}