namespace Rahnemun.Common
{
    public class IdModel
    {
        public int Id { get; set; }

        public static implicit operator IdModel(int value)
        {
            return new IdModel { Id = value};
        }
    }
}
