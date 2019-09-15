namespace DataAccess.Entity.Base
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}