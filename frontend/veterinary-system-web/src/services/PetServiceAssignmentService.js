import apiClient from "../api/apiClient";

export const getPetServices = async () => {
    const response = await apiClient.get("/PetServices");
    return response.data.data;
};

export const getPetServiceById = async (id) => {
    const response = await apiClient.get(`/PetServices/${id}`);
    return response.data.data;
};

export const getPetServicesByPetId = async (petId) => {
    const response = await apiClient.get(`/PetServices/by-pet/${petId}`);
    return response.data.data;
};

export const createPetService = async(petService) => {
    const response = await apiClient.post("/PetServices", petService);
    return response.data.data;
};

export const updateBillingStatus = async(id, billingStatus) => {
    const response = await apiClient.patch(`/PetServices/${id}/billing-status`, {billingStatus,});
    return response.data;
};