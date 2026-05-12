import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";

// Displays monthly revenue trends using line charts
export default function MonthlyRevenueChart({ data }) {
  return (
    <div className="chart-card wide-chart">
      <h3>Ingresos por mes</h3>

      <div className="chart-container">
        <ResponsiveContainer width="100%" height="100%">
          <LineChart data={data}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="month" />
            <YAxis />
            <Tooltip
              formatter={(value) => `₡${Number(value).toLocaleString()}`}
            />
            <Legend />
            <Line
              type="monotone"
              dataKey="estimatedRevenue"
              name="Ingresos estimados"
            />
            <Line
              type="monotone"
              dataKey="paidRevenue"
              name="Ingresos pagados"
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}