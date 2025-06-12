namespace BaseLibrary.DTOs
{
    /// <summary>
    /// Inmutable class that represents the claims of a user.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    /// <param name="Role"></param>
    public record CustomUserClaims(string Id = null!, string Name = null!, string Email = null!, string Role = null!);
}
