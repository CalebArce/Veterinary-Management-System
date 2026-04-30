import apiClient from "../api/apiClient";

export const getPets = async () => {
  const response = await apiClient.get("/Pets");
  return response.data.data;
};

export const getPetById = async (id) => {
  const response = await apiClient.get(`/Pets/${id}`);
  return response.data.data;
};

export const getPetsByClientId = async (clientId) => {
  const response = await apiClient.get(`/Pets/by-client/${clientId}`);
  return response.data.data;
};

export const createPet = async (pet) => {
  const response = await apiClient.post("/Pets", pet);
  return response.data.data;
};

export const updatePet = async (id, pet) => {
  const response = await apiClient.put(`/Pets/${id}`, pet);
  return response.data.data;
};

export const deletePet = async (id) => {
  const response = await apiClient.delete(`/Pets/${id}`);
  return response.data;
};