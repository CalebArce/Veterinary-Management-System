// Utility for generating downloadable Excel reports from system data

import * as XLSX from "xlsx";

export const exportToExcel = (data, fileName, sheetName = "Reporte") => {
  if (!data || data.length === 0) {
    alert("No hay datos para exportar.");
    return;
  }

  const worksheet = XLSX.utils.json_to_sheet(data);
  const workbook = XLSX.utils.book_new();

  XLSX.utils.book_append_sheet(workbook, worksheet, sheetName);
  XLSX.writeFile(workbook, `${fileName}.xlsx`);
};