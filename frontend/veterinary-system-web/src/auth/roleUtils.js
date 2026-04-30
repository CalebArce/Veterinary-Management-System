export const getCurrentRole = () => {
  return localStorage.getItem("userRole");
};

export const isAdmin = () => {
  return getCurrentRole() === "Admin";
};

export const isVeterinarian = () => {
  return getCurrentRole() === "Veterinarian";
};

export const isAssistant = () => {
  return getCurrentRole() === "Assistant";
};

export const canManageTypeServices = () => {
  return isAdmin();
};

export const canViewAuditLogs = () => {
  return isAdmin();
};

export const canDeleteClients = () => {
  return isAdmin();
};

export const canExportReports = () => {
  return isAdmin();
};

export const canManagePetServices = () => {
  return isAdmin() || isVeterinarian();
};

export const canChangeBillingStatus = () => {
  return isAdmin() || isVeterinarian();
};

export const canCreateClients = () => {
  return isAdmin() || isAssistant();
};

export const canCreatePets = () => {
  return isAdmin() || isAssistant();
};