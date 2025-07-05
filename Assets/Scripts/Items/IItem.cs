namespace Assets.Items
{
    public interface IItem
    {
        string Name { get; }
        void Use();
    }
}