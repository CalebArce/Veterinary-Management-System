import apiClient from "../api/apiClient";

export const getTypeServices = async () => {
  const response = await apiClient.get("/TypeServices");
  return response.data.data;
};

export const getTypeServiceById = async (id) => {
  const response = await apiClient.get(`/TypeServices/${id}`);
  return response.data.data;
};

export const createTypeService = async (typeService) => {
  const response = await apiClient.post("/TypeServices", typeService);
  return response.data.data;
};

export const updateTypeService = async (id, typeService) => {
  const response = await apiClient.put(`/TypeServices/${id}`, typeService);
  return response.data.data;
};

export const deleteTypeService = async (id) => {
  const response = await apiClient.delete(`/TypeServices/${id}`);
  return response.data;
};