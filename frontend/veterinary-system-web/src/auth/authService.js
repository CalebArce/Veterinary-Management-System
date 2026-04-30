import apiClient from "../api/apiClient";

export const login = async (email, password) => {
    const response = await apiClient.post("/Auth/login", {
        email,
        password
    });

    const data = response.data.data;

    localStorage.setItem("accessToken", data.accessToken);
    localStorage.setItem("refreshToken", data.refreshToken);
    localStorage.setItem("userEmail", data.email);
    localStorage.setItem("userRole", data.role);

    return data;
};

export const logout = async () => {
    const refreshToken = localStorage.getItem("refreshToken");

    if (refreshToken) {
        await apiClient.post("/Auth/logout", { refreshToken })
    }

    localStorage.clear();
};
