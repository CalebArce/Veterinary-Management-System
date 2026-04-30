import apiClient from "../api/apiClient";

export const getClients = async () => {
  const response = await apiClient.get("/Clients");
  return response.data.data;
};

export const getClientById = async (id) => {
  const response = await apiClient.get(`/Clients/${id}`);
  return response.data.data;
};

export const createClient = async (client) => {
  const response = await apiClient.post("/Clients", client);
  return response.data.data;
};

export const updateClient = async (id, client) => {
  const response = await apiClient.put(`/Clients/${id}`, client);
  return response.data.data;
};

export const deleteClient = async (id) => {
  const response = await apiClient.delete(`/Clients/${id}`);
  return response.data;
};