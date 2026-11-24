using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class LoginRequestDto
{
    [Required, MaxLength(256)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string Password { get; set; } = string.Empty;
}