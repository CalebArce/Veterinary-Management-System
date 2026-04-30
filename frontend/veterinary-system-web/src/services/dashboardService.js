import apiClient from "../api/apiClient";

export const getDashboardSummary = async (startDate, endDate) => {
  let url = "/Dashboard/summary";

  const params = [];

  if (startDate) params.push(`startDate=${startDate}`);
  if (endDate) params.push(`endDate=${endDate}`);

  if (params.length > 0) {
    url += `?${params.join("&")}`;
  }

  const response = await apiClient.get(url);
  return response.data.data;
};

export const getMonthlyRevenue = async (startDate, endDate) => {
  let url = "/Dashboard/monthly-revenue";

  const params = [];

  if (startDate) params.push(`startDate=${startDate}`);
  if (endDate) params.push(`endDate=${endDate}`);

  if (params.length > 0) {
    url += `?${params.join("&")}`;
  }

  const response = await apiClient.get(url);
  return response.data.data;
};