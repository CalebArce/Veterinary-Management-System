import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

export default function RevenueChart({ summary }) {
  const data = [
    {
      name: "Estimado",
      monto: summary?.estimatedRevenue ?? 0,
    },
    {
      name: "Pagado",
      monto: summary?.paidRevenue ?? 0,
    },
  ];

  return (
    <div className="chart-card">
      <h3>Ingresos</h3>

      <div className="chart-container">
        <ResponsiveContainer width="100%" height="100%">
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip formatter={(value) => `₡${Number(value).toLocaleString()}`} />
            <Bar dataKey="monto" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}