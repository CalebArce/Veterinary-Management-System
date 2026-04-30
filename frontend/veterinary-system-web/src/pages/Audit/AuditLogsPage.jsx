import { useEffect, useMemo, useState } from "react";
import { getAuditLogs } from "../../services/auditService";
import { exportToExcel } from "../../utils/exportToExcel";
import { canExportReports } from "../../auth/roleUtil";
import "./AuditLogsPage.css";

const actionLabels = {
  1: "Login",
  2: "Registro de usuario",
  3: "Crear cliente",
  4: "Actualizar cliente",
  5: "Eliminar cliente",
  6: "Crear mascota",
  7: "Actualizar mascota",
  8: "Eliminar mascota",
  9: "Crear tipo de servicio",
  10: "Actualizar tipo de servicio",
  11: "Eliminar tipo de servicio",
  12: "Asignar servicio",
  13: "Actualizar facturación",
};

export default function AuditLogsPage() {
  const [logs, setLogs] = useState([]);
  const [search, setSearch] = useState("");
  const [actionFilter, setActionFilter] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadLogs = async () => {
      try {
        setLoading(true);
        setError("");

        const data = await getAuditLogs();
        setLogs(data);
      } catch (err) {
        setError("No se pudieron cargar los registros de auditoría");
        console.error("Error cargando auditoría:", err);
      } finally {
        setLoading(false);
      }
    };

    loadLogs();
  }, []);

  const filteredLogs = useMemo(() => {
    return logs.filter((log) => {
      const text = `${log.userEmail ?? ""} ${log.userRole ?? ""} ${
        log.entityName ?? ""
      } ${log.description ?? ""} ${log.ipAddress ?? ""}`.toLowerCase();

      const matchesSearch = text.includes(search.toLowerCase());

      const matchesAction = actionFilter
        ? String(log.action) === actionFilter
        : true;

      return matchesSearch && matchesAction;
    });
  }, [logs, search, actionFilter]);

  const handleExportAudit = () => {
    const data = filteredLogs.map((log) => ({
      Fecha: new Date(log.createdAt).toLocaleString(),
      Accion: actionLabels[log.action] ?? log.action,
      Entidad: `${log.entityName}${log.entityId ? ` #${log.entityId}` : ""}`,
      Usuario: log.userEmail || "Anonymous",
      Rol: log.userRole || "Unknown",
      IP: log.ipAddress || "N/A",
      Descripcion: log.description || "Sin descripción",
    }));

    exportToExcel(data, "auditoria_sistema", "Auditoria");
  };

  return (
    <main className="audit-page">
      <header className="audit-header">
        <div>
          <h1 className="audit-title">Auditoría</h1>
          <p className="audit-subtitle">
            Registro de acciones importantes realizadas dentro del sistema.
          </p>
        </div>

        {canExportReports() && (
          <button className="export-button" onClick={handleExportAudit}>
            Exportar Excel
          </button>
        )}
      </header>

      <section className="audit-card">
        <div className="audit-filters">
          <input
            type="text"
            placeholder="Buscar por usuario, entidad, IP o descripción..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />

          <select
            value={actionFilter}
            onChange={(e) => setActionFilter(e.target.value)}
          >
            <option value="">Todas las acciones</option>
            {Object.entries(actionLabels).map(([value, label]) => (
              <option key={value} value={value}>
                {label}
              </option>
            ))}
          </select>
        </div>

        {error && <div className="audit-error">{error}</div>}

        {loading ? (
          <div className="empty-message">Cargando auditoría...</div>
        ) : (
          <div className="table-wrapper">
            <table className="audit-table">
              <thead>
                <tr>
                  <th>Fecha</th>
                  <th>Acción</th>
                  <th>Entidad</th>
                  <th>Usuario</th>
                  <th>Rol</th>
                  <th>IP</th>
                  <th>Descripción</th>
                </tr>
              </thead>

              <tbody>
                {filteredLogs.length === 0 ? (
                  <tr>
                    <td colSpan="7" className="empty-message">
                      No hay registros de auditoría para mostrar.
                    </td>
                  </tr>
                ) : (
                  filteredLogs.map((log) => (
                    <tr key={log.id}>
                      <td>{new Date(log.createdAt).toLocaleString()}</td>
                      <td>
                        <span className="audit-action-badge">
                          {actionLabels[log.action] ?? log.action}
                        </span>
                      </td>
                      <td>
                        {log.entityName}
                        {log.entityId ? ` #${log.entityId}` : ""}
                      </td>
                      <td>{log.userEmail || "Anonymous"}</td>
                      <td>{log.userRole || "Unknown"}</td>
                      <td>{log.ipAddress || "N/A"}</td>
                      <td>{log.description || "Sin descripción"}</td>
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