namespace VeterinarySystem.Domain.Enums;

public enum AuditAction
{
    Login = 1,
    RegisterUser = 2,
    CreateClient = 3,
    UpdateClient = 4,
    DeleteClient = 5,
    CreatePet = 6,
    UpdatePet = 7,
    DeletePet = 8,
    CreateTypeService = 9,
    UpdateTypeService = 10,
    DeleteTypeService = 11,
    CreatePetService = 12,
    UpdateBillingStatus = 13
}