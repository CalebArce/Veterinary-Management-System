import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

export default function SystemDistributionChart({ summary }) {
  const data = [
    {
      name: "Clientes",
      total: summary?.totalClients ?? 0,
    },
    {
      name: "Mascotas",
      total: summary?.totalPets ?? 0,
    },
    {
      name: "Servicios",
      total: summary?.totalPetServices ?? 0,
    },
    {
      name: "Tipos",
      total: summary?.totalTypeServices ?? 0,
    },
  ];

  return (
    <div className="chart-card">
      <h3>Distribución general</h3>

      <div className="chart-container">
        <ResponsiveContainer width="100%" height="100%">
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis allowDecimals={false} />
            <Tooltip />
            <Bar dataKey="total" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}