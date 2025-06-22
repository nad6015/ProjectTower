namespace Assets.Items
{
    interface IItem
    {
        string Name { get; }
        void Use();
    }
}