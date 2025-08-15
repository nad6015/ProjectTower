namespace Assets.DungeonGenerator.Components
{
    public struct Range<T>
    {
        public T min;
        public T max;

        public Range(T min, T max) : this()
        {
            this.min = min;
            this.max = max;
        }
    }

}