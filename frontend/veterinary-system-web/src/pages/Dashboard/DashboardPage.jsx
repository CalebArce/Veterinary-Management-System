import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { deleteClient, getClients } from "../../services/clientService";
import {
  getDashboardSummary,
  getMonthlyRevenue,
} from "../../services/dashboardService";
import {
  canDeleteClients,
  canManagePetServices,
  canManageTypeServices,
} from "../../auth/roleUtils";
import BillingStatusChart from "../../components/charts/BillingStatusChart";
import RevenueChart from "../../components/charts/RevenueChart";
import SystemDistributionChart from "../../components/charts/SystemDistributionChart";
import MonthlyRevenueChart from "../../components/charts/MonthlyRevenueChart";
import "./DashboardPage.css";

export default function DashboardPage() {
  const navigate = useNavigate();

  const [clients, setClients] = useState([]);
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [monthlyRevenue, setMonthlyRevenue] = useState([]);

  const refreshDashboard = async (start = startDate, end = endDate) => {
    const clientsData = await getClients();
    const summaryData = await getDashboardSummary(start, end);
    const monthlyRevenueData = await getMonthlyRevenue(start, end);

    setClients(clientsData);
    setSummary(summaryData);
    setMonthlyRevenue(monthlyRevenueData);
  };

  useEffect(() => {
    const loadDashboard = async () => {
      try {
        setLoading(true);
        setError("");

        await refreshDashboard("", "");
      } catch (err) {
        console.error("Error completo:", err);
        console.error("Respuesta backend:", err.response?.data);
        console.error("Status:", err.response?.status);

        setError(
          err.response?.data?.message ||
            `No se pudo cargar la información del dashboard. Status: ${err.response?.status}`
        );
      }finally {
        setLoading(false);
      }
    };

    loadDashboard();
  }, []);

  const handleFilter = async () => {
    try {
      setLoading(true);
      setError("");

      await refreshDashboard();
    } catch (err) {
      setError("No se pudo aplicar el filtro de fechas.");
      console.error("Error filtrando dashboard:", err);
    } finally {
      setLoading(false);
    }
  };

  const handleClearFilter = async () => {
    try {
      setLoading(true);
      setError("");

      setStartDate("");
      setEndDate("");

      await refreshDashboard("", "");
    } catch (err) {
      setError("No se pudo limpiar el filtro.");
      console.error("Error limpiando filtro:", err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm(
      "¿Está seguro de que desea eliminar este cliente?"
    );

    if (!confirmDelete) return;

    try {
      await deleteClient(id);
      await refreshDashboard();
    } catch (err) {
      alert(err.response?.data?.message || "No se pudo eliminar el cliente");
    }
  };

  return (
    <main className="dashboard-page">
      <header className="dashboard-header">
        <div>
          <h1 className="dashboard-title">Dashboard</h1>
          <p className="dashboard-user">
            Bienvenido, {localStorage.getItem("userEmail")}
          </p>
        </div>
      </header>

      <section className="dashboard-filters">
        <div>
          <label>Fecha inicio</label>
          <input
            type="date"
            value={startDate}
            onChange={(event) => setStartDate(event.target.value)}
          />
        </div>

        <div>
          <label>Fecha fin</label>
          <input
            type="date"
            value={endDate}
            onChange={(event) => setEndDate(event.target.value)}
          />
        </div>

        <button className="filter-button" onClick={handleFilter}>
          Filtrar
        </button>

        <button className="secondary-button" onClick={handleClearFilter}>
          Limpiar
        </button>
      </section>

      {summary && (
        <section className="metrics-grid">
          <div className="metric-card">
            <span>Clientes</span>
            <strong>{summary.totalClients}</strong>
          </div>

          <div className="metric-card">
            <span>Mascotas</span>
            <strong>{summary.totalPets}</strong>
          </div>

          <div className="metric-card">
            <span>Tipos de servicio</span>
            <strong>{summary.totalTypeServices}</strong>
          </div>

          <div className="metric-card">
            <span>Servicios asignados</span>
            <strong>{summary.totalPetServices}</strong>
          </div>

          <div className="metric-card">
            <span>Pendientes</span>
            <strong>{summary.pendingServices}</strong>
          </div>

          <div className="metric-card">
            <span>Pagados</span>
            <strong>{summary.paidServices}</strong>
          </div>

          <div className="metric-card">
            <span>Ingresos estimados</span>
            <strong>
              ₡{Number(summary.estimatedRevenue).toLocaleString()}
            </strong>
          </div>

          <div className="metric-card">
            <span>Ingresos pagados</span>
            <strong>₡{Number(summary.paidRevenue).toLocaleString()}</strong>
          </div>
        </section>
      )}

          {monthlyRevenue.length > 0 && (
            <section className="monthly-chart-section">
              <MonthlyRevenueChart data={monthlyRevenue} />
            </section>
          )}

      {summary && (
        <section className="charts-grid">
          <BillingStatusChart summary={summary} />
          <RevenueChart summary={summary} />
          <SystemDistributionChart summary={summary} />
        </section>
      )}

      <section className="dashboard-card">
        <div className="card-header">
          <div>
            <h2 className="card-title">Clientes registrados</h2>
            <p className="card-subtitle">
              Gestión de clientes del sistema veterinario.
            </p>
          </div>

          <div className="dashboard-actions">
            <button
              className="new-client-button"
              onClick={() => navigate("/clients/new")}
            >
              + Nuevo cliente
            </button>

            <button
              className="new-client-button"
              onClick={() => navigate("/pets")}
            >
              Ver mascotas
            </button>

            {canManageTypeServices() && (
              <button
                className="new-client-button"
                onClick={() => navigate("/type-services")}
              >
                Tipos de servicio
              </button>
            )}

            {canManagePetServices() && (
              <button
                className="new-client-button"
                onClick={() => navigate("/pet-services")}
              >
                Servicios asignados
              </button>
            )}
          </div>
        </div>

        {error && <div className="table-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando información...</div>
        ) : (
          <div className="table-wrapper">
            <table className="clients-table">
              <thead>
                <tr>
                  <th>Identificación</th>
                  <th>Nombre</th>
                  <th>Teléfono</th>
                  <th>Correo</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {clients.length === 0 ? (
                  <tr>
                    <td colSpan="6" className="empty-message">
                      No hay clientes registrados.
                    </td>
                  </tr>
                ) : (
                  clients.map((client) => (
                    <tr key={client.id}>
                      <td>{client.identification}</td>
                      <td>{client.fullName}</td>
                      <td>{client.phone}</td>
                      <td>{client.email}</td>
                      <td>
                        <span
                          className={
                            client.isActive
                              ? "status-badge active"
                              : "status-badge inactive"
                          }
                        >
                          {client.isActive ? "Activo" : "Inactivo"}
                        </span>
                      </td>
                      <td className="actions-cell">
                        <button
                          className="edit-button"
                          onClick={() => navigate(`/clients/${client.id}/edit`)}
                        >
                          Editar
                        </button>

                        {canDeleteClients() && (
                          <button
                            className="delete-button"
                            onClick={() => handleDelete(client.id)}
                          >
                            Eliminar
                          </button>
                        )}
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        )}
      </section>
    </main>
  );
}