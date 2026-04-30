import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

export default function BillingStatusChart({ summary }) {
  const data = [
    {
      name: "Pendientes",
      cantidad: summary?.pendingServices ?? 0,
    },
    {
      name: "Facturados",
      cantidad: summary?.invoicedServices ?? 0,
    },
    {
      name: "Pagados",
      cantidad: summary?.paidServices ?? 0,
    },
    {
      name: "Cancelados",
      cantidad: summary?.cancelledServices ?? 0,
    },
  ];

  return (
    <div className="chart-card">
      <h3>Servicios por estado</h3>

      <div className="chart-container">
        <ResponsiveContainer width="100%" height="100%">
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="name" />
            <YAxis allowDecimals={false} />
            <Tooltip />
            <Bar dataKey="cantidad" />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}