namespace BaseLibrary.Responses
{
    /// <summary>
    /// Inmutable class that represents the response of a login request.
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="Token"></param>
    /// <param name="RefreshToken"></param>
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!, string RefreshToken = null!);
}
