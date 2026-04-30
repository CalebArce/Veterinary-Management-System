namespace VeterinarySystem.Application.Security;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Veterinarian = "Veterinarian";
    public const string Assistant = "Assistant";

    public const string AdminOrVeterinarian = Admin + "," + Veterinarian;

    public const string AdminVeterinarianOrAssistant = Admin + "," + Veterinarian + "," + Assistant;
}