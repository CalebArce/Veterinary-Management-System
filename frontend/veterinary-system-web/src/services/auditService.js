import apiClient from "../api/apiClient";

// Stores security-sensitive actions performed within the system
// for traceability and auditing purposes
export const getAuditLogs = async () => {
  const response = await apiClient.get("/AuditLogs");
  return response.data.data;
};

export const getAuditLogsByEntity = async (entityName, entityId) => {
  const response = await apiClient.get(`/AuditLogs/${entityName}/${entityId}`);
  return response.data.data;
};