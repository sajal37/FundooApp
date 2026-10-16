namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public long ContactNo { get; set; }
        public string? PasswordHash { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public List<NoteEntity> Notes { get; set; } = new List<NoteEntity>();
    }
}
